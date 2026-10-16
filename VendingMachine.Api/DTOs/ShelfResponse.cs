namespace VendingMachine.Api.DTOs;

public sealed record ShelfResponse(
    Guid Id,
    string ProductType,
    int Capacity,
    int CurrentQuantity,
    IReadOnlyList<InventoryItemResponse> Inventory
);