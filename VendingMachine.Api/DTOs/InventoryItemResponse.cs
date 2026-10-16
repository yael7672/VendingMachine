namespace VendingMachine.Api.DTOs;

public sealed record InventoryItemResponse(
    string ProductCode,
    string ProductName,
    string ProductType,
    int Quantity
);