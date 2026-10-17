namespace VendingMachine.Api.Domain.Entities;

public class InventoryItem
{
    public Product Product { get; }

    public int Quantity { get; private set; }

    /// <summary>
    /// Creates an inventory entry with an initial positive quantity.
    /// </summary>
    public InventoryItem(Product product, int quantity)
    {
        Product = product ?? throw new ArgumentNullException(nameof(product));

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Quantity must be greater than zero.");

        Quantity = quantity;
    }

    /// <summary>
    /// Increases the stored quantity by a positive amount.
    /// </summary>
    internal void AddQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        Quantity = checked(Quantity + quantity);
    }

    /// <summary>
    /// Decreases the stored quantity without allowing insufficient stock.
    /// </summary>
    internal void RemoveQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        if (quantity > Quantity)
            throw new InvalidOperationException(
                "Insufficient inventory.");

        Quantity -= quantity;
    }
}