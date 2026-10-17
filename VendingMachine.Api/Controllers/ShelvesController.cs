using Microsoft.AspNetCore.Mvc;
using VendingMachine.Api.DTOs;
using VendingMachine.Api.Services;

namespace VendingMachine.Api.Controllers;

[ApiController]
[Route("api/machines/{machineId:guid}/shelves")]
public class ShelvesController : ControllerBase
{
    private readonly VendingMachineService _service;

    /// <summary>
    /// Initializes the controller with the application service.
    /// </summary>
    public ShelvesController(VendingMachineService service)
    {
        _service = service;
    }

    /// <summary>
    /// Adds a shelf to a machine and returns HTTP 201.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(
        typeof(ShelfResponse),
        StatusCodes.Status201Created)]
    public ActionResult<ShelfResponse> AddShelf(
        Guid machineId,
        [FromBody] AddShelfRequest request)
    {
        var shelf = _service.AddShelf(
            machineId,
            request.ProductType,
            request.Capacity);

        return CreatedAtAction(
            nameof(GetShelf),
            new { machineId, shelfId = shelf.Id },
            shelf.ToResponse());
    }

    /// <summary>
    /// Returns the requested shelf and its current inventory.
    /// </summary>
    [HttpGet("{shelfId:guid}")]
    public ActionResult<ShelfResponse> GetShelf(
        Guid machineId,
        Guid shelfId)
    {
        var shelf = _service.GetShelf(machineId, shelfId);

        return Ok(shelf.ToResponse());
    }

    /// <summary>
    /// Loads product units into a shelf and returns the updated shelf.
    /// </summary>
    [HttpPost("{shelfId:guid}/inventory")]
    public ActionResult<ShelfResponse> LoadInventory(
        Guid machineId,
        Guid shelfId,
        [FromBody] LoadInventoryRequest request)
    {
        var shelf = _service.LoadInventory(
            machineId,
            shelfId,
            request.ProductCode,
            request.ProductName,
            request.ProductType,
            request.Quantity);

        return Ok(shelf.ToResponse());
    }

    /// <summary>
    /// Purchases product units and returns the updated shelf.
    /// </summary>
    [HttpPost("{shelfId:guid}/purchase")]
    public ActionResult<ShelfResponse> PurchaseProduct(
        Guid machineId,
        Guid shelfId,
        [FromBody] PurchaseProductRequest request)
    {
        var shelf = _service.PurchaseProduct(
            machineId,
            shelfId,
            request.ProductCode,
            request.Quantity);

        return Ok(shelf.ToResponse());
    }
}