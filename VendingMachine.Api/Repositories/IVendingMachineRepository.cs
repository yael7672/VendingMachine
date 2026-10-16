using VendingMachineEntity = global::VendingMachine.Api.Domain.Entities.VendingMachine;

namespace VendingMachine.Api.Repositories;

public interface IVendingMachineRepository
{
    void Add(VendingMachineEntity machine);

    VendingMachineEntity? GetById(Guid id);

    IReadOnlyCollection<VendingMachineEntity> GetAll();
}