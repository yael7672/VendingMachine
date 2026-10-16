using VendingMachine.Api.Domain.Entities;
using VendingMachineEntity =
    global::VendingMachine.Api.Domain.Entities.VendingMachine;

namespace VendingMachine.Api.DTOs;

public static class ResponseMapper
{
    public static MachineResponse ToResponse(this VendingMachineEntity machine)
    {
        return new MachineResponse(
            machine.Id,
            machine.Name,
            machine.Location,
            machine.MaxShelves,
            machine.Shelves.Select(s => s.ToResponse()).ToList());
    }

    public static ShelfResponse ToResponse(this Shelf shelf)
    {
        var inventory = shelf.Inventory
            .Select(item => new InventoryItemResponse(
                item.Product.Code,
                item.Product.Name,
                item.Product.ProductType,
                item.Quantity))
            .ToList();

        return new ShelfResponse(
            shelf.Id,
            shelf.ProductType,
            shelf.Capacity,
            inventory.Sum(item => item.Quantity),
            inventory);
    }
}
