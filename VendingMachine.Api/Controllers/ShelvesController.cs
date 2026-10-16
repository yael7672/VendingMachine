using Microsoft.AspNetCore.Mvc;
using VendingMachine.Api.DTOs;
using VendingMachine.Api.Services;

namespace VendingMachine.Api.Controllers;

[ApiController]
[Route("api/machines/{machineId:guid}/shelves")]
public class ShelvesController : ControllerBase
{
    private readonly VendingMachineService _service;

    public ShelvesController(VendingMachineService service)
    {
        _service = service;
    }

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

    [HttpGet("{shelfId:guid}")]
    public ActionResult<ShelfResponse> GetShelf(
        Guid machineId,
        Guid shelfId)
    {
        var shelf = _service.GetShelf(machineId, shelfId);

        return Ok(shelf.ToResponse());
    }

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