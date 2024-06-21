using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.BEND.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderItemController : ControllerBase
{
    private readonly IOrderItemService _orderItemService;

    public OrderItemController(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderItemDTO>> GetOrderItemById(Guid id)
    {
        var orderItem = await _orderItemService.GetByIdAsync(id);
        if (orderItem == null)
        {
            return NotFound();
        }
        return Ok(orderItem);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderItemDTO>>> GetAllOrderItems()
    {
        var orderItems = await _orderItemService.GetAllAsync();
        return Ok(orderItems);
    }

    [HttpPost]
    public async Task<ActionResult> AddOrderItem([FromBody] OrderItemDTO orderItem)
    {
        if (orderItem == null)
        {
            return BadRequest();
        }

        await _orderItemService.AddAsync(orderItem);
        return CreatedAtAction(nameof(GetOrderItemById), new { id = orderItem.OrderItemID }, orderItem);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOrderItem(Guid id, [FromBody] OrderItemDTO orderItem)
    {
        if (orderItem == null || orderItem.OrderItemID != id)
        {
            return BadRequest();
        }

        var existingOrderItem = await _orderItemService.GetByIdAsync(id);
        if (existingOrderItem == null)
        {
            return NotFound();
        }

        await _orderItemService.UpdateAsync(orderItem);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrderItem(Guid id)
    {
        var existingOrderItem = await _orderItemService.GetByIdAsync(id);
        if (existingOrderItem == null)
        {
            return NotFound();
        }

        await _orderItemService.DeleteAsync(id);
        return NoContent();
    }
}