using Microsoft.AspNetCore.Mvc;
using VendingMachine.Api.DTOs;
using VendingMachine.Api.Services;

namespace VendingMachine.Api.Controllers;

[ApiController]
[Route("api/machines")]
public class MachinesController : ControllerBase
{
    private readonly VendingMachineService _service;

    public MachinesController(VendingMachineService service)
    {
        _service = service;
    }

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

    [HttpGet]
    public ActionResult<IEnumerable<MachineResponse>> GetAll()
    {
        var machines = _service.GetAllMachines()
            .Select(m => m.ToResponse())
            .ToList();

        return Ok(machines);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<MachineResponse> GetMachine(Guid id)
    {
        var machine = _service.GetMachine(id);

        return Ok(machine.ToResponse());
    }
}