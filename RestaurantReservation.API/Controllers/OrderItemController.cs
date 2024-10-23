using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs.OrderItemDTOs;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.OrderItemRepository;

namespace RestaurantReservation.API.Controllers;

[ApiController]
[Route("api/orderitems")]
[Authorize]
public class OrderItemController : ControllerBase
{
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IMapper _mapper;

    public OrderItemController(IOrderItemRepository orderItemRepository, IMapper mapper)
    {
        _orderItemRepository = orderItemRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all orderItems.
    /// </summary>
    /// <returns>A list of all orderItems.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderItems()
    {
        var orderItems = await _orderItemRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<OrderItemDto>>(orderItems));
    }

    /// <summary>
    /// Retrieves a specific orderItem by ID.
    /// </summary>
    /// <param name="id">The ID of the orderItem to retrieve.</param>
    /// <returns>Returns the orderItem data.</returns>
    /// <response code="200">Returned if the orderItem was found.</response>
    /// <response code="404">Returned if the orderItem is not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderItemDto>> GetOrderItemsById(int id)
    {
        var orderItem = await _orderItemRepository.GetByIdAsync(id);
        if (orderItem == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<OrderItemDto>(orderItem));
    }

    /// <summary>
    /// Creates a new orderItem.
    /// </summary>
    /// <param name="orderItemForCreationDto">The orderItem data to create.</param>
    /// <returns>A newly created orderItem.</returns>
    /// <response code="200">Returned if the orderItem is successfully created.</response>
    /// <response code="400">Returned if the request data is invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(OrderItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] 
    public async Task<IActionResult> CreateOrderItem([FromBody] OrderItemForCreationDto orderItemForCreationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var orderItem = _mapper.Map<OrderItem>(orderItemForCreationDto);

        await _orderItemRepository.AddAsync(orderItem);

        var orderItemDto = _mapper.Map<OrderItemDto>(orderItem);

        return Ok(orderItemDto);

    }

    /// <summary>
    /// Updates an orderItem.
    /// </summary>
    /// <param name="id">The ID of the orderItem to update.</param>
    /// <param name="orderItemForUpdateDto">The updated orderItem data.</param>
    /// <response code="204">Returned if the orderItem is successfully updated.</response>
    /// <response code="404">Returned if the orderItem is not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] OrderItemForUpdateDto orderItemForUpdateDto)
    {
        var orderItem = await _orderItemRepository.GetByIdAsync(id);

        if (orderItem == null)
        {
            return NotFound();
        }
        
        _mapper.Map(orderItemForUpdateDto, orderItem);
        
        await _orderItemRepository.UpdateAsync(orderItem);

        return NoContent();
    }

    /// <summary>
    /// Partially updates an orderItem.
    /// </summary>
    /// <param name="id">The ID of the orderItem to update.</param>
    /// <param name="patchDocument">The patch document for update.</param>
    /// <response code="204">Returned if the orderItem is successfully updated.</response>
    /// <response code="400">Returned if the request data is invalid.</response>
    /// <response code="404">Returned if the orderItem is not found.</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PartiallyUpdateOrderItem(int id,
        [FromBody] JsonPatchDocument<OrderItemForUpdateDto> patchDocument)
    {
        var orderItemEntity = await _orderItemRepository.GetByIdAsync(id);

        if (orderItemEntity is null)
        {
            return NotFound();
        }

        var orderItemToPatch = _mapper.Map<OrderItemForUpdateDto>(orderItemEntity);

        patchDocument.ApplyTo(orderItemToPatch, ModelState);

        if (!ModelState.IsValid || !TryValidateModel(orderItemToPatch))
        {
            return BadRequest(ModelState);
        }

        _mapper.Map(orderItemToPatch, orderItemEntity);

        await _orderItemRepository.UpdateAsync(orderItemEntity);

        return NoContent();
    }

    /// <summary>
    /// Deletes a specific orderItem.
    /// </summary>
    /// <param name="id">The ID of the orderItem to delete.</param>
    /// <response code="204">Returned if the orderItem is successfully deleted.</response>
    /// <response code="404">Returned if the orderItem is not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOrderItem(int id)
    {
        var orderItem = await _orderItemRepository.GetByIdAsync(id);

        if (orderItem == null)
        {
            return NotFound();
        }

        await _orderItemRepository.DeleteAsync(id);

        return NoContent();
    }
}