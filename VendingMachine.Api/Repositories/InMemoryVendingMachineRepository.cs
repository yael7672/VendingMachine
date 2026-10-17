using System.Collections.Concurrent;
using VendingMachineEntity = global::VendingMachine.Api.Domain.Entities.VendingMachine;

namespace VendingMachine.Api.Repositories;

public class InMemoryVendingMachineRepository
    : IVendingMachineRepository
{
    private readonly ConcurrentDictionary<Guid, VendingMachineEntity>
        _machines = new();

    /// <summary>
    /// Stores a machine in the in-memory collection.
    /// </summary>
    public void Add(VendingMachineEntity machine)
    {
        ArgumentNullException.ThrowIfNull(machine);

        if (!_machines.TryAdd(machine.Id, machine))
        {
            throw new InvalidOperationException(
                $"Machine with ID {machine.Id} already exists.");
        }
    }

    /// <summary>
    /// Finds a stored machine by its identifier.
    /// </summary>
    public VendingMachineEntity? GetById(Guid id)
    {
        _machines.TryGetValue(id, out var machine);

        return machine;
    }

    /// <summary>
    /// Returns a snapshot of all stored machines.
    /// </summary>
    public IReadOnlyCollection<VendingMachineEntity> GetAll()
    {
        return _machines.Values.ToList().AsReadOnly();
    }
}