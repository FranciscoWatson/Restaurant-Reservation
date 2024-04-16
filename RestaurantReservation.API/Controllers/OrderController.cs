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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var orders = await _orderRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrdersById(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<OrderDto>(order));
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderForCreationDto orderForCreationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var order = _mapper.Map<Order>(orderForCreationDto);

        await _orderRepository.AddAsync(order);

        var orderDto = _mapper.Map<OrderDto>(order);

        return Ok(orderDto);

    }

    [HttpPut("{id}")]
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

    [HttpPatch("{id}")]
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

    [HttpDelete("{id}")]
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