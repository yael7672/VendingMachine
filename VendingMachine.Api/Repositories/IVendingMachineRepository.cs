using VendingMachineEntity = global::VendingMachine.Api.Domain.Entities.VendingMachine;

namespace VendingMachine.Api.Repositories;

public interface IVendingMachineRepository
{
    /// <summary>
    /// Stores a new vending machine.
    /// </summary>
    void Add(VendingMachineEntity machine);

    /// <summary>
    /// Finds a machine by its unique identifier.
    /// </summary>
    VendingMachineEntity? GetById(Guid id);

    /// <summary>
    /// Returns all stored machines.
    /// </summary>
    IReadOnlyCollection<VendingMachineEntity> GetAll();
}