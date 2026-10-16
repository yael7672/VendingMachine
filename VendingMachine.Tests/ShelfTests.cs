using Xunit;
using Shelf = global::VendingMachine.Api.Domain.Entities.Shelf;
using Product = global::VendingMachine.Api.Domain.Entities.Product;
using BusinessRuleException = global::VendingMachine.Api.Domain.Exceptions.BusinessRuleException;

namespace VendingMachine.Tests;

public class ShelfTests
{
    [Fact]
    public void CreateShelf_ShouldStartEmpty()
    {
        var shelf = new Shelf("BottledDrink", 10);

        Assert.Equal(10, shelf.Capacity);
        Assert.Equal(0, shelf.CurrentQuantity);
        Assert.Empty(shelf.Inventory);
    }

    [Fact]
    public void LoadProducts_WithMatchingType_ShouldSucceed()
    {
        var shelf = new Shelf("BottledDrink", 10);
        var product = new Product("WATER", "Water", "BottledDrink");

        shelf.LoadProducts(product, 5);

        Assert.Equal(5, shelf.CurrentQuantity);
        Assert.Single(shelf.Inventory);
    }

    [Fact]
    public void LoadProducts_WithWrongType_ShouldFail()
    {
        var shelf = new Shelf("BottledDrink", 10);
        var product = new Product("CHIPS", "Chips", "Snack");

        Assert.Throws<BusinessRuleException>(() =>
            shelf.LoadProducts(product, 5));

        Assert.Equal(0, shelf.CurrentQuantity);
    }

    [Fact]
    public void LoadProducts_ExceedingCapacity_ShouldFail()
    {
        var shelf = new Shelf("BottledDrink", 10);
        var product = new Product("WATER", "Water", "BottledDrink");

        Assert.Throws<BusinessRuleException>(() =>
            shelf.LoadProducts(product, 18));

        Assert.Equal(0, shelf.CurrentQuantity);
    }

    [Fact]
    public void LoadProducts_ExceedingRemainingCapacity_ShouldFail()
    {
        var shelf = new Shelf("BottledDrink", 10);
        var product = new Product("WATER", "Water", "BottledDrink");

        shelf.LoadProducts(product, 5);

        Assert.Throws<BusinessRuleException>(() =>
            shelf.LoadProducts(product, 6));

        Assert.Equal(5, shelf.CurrentQuantity);
    }

    [Fact]
    public void LoadProducts_ExactlyAtCapacity_ShouldSucceed()
    {
        var shelf = new Shelf("BottledDrink", 10);
        var product = new Product("WATER", "Water", "BottledDrink");

        shelf.LoadProducts(product, 10);

        Assert.Equal(10, shelf.CurrentQuantity);
    }

    [Fact]
    public void LoadProducts_SameProduct_ShouldIncreaseQuantity()
    {
        var shelf = new Shelf("BottledDrink", 10);
        var product = new Product("WATER", "Water", "BottledDrink");

        shelf.LoadProducts(product, 3);
        shelf.LoadProducts(product, 4);

        Assert.Equal(7, shelf.CurrentQuantity);
        Assert.Single(shelf.Inventory);
        Assert.Equal(7, shelf.Inventory.First().Quantity);
    }

    [Fact]
    public void LoadProducts_DifferentProductsSameType_ShouldSucceed()
    {
        var shelf = new Shelf("BottledDrink", 10);

        var water = new Product("WATER", "Water", "BottledDrink");
        var cola = new Product("COLA", "Cola", "BottledDrink");

        shelf.LoadProducts(water, 4);
        shelf.LoadProducts(cola, 3);

        Assert.Equal(7, shelf.CurrentQuantity);
        Assert.Equal(2, shelf.Inventory.Count);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void LoadProducts_WithInvalidQuantity_ShouldFail(int quantity)
    {
        var shelf = new Shelf("BottledDrink", 10);
        var product = new Product("WATER", "Water", "BottledDrink");

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            shelf.LoadProducts(product, quantity));

        Assert.Equal(0, shelf.CurrentQuantity);
    }

    [Fact]
    public void PurchaseProduct_WithAvailableStock_ShouldDecreaseQuantity()
    {
        var shelf = new Shelf("BottledDrink", 10);
        var product = new Product("WATER", "Water", "BottledDrink");

        shelf.LoadProducts(product, 5);

        shelf.PurchaseProduct("WATER", 2);

        Assert.Equal(3, shelf.CurrentQuantity);
        Assert.Equal(3, shelf.Inventory.First().Quantity);
    }

    [Fact]
    public void PurchaseProduct_LastItem_ShouldRemoveInventoryItem()
    {
        var shelf = new Shelf("BottledDrink", 10);
        var product = new Product("WATER", "Water", "BottledDrink");

        shelf.LoadProducts(product, 1);

        shelf.PurchaseProduct("WATER", 1);

        Assert.Equal(0, shelf.CurrentQuantity);
        Assert.Empty(shelf.Inventory);
    }

    [Fact]
    public void PurchaseProduct_WhenStockIsEmpty_ShouldFail()
    {
        var shelf = new Shelf("BottledDrink", 10);

        Assert.Throws<BusinessRuleException>(() =>
            shelf.PurchaseProduct("WATER", 1));

        Assert.Equal(0, shelf.CurrentQuantity);
    }

    [Fact]
    public void PurchaseProduct_WhenQuantityExceedsStock_ShouldFail()
    {
        var shelf = new Shelf("BottledDrink", 10);
        var product = new Product("WATER", "Water", "BottledDrink");

        shelf.LoadProducts(product, 3);

        Assert.Throws<BusinessRuleException>(() =>
            shelf.PurchaseProduct("WATER", 5));

        Assert.Equal(3, shelf.CurrentQuantity);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void PurchaseProduct_WithInvalidQuantity_ShouldFail(int quantity)
    {
        var shelf = new Shelf("BottledDrink", 10);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            shelf.PurchaseProduct("WATER", quantity));
    }
    [Fact]
    public void LoadProducts_ConcurrentRequests_NeverExceedCapacity()
    {
        var shelf = new Shelf("BottledDrink", 10);
        var water = new Product("WATER", "Water", "BottledDrink");
        var successful = 0;

        Parallel.For(0, 100, _ =>
        {
            try
            {
                shelf.LoadProducts(water, 1);
                Interlocked.Increment(ref successful);
            }
            catch (BusinessRuleException)
            {
                // Attempts beyond capacity should be rejected.
            }
        });

        Assert.Equal(10, successful);
        Assert.Equal(10, shelf.CurrentQuantity);
    }
}