using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs.TableDTOs;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.RestaurantRepository;
using RestaurantReservation.Db.Repositories.TableRepository;

namespace RestaurantReservation.API.Controllers;

[ApiController]
[Route("api/tables")]
public class TableController : ControllerBase
{
    private readonly ITableRepository _tableRepository;
    private readonly IMapper _mapper;

    public TableController(ITableRepository tableRepository, IMapper mapper)
    {
        _tableRepository = tableRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TableDto>>> GetTables()
    {
        var tables = await _tableRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<TableDto>>(tables));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TableDto>> GetTablesById(int id)
    {
        var table = await _tableRepository.GetByIdAsync(id);
        if (table == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<TableDto>(table));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTable([FromBody] TableForCreationDto tableForCreationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var table = _mapper.Map<Table>(tableForCreationDto);

        await _tableRepository.AddAsync(table);

        var tableDto = _mapper.Map<TableDto>(table);

        return Ok(tableDto);

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TableForUpdateDto tableForUpdateDto)
    {
        var table = await _tableRepository.GetByIdAsync(id);

        if (table == null)
        {
            return NotFound();
        }

        _mapper.Map(tableForUpdateDto, table);

        await _tableRepository.UpdateAsync(table);

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartiallyUpdateTable(int id,
        [FromBody] JsonPatchDocument<TableForUpdateDto> patchDocument)
    {
        var tableEntity = await _tableRepository.GetByIdAsync(id);

        if (tableEntity is null)
        {
            return NotFound();
        }

        var tableToPatch = _mapper.Map<TableForUpdateDto>(tableEntity);

        patchDocument.ApplyTo(tableToPatch, ModelState);

        if (!ModelState.IsValid || !TryValidateModel(tableToPatch))
        {
            return BadRequest(ModelState);
        }

        _mapper.Map(tableToPatch, tableEntity);

        await _tableRepository.UpdateAsync(tableEntity);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTable(int id)
    {
        var table = await _tableRepository.GetByIdAsync(id);

        if (table == null)
        {
            return NotFound();
        }

        await _tableRepository.DeleteAsync(id);

        return NoContent();
    }
}