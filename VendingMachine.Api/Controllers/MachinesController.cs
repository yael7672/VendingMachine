using Microsoft.AspNetCore.Mvc;
using VendingMachine.Api.DTOs;
using VendingMachine.Api.Services;

namespace VendingMachine.Api.Controllers;

[ApiController]
[Route("api/machines")]
public class MachinesController : ControllerBase
{
    private readonly VendingMachineService _service;

    /// <summary>
    /// Initializes the controller with the application service.
    /// </summary>
    public MachinesController(VendingMachineService service)
    {
        _service = service;
    }

    /// <summary>
    /// Creates a machine and returns HTTP 201 with its details.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(
        typeof(MachineResponse),
        StatusCodes.Status201Created)]
    public ActionResult<MachineResponse> CreateMachine(
        [FromBody] CreateMachineRequest request)
    {
        var machine = _service.CreateMachine(
            request.Name,
            request.Location,
            request.MaxShelves);

        return CreatedAtAction(
            nameof(GetMachine),
            new { id = machine.Id },
            machine.ToResponse());
    }

    /// <summary>
    /// Returns all machines with HTTP 200.
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<MachineResponse>> GetAll()
    {
        var machines = _service.GetAllMachines()
            .Select(m => m.ToResponse())
            .ToList();

        return Ok(machines);
    }

    /// <summary>
    /// Returns a machine by ID with HTTP 200.
    /// </summary>
    [HttpGet("{id:guid}")]
    public ActionResult<MachineResponse> GetMachine(Guid id)
    {
        var machine = _service.GetMachine(id);

        return Ok(machine.ToResponse());
    }
}