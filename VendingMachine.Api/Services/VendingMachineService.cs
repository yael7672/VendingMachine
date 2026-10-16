using VendingMachine.Api.Domain.Entities;
using VendingMachine.Api.Repositories;

using VendingMachineEntity =
    global::VendingMachine.Api.Domain.Entities.VendingMachine;

namespace VendingMachine.Api.Services;

public class VendingMachineService
{
    private readonly IVendingMachineRepository _repository;

    public VendingMachineService(
        IVendingMachineRepository repository)
    {
        _repository = repository;
    }

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

    public IReadOnlyCollection<VendingMachineEntity> GetAllMachines()
    {
        return _repository.GetAll();
    }

    public VendingMachineEntity GetMachine(Guid machineId)
    {
        return _repository.GetById(machineId)
            ?? throw new KeyNotFoundException(
                $"Machine with ID {machineId} was not found.");
    }

    public Shelf AddShelf(
        Guid machineId,
        string productType,
        int capacity)
    {
        var machine = GetMachine(machineId);

        return machine.AddShelf(productType, capacity);
    }

    public Shelf GetShelf(Guid machineId, Guid shelfId)
    {
        var machine = GetMachine(machineId);

        return machine.Shelves.FirstOrDefault(
            shelf => shelf.Id == shelfId)
            ?? throw new KeyNotFoundException(
                $"Shelf with ID {shelfId} was not found.");
    }

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