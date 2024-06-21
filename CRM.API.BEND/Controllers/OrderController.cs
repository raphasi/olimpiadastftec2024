using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.BEND.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDTO>> GetOrderById(Guid id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrders()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders);
    }

    [HttpPost]
    public async Task<ActionResult> AddOrder([FromBody] OrderDTO order)
    {
        if (order == null)
        {
            return BadRequest();
        }

        await _orderService.AddAsync(order);
        return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderID }, order);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOrder(Guid id, [FromBody] OrderDTO order)
    {
        if (order == null || order.OrderID != id)
        {
            return BadRequest();
        }

        var existingOrder = await _orderService.GetByIdAsync(id);
        if (existingOrder == null)
        {
            return NotFound();
        }

        await _orderService.UpdateAsync(order);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrder(Guid id)
    {
        var existingOrder = await _orderService.GetByIdAsync(id);
        if (existingOrder == null)
        {
            return NotFound();
        }

        await _orderService.DeleteAsync(id);
        return NoContent();
    }
}