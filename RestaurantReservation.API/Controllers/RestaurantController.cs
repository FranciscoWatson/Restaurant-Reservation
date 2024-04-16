using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs.RestaurantDTOs;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.RestaurantRepository;

namespace RestaurantReservation.API.Controllers;

[ApiController]
[Route("api/restaurants")]
[Authorize]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IMapper _mapper;

    public RestaurantController(IRestaurantRepository restaurantRepository, IMapper mapper)
    {
        _restaurantRepository = restaurantRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetOrders()
    {
        var restaurants = await _restaurantRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<RestaurantDto>>(restaurants));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RestaurantDto>> GetRestaurantsById(int id)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(id);
        if (restaurant == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<RestaurantDto>(restaurant));
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant([FromBody] RestaurantForCreationDto restaurantForCreationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var restaurant = _mapper.Map<Restaurant>(restaurantForCreationDto);

        await _restaurantRepository.AddAsync(restaurant);

        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        return Ok(restaurantDto);

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RestaurantForUpdateDto restaurantForUpdateDto)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(id);

        if (restaurant == null)
        {
            return NotFound();
        }

        _mapper.Map(restaurantForUpdateDto, restaurant);

        await _restaurantRepository.UpdateAsync(restaurant);

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartiallyUpdateRestaurant(int id,
        [FromBody] JsonPatchDocument<RestaurantForUpdateDto> patchDocument)
    {
        var restaurantEntity = await _restaurantRepository.GetByIdAsync(id);

        if (restaurantEntity is null)
        {
            return NotFound();
        }

        var restaurantToPatch = _mapper.Map<RestaurantForUpdateDto>(restaurantEntity);

        patchDocument.ApplyTo(restaurantToPatch, ModelState);

        if (!ModelState.IsValid || !TryValidateModel(restaurantToPatch))
        {
            return BadRequest(ModelState);
        }

        _mapper.Map(restaurantToPatch, restaurantEntity);

        await _restaurantRepository.UpdateAsync(restaurantEntity);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRestaurant(int id)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(id);

        if (restaurant == null)
        {
            return NotFound();
        }

        await _restaurantRepository.DeleteAsync(id);

        return NoContent();
    }
}