using System.ComponentModel.DataAnnotations;

namespace VendingMachine.Api.DTOs;

public sealed class CreateMachineRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Location { get; set; }

    [Range(1, int.MaxValue)]
    public int MaxShelves { get; set; }
}