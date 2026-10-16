using System.ComponentModel.DataAnnotations;

namespace VendingMachine.Api.DTOs;

public sealed class AddShelfRequest
{
    [Required]
    public string ProductType { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Capacity { get; set; }
}