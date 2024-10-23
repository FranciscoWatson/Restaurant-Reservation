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

    /// <summary>
    /// Retrieves all restaurants.
    /// </summary>
    /// <returns>A list of all restaurants.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetOrders()
    {
        var restaurants = await _restaurantRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<RestaurantDto>>(restaurants));
    }

    /// <summary>
    /// Retrieves a specific restaurant by ID.
    /// </summary>
    /// <param name="id">The ID of the restaurant to retrieve.</param>
    /// <returns>Returns the restaurant data.</returns>
    /// <response code="200">Returned if the restaurant was found.</response>
    /// <response code="404">Returned if the restaurant is not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RestaurantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RestaurantDto>> GetRestaurantsById(int id)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(id);
        if (restaurant == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<RestaurantDto>(restaurant));
    }

    /// <summary>
    /// Creates a new restaurant.
    /// </summary>
    /// <param name="employeeForCreationDto">The restaurant data to create.</param>
    /// <returns>A newly created restaurant.</returns>
    /// <response code="200">Returned if the restaurant is successfully created.</response>
    /// <response code="400">Returned if the request data is invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(RestaurantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] 
    public async Task<IActionResult> CreateRestaurant([FromBody] RestaurantForCreationDto restaurantForCreationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var restaurant = _mapper.Map<Restaurant>(restaurantForCreationDto);

        await _restaurantRepository.AddAsync(restaurant);

        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        return Ok(restaurantDto);

    }

    /// <summary>
    /// Updates a restaurant.
    /// </summary>
    /// <param name="id">The ID of the restaurant to update.</param>
    /// <param name="restaurantForUpdateDto">The updated restaurant data.</param>
    /// <response code="204">Returned if the restaurant is successfully updated.</response>
    /// <response code="404">Returned if the restaurant is not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Partially updates a restaurant.
    /// </summary>
    /// <param name="id">The ID of the restaurant to update.</param>
    /// <param name="patchDocument">The patch document for update.</param>
    /// <response code="204">Returned if the restaurant is successfully updated.</response>
    /// <response code="400">Returned if the request data is invalid.</response>
    /// <response code="404">Returned if the restaurant is not found.</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Deletes a specific restaurant.
    /// </summary>
    /// <param name="id">The ID of the restaurant to delete.</param>
    /// <response code="204">Returned if the restaurant is successfully deleted.</response>
    /// <response code="404">Returned if the restaurant is not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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