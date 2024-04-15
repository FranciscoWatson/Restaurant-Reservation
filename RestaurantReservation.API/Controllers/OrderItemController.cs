using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs.OrderItemDTOs;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.OrderItemRepository;

namespace RestaurantReservation.API.Controllers;

[ApiController]
[Route("api/orderitems")]
public class OrderItemController : ControllerBase
{
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IMapper _mapper;

    public OrderItemController(IOrderItemRepository orderItemRepository, IMapper mapper)
    {
        _orderItemRepository = orderItemRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderItems()
    {
        var orderItems = await _orderItemRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<OrderItemDto>>(orderItems));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderItemDto>> GetOrderItemsById(int id)
    {
        var orderItem = await _orderItemRepository.GetByIdAsync(id);
        if (orderItem == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<OrderItemDto>(orderItem));
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderItem([FromBody] OrderItemForCreationDto orderItemForCreationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var orderItem = _mapper.Map<OrderItem>(orderItemForCreationDto);

        await _orderItemRepository.AddAsync(orderItem);

        var orderItemDto = _mapper.Map<OrderItemDto>(orderItem);

        return Ok(orderItemDto);

    }

    [HttpPut("{id}")]
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

    [HttpPatch("{id}")]
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

    [HttpDelete("{id}")]
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