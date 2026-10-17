using VendingMachine.Api.Domain.Exceptions;

namespace VendingMachine.Api.Domain.Entities;

public class Shelf
{
    private readonly object _sync = new();
    private readonly List<InventoryItem> _inventory = new();

    public Guid Id { get; }
    public string ProductType { get; }
    public int Capacity { get; }

    public int CurrentQuantity
    {
        get
        {
            lock (_sync)
            {
                return _inventory.Sum(item => item.Quantity);
            }
        }
    }

    public IReadOnlyCollection<InventoryItem> Inventory
    {
        get
        {
            lock (_sync)
            {
                // Return a snapshot instead of exposing the mutable inventory.
                return _inventory
                    .Select(item => new InventoryItem(item.Product, item.Quantity))
                    .ToArray();
            }
        }
    }

    /// <summary>
    /// Creates an empty shelf with a product category and capacity.
    /// </summary>
    public Shelf(string productType, int capacity)
    {
        if (string.IsNullOrWhiteSpace(productType))
            throw new ArgumentException(
                "Product type is required.", nameof(productType));

        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(capacity), "Capacity must be greater than zero.");

        Id = Guid.NewGuid();
        ProductType = productType.Trim();
        Capacity = capacity;
    }

    /// <summary>
    /// Loads matching products without exceeding the shelf capacity.
    /// </summary>
    public void LoadProducts(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity), "Quantity must be greater than zero.");

        lock (_sync)
        {
            if (!string.Equals(product.ProductType, ProductType,
                    StringComparison.OrdinalIgnoreCase))
                throw new BusinessRuleException(
                    "Product type does not match shelf type.");

            if (quantity > Capacity - _inventory.Sum(item => item.Quantity))
                throw new BusinessRuleException("Shelf capacity exceeded.");

            var existingItem = _inventory.FirstOrDefault(item =>
                string.Equals(item.Product.Code, product.Code,
                    StringComparison.OrdinalIgnoreCase));

            if (existingItem != null)
            {
                if (!string.Equals(existingItem.Product.Name, product.Name,
                        StringComparison.OrdinalIgnoreCase))
                    throw new BusinessRuleException(
                        "Product code already exists with a different name.");

                existingItem.AddQuantity(quantity);
            }
            else
            {
                _inventory.Add(new InventoryItem(product, quantity));
            }
        }
    }

    /// <summary>
    /// Removes purchased units from the shelf when enough stock exists.
    /// </summary>
    public void PurchaseProduct(string productCode, int quantity)
    {
        if (string.IsNullOrWhiteSpace(productCode))
            throw new ArgumentException(
                "Product code is required.", nameof(productCode));

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity), "Quantity must be greater than zero.");

        lock (_sync)
        {
            var item = _inventory.FirstOrDefault(i =>
                string.Equals(i.Product.Code, productCode,
                    StringComparison.OrdinalIgnoreCase));

            if (item == null)
                throw new BusinessRuleException("Product not found on shelf.");

            if (item.Quantity < quantity)
                throw new BusinessRuleException("Insufficient inventory.");

            item.RemoveQuantity(quantity);

            if (item.Quantity == 0)
                _inventory.Remove(item);
        }
    }
}
