namespace VendingMachine.Api.Domain.Entities;

public class InventoryItem
{
    public Product Product { get; }

    public int Quantity { get; private set; }

    public InventoryItem(Product product, int quantity)
    {
        Product = product ?? throw new ArgumentNullException(nameof(product));

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Quantity must be greater than zero.");

        Quantity = quantity;
    }

    internal void AddQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        Quantity = checked(Quantity + quantity);
    }

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