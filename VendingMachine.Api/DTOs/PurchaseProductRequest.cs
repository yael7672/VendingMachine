using System.ComponentModel.DataAnnotations;

namespace VendingMachine.Api.DTOs;

public sealed class PurchaseProductRequest
{
    [Required]
    public string ProductCode { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}