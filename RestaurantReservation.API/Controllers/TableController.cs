using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs.TableDTOs;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.RestaurantRepository;
using RestaurantReservation.Db.Repositories.TableRepository;

namespace RestaurantReservation.API.Controllers;

[ApiController]
[Route("api/tables")]
[Authorize]
public class TableController : ControllerBase
{
    private readonly ITableRepository _tableRepository;
    private readonly IMapper _mapper;

    public TableController(ITableRepository tableRepository, IMapper mapper)
    {
        _tableRepository = tableRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all tables.
    /// </summary>
    /// <returns>A list of all tables.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TableDto>>> GetTables()
    {
        var tables = await _tableRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<TableDto>>(tables));
    }

    /// <summary>
    /// Retrieves a specific table by ID.
    /// </summary>
    /// <param name="id">The ID of the table to retrieve.</param>
    /// <returns>Returns the table data.</returns>
    /// <response code="200">Returned if the table was found.</response>
    /// <response code="404">Returned if the table is not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TableDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TableDto>> GetTablesById(int id)
    {
        var table = await _tableRepository.GetByIdAsync(id);
        if (table == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<TableDto>(table));
    }

    /// <summary>
    /// Creates a new table.
    /// </summary>
    /// <param name="tableForCreationDto">The table data to create.</param>
    /// <returns>A newly created table.</returns>
    /// <response code="200">Returned if the table is successfully created.</response>
    /// <response code="400">Returned if the request data is invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(TableDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] 
    public async Task<IActionResult> CreateTable([FromBody] TableForCreationDto tableForCreationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var table = _mapper.Map<Table>(tableForCreationDto);

        await _tableRepository.AddAsync(table);

        var tableDto = _mapper.Map<TableDto>(table);

        return Ok(tableDto);

    }

    /// <summary>
    /// Updates an table.
    /// </summary>
    /// <param name="id">The ID of the table to update.</param>
    /// <param name="tableForUpdateDto">The updated table data.</param>
    /// <response code="204">Returned if the table is successfully updated.</response>
    /// <response code="404">Returned if the table is not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Partially updates a table.
    /// </summary>
    /// <param name="id">The ID of the table to update.</param>
    /// <param name="patchDocument">The patch document for update.</param>
    /// <response code="204">Returned if the table is successfully updated.</response>
    /// <response code="400">Returned if the request data is invalid.</response>
    /// <response code="404">Returned if the table is not found.</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Deletes a specific table.
    /// </summary>
    /// <param name="id">The ID of the table to delete.</param>
    /// <response code="204">Returned if the table is successfully deleted.</response>
    /// <response code="404">Returned if the table is not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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