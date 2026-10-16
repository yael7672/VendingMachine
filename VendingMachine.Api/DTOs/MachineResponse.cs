namespace VendingMachine.Api.DTOs;

public sealed record MachineResponse(
    Guid Id,
    string Name,
    string? Location,
    int MaxShelves,
    IReadOnlyList<ShelfResponse> Shelves
);