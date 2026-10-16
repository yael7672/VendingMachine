using System.ComponentModel.DataAnnotations;

namespace VendingMachine.Api.DTOs;

public sealed class LoadInventoryRequest
{
    [Required]
    public string ProductCode { get; set; } = string.Empty;

    [Required]
    public string ProductName { get; set; } = string.Empty;

    [Required]
    public string ProductType { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}