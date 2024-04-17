using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs.OrderDTOs;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.OrderRepository;

namespace RestaurantReservation.API.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderController(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all orders.
    /// </summary>
    /// <returns>A list of all orders.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var orders = await _orderRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
    }

    /// <summary>
    /// Retrieves a specific order by ID.
    /// </summary>
    /// <param name="id">The ID of the order to retrieve.</param>
    /// <returns>Returns the order data.</returns>
    /// <response code="200">Returned if the order was found.</response>
    /// <response code="404">Returned if the order is not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> GetOrdersById(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<OrderDto>(order));
    }

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="orderForCreationDto">The order data to create.</param>
    /// <returns>A newly created order.</returns>
    /// <response code="200">Returned if the order is successfully created.</response>
    /// <response code="400">Returned if the request data is invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] 
    public async Task<IActionResult> CreateOrder([FromBody] OrderForCreationDto orderForCreationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var order = _mapper.Map<Order>(orderForCreationDto);

        await _orderRepository.AddAsync(order);

        var orderDto = _mapper.Map<OrderDto>(order);

        return Ok(orderDto);

    }

    /// <summary>
    /// Updates an order.
    /// </summary>
    /// <param name="id">The ID of the order to update.</param>
    /// <param name="orderForUpdateDto">The updated order data.</param>
    /// <response code="204">Returned if the order is successfully updated.</response>
    /// <response code="404">Returned if the order is not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] OrderForUpdateDto orderForUpdateDto)
    {
        var order = await _orderRepository.GetByIdAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        _mapper.Map(orderForUpdateDto, order);

        await _orderRepository.UpdateAsync(order);

        return NoContent();
    }

    /// <summary>
    /// Partially updates an order.
    /// </summary>
    /// <param name="id">The ID of the order to update.</param>
    /// <param name="patchDocument">The patch document for update.</param>
    /// <response code="204">Returned if the order is successfully updated.</response>
    /// <response code="400">Returned if the request data is invalid.</response>
    /// <response code="404">Returned if the order is not found.</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PartiallyUpdateOrder(int id,
        [FromBody] JsonPatchDocument<OrderForUpdateDto> patchDocument)
    {
        var orderEntity = await _orderRepository.GetByIdAsync(id);

        if (orderEntity is null)
        {
            return NotFound();
        }

        var orderToPatch = _mapper.Map<OrderForUpdateDto>(orderEntity);

        patchDocument.ApplyTo(orderToPatch, ModelState);

        if (!ModelState.IsValid || !TryValidateModel(orderToPatch))
        {
            return BadRequest(ModelState);
        }

        _mapper.Map(orderToPatch, orderEntity);

        await _orderRepository.UpdateAsync(orderEntity);

        return NoContent();
    }

    /// <summary>
    /// Deletes a specific order.
    /// </summary>
    /// <param name="id">The ID of the order to delete.</param>
    /// <response code="204">Returned if the order is successfully deleted.</response>
    /// <response code="404">Returned if the order is not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        await _orderRepository.DeleteAsync(id);

        return NoContent();
    }
}