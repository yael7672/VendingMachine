using Xunit;
using Machine = global::VendingMachine.Api.Domain.Entities.VendingMachine;
using BusinessRuleException = global::VendingMachine.Api.Domain.Exceptions.BusinessRuleException;

namespace VendingMachine.Tests;

public class VendingMachineTests
{
    [Fact]
    public void CreateMachine_ShouldStartWithNoShelves()
    {
        var machine = new Machine("Machine A", "Tel Aviv", 5);

        Assert.NotEqual(Guid.Empty, machine.Id);
        Assert.Equal("Machine A", machine.Name);
        Assert.Equal("Tel Aviv", machine.Location);
        Assert.Equal(5, machine.MaxShelves);
        Assert.Empty(machine.Shelves);
    }

    [Fact]
    public void CreateMachine_WithoutLocation_ShouldSucceed()
    {
        var machine = new Machine("Machine A", null, 5);

        Assert.Null(machine.Location);
    }

    [Theory]
    [InlineData("", 5)]
    [InlineData("   ", 5)]
    [InlineData("Machine A", 0)]
    [InlineData("Machine A", -1)]
    public void CreateMachine_WithInvalidData_ShouldFail(
        string name, int maxShelves)
    {
        Assert.ThrowsAny<ArgumentException>(() =>
            new Machine(name, null, maxShelves));
    }

    [Fact]
    public void AddShelf_WithinLimit_ShouldSucceed()
    {
        var machine = new Machine("Machine A", null, 5);

        var shelf1 = machine.AddShelf("BottledDrink", 10);
        var shelf2 = machine.AddShelf("Snack", 20);
        var shelf3 = machine.AddShelf("CannedDrink", 15);

        Assert.Equal(3, machine.Shelves.Count);
        Assert.Contains(shelf1, machine.Shelves);
        Assert.Contains(shelf2, machine.Shelves);
        Assert.Contains(shelf3, machine.Shelves);
    }

    [Fact]
    public void AddShelf_WhenMaximumReached_ShouldFail()
    {
        var machine = new Machine("Machine A", null, 5);

        for (int i = 0; i < 5; i++)
        {
            machine.AddShelf("Snack", 10);
        }

        Assert.Throws<BusinessRuleException>(() =>
            machine.AddShelf("BottledDrink", 10));

        Assert.Equal(5, machine.Shelves.Count);
    }

    [Theory]
    [InlineData("", 10)]
    [InlineData("Snack", 0)]
    [InlineData("Snack", -5)]
    public void AddShelf_WithInvalidData_ShouldFail(
        string productType, int capacity)
    {
        var machine = new Machine("Machine A", null, 5);

        Assert.ThrowsAny<ArgumentException>(() =>
            machine.AddShelf(productType, capacity));

        Assert.Empty(machine.Shelves);
    }
    [Fact]
    public void AddShelf_ConcurrentRequests_NeverExceedMaximum()
    {
        var machine = new Machine("Machine A", null, 5);
        var successful = 0;

        Parallel.For(0, 100, _ =>
        {
            try
            {
                machine.AddShelf("Snack", 10);
                Interlocked.Increment(ref successful);
            }
            catch (BusinessRuleException)
            {
                // Attempts beyond the maximum should be rejected.
            }
        });

        Assert.Equal(5, successful);
        Assert.Equal(5, machine.Shelves.Count);
    }
}