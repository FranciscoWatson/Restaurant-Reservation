using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs;
using RestaurantReservation.API.DTOs.MenuItemDTOs;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.MenuItemRepository;

namespace RestaurantReservation.API.Controllers;

[ApiController]
[Route("api/menuitems")]
[Authorize]
public class MenuItemController : ControllerBase
{
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly IMapper _mapper;
    
    public MenuItemController(IMenuItemRepository menuItemRepository, IMapper mapper)
    {
        _menuItemRepository = menuItemRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all menuItems.
    /// </summary>
    /// <returns>A list of all menuItems.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenuItems()
    {
        var menuItems = await _menuItemRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<MenuItemDto>>(menuItems));
    }

    /// <summary>
    /// Retrieves a specific menuItem by ID.
    /// </summary>
    /// <param name="id">The ID of the menuItem to retrieve.</param>
    /// <returns>Returns the menuItem data.</returns>
    /// <response code="200">Returned if the menuItem was found.</response>
    /// <response code="404">Returned if the menuItem is not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuItemDto>> GetMenuItemById(int id)
    {
        var menuItem = await _menuItemRepository.GetByIdAsync(id);
        if (menuItem == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<MenuItemDto>(menuItem));
    }
    
    /// <summary>
    /// Creates a new menuItem.
    /// </summary>
    /// <param name="menuItemForCreationDto">The menuItem data to create.</param>
    /// <returns>A newly created menuItem.</returns>
    /// <response code="200">Returned if the menuItem is successfully created.</response>
    /// <response code="400">Returned if the request data is invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] 
    public async Task<IActionResult> CreateMenuItem([FromBody] MenuItemForCreationDto menuItemForCreationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var menuItem = _mapper.Map<MenuItem>(menuItemForCreationDto);

        await _menuItemRepository.AddAsync(menuItem);

        var menuItemDto = _mapper.Map<MenuItemDto>(menuItem);
        
        return Ok(menuItemDto);

    }
    
    /// <summary>
    /// Updates a menuItem.
    /// </summary>
    /// <param name="id">The ID of the menuItem to update.</param>
    /// <param name="menuItemForUpdateDto">The updated menuItem data.</param>
    /// <response code="204">Returned if the menuItem is successfully updated.</response>
    /// <response code="404">Returned if the menuItem is not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] MenuItemForUpdateDto menuItemForUpdateDto)
    {
        var menuItem = await _menuItemRepository.GetByIdAsync(id);
        
        if (menuItem == null)
        {
            return NotFound();
        }
        
        _mapper.Map(menuItemForUpdateDto, menuItem);
        
        await _menuItemRepository.UpdateAsync(menuItem);
        
        return NoContent();
    }

    /// <summary>
    /// Partially updates a menuItem.
    /// </summary>
    /// <param name="id">The ID of the menuItem to update.</param>
    /// <param name="patchDocument">The patch document for update.</param>
    /// <response code="204">Returned if the menuItem is successfully updated.</response>
    /// <response code="400">Returned if the request data is invalid.</response>
    /// <response code="404">Returned if the menuItem is not found.</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PartiallyUpdateMenuItem(int id, [FromBody] JsonPatchDocument<MenuItemForUpdateDto> patchDocument)
    {
        var menuItemEntity = await _menuItemRepository.GetByIdAsync(id);

        if (menuItemEntity is null)
        {
            return NotFound();
        }

        var menuItemToPatch = _mapper.Map<MenuItemForUpdateDto>(menuItemEntity);

        patchDocument.ApplyTo(menuItemToPatch, ModelState);

        if (!ModelState.IsValid || !TryValidateModel(menuItemToPatch))
        {
            return BadRequest(ModelState);
        }

        _mapper.Map(menuItemToPatch, menuItemEntity);

        await _menuItemRepository.UpdateAsync(menuItemEntity);

        return NoContent();
    }

    /// <summary>
    /// Deletes a specific menuItem.
    /// </summary>
    /// <param name="id">The ID of the menuItem to delete.</param>
    /// <response code="204">Returned if the menuItem is successfully deleted.</response>
    /// <response code="404">Returned if the menuItem is not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMenuItem(int id)
    {
        var menuItem = await _menuItemRepository.GetByIdAsync(id);
            
        if (menuItem == null)
        {
            return NotFound();
        }
        
        await _menuItemRepository.DeleteAsync(id);

        return NoContent();
    }
    
}