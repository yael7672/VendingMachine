using System.Collections.Concurrent;
using VendingMachineEntity = global::VendingMachine.Api.Domain.Entities.VendingMachine;

namespace VendingMachine.Api.Repositories;

public class InMemoryVendingMachineRepository
    : IVendingMachineRepository
{
    private readonly ConcurrentDictionary<Guid, VendingMachineEntity>
        _machines = new();

    public void Add(VendingMachineEntity machine)
    {
        ArgumentNullException.ThrowIfNull(machine);

        if (!_machines.TryAdd(machine.Id, machine))
        {
            throw new InvalidOperationException(
                $"Machine with ID {machine.Id} already exists.");
        }
    }

    public VendingMachineEntity? GetById(Guid id)
    {
        _machines.TryGetValue(id, out var machine);

        return machine;
    }

    public IReadOnlyCollection<VendingMachineEntity> GetAll()
    {
        return _machines.Values.ToList().AsReadOnly();
    }
}