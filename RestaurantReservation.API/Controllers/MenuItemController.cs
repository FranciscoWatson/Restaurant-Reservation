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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenuItems()
    {
        var menuItems = await _menuItemRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<MenuItemDto>>(menuItems));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MenuItemDto>> GetMenuItemById(int id)
    {
        var menuItem = await _menuItemRepository.GetByIdAsync(id);
        if (menuItem == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<MenuItemDto>(menuItem));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateMenuItem([FromBody] MenuItemForCreationDto menuItemForCreationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var menuItem = _mapper.Map<MenuItem>(menuItemForCreationDto);

        await _menuItemRepository.AddAsync(menuItem);

        var menuItemDto = _mapper.Map<MenuItemDto>(menuItem);
        
        return Ok(menuItemDto);

    }
    
    [HttpPut("{id}")]
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

    [HttpPatch("{id}")]
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

    [HttpDelete("{id}")]
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