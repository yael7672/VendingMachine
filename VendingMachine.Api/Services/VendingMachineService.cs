using VendingMachine.Api.Domain.Entities;
using VendingMachine.Api.Repositories;

using VendingMachineEntity =
    global::VendingMachine.Api.Domain.Entities.VendingMachine;

namespace VendingMachine.Api.Services;

public class VendingMachineService
{
    private readonly IVendingMachineRepository _repository;

    /// <summary>
    /// Initializes the service with its repository dependency.
    /// </summary>
    public VendingMachineService(
        IVendingMachineRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Creates and stores a new vending machine.
    /// </summary>
    public VendingMachineEntity CreateMachine(
        string name,
        string? location,
        int maxShelves)
    {
        var machine = new VendingMachineEntity(
            name,
            location,
            maxShelves);

        _repository.Add(machine);

        return machine;
    }

    /// <summary>
    /// Returns all stored vending machines.
    /// </summary>
    public IReadOnlyCollection<VendingMachineEntity> GetAllMachines()
    {
        return _repository.GetAll();
    }

    /// <summary>
    /// Retrieves a machine or reports that it does not exist.
    /// </summary>
    public VendingMachineEntity GetMachine(Guid machineId)
    {
        return _repository.GetById(machineId)
            ?? throw new KeyNotFoundException(
                $"Machine with ID {machineId} was not found.");
    }

    /// <summary>
    /// Finds a machine and delegates shelf creation to its domain logic.
    /// </summary>
    public Shelf AddShelf(
        Guid machineId,
        string productType,
        int capacity)
    {
        var machine = GetMachine(machineId);

        return machine.AddShelf(productType, capacity);
    }

    /// <summary>
    /// Retrieves a shelf belonging to the specified machine.
    /// </summary>
    public Shelf GetShelf(Guid machineId, Guid shelfId)
    {
        var machine = GetMachine(machineId);

        return machine.Shelves.FirstOrDefault(
            shelf => shelf.Id == shelfId)
            ?? throw new KeyNotFoundException(
                $"Shelf with ID {shelfId} was not found.");
    }

    /// <summary>
    /// Finds a shelf and loads a product through the domain rules.
    /// </summary>
    public Shelf LoadInventory(
        Guid machineId,
        Guid shelfId,
        string productCode,
        string productName,
        string productType,
        int quantity)
    {
        var shelf = GetShelf(machineId, shelfId);

        var product = new Product(
            productCode,
            productName,
            productType);

        shelf.LoadProducts(product, quantity);

        return shelf;
    }

    /// <summary>
    /// Finds a shelf and purchases the requested quantity.
    /// </summary>
    public Shelf PurchaseProduct(
        Guid machineId,
        Guid shelfId,
        string productCode,
        int quantity)
    {
        var shelf = GetShelf(machineId, shelfId);

        shelf.PurchaseProduct(productCode, quantity);

        return shelf;
    }
}