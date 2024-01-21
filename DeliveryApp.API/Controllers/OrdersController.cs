using DeliveryApp.API.Dtos;
using DeliveryApp.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationContext _appContext;
        
        public OrdersController(ApplicationContext appContext) => _appContext = appContext;
        
        [Authorize(Roles = "User")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                var currentUserId = ControllerContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")
                    ?.Value;
                if (currentUserId is null)
                {
                    return BadRequest("Вы ввели неправильный токен.");
                }

                var currentUserGuidId = Guid.Parse(currentUserId);
                var currentUser = _appContext.Users.FirstOrDefault(x => x.Id == currentUserGuidId);
                var items = new List<Item>();

                foreach (var item in createOrderDto.Items)
                {
                    var product = await _appContext.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                    if (product == null)
                    {
                        return BadRequest("Такого продукта еще нет в нашей базе.");
                    }
                    items.Add(new Item { Product = product, Count = item.Count, TotalSum = product.Cost * item.Count });
                }

                currentUser?.Orders.Add(new Order
                {
                    Status = OrderStatus.Free, DeliveryDate = createOrderDto.DeliveryDate, Items = items,
                    Requests = createOrderDto.Requests
                });
                await _appContext.SaveChangesAsync();
                return Ok("Заказ был успешно создан! Ожидайте...");
            }
            catch (DbUpdateConcurrencyException e)
            {
                return BadRequest("Произошла неизвестная ошибка.");
            }
        }
        
        [Authorize(Roles = "User, Admin")]
        [HttpPost("cancel")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            try
            {
                var currentUserId = ControllerContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")
                    ?.Value;
                if (currentUserId is null)
                {
                    return BadRequest("Вы ввели неправильный токен.");
                }
            
                var currentUserGuidId = Guid.Parse(currentUserId);
                var currentUser = _appContext.Users.FirstOrDefault(x => x.Id == currentUserGuidId);
                var orderToCancel = await _appContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
                if (orderToCancel == null)
                {
                    return NotFound("Этого заказа не существует!");
                }
                if (orderToCancel.Status != OrderStatus.Accepted)
                {
                    return BadRequest("Этот заказ не выполняется!");
                }
                if (currentUser.Role == Role.Admin)
                {
                    orderToCancel.Status = OrderStatus.Canceled;
                    await _appContext.SaveChangesAsync();
                    return Ok("Заказ успешно отменен!");
                }
                if (!currentUser.Orders.Contains(orderToCancel))
                {
                    return BadRequest("Вы пытаетесь отменить не свой заказ!");
                }
                orderToCancel.Status = OrderStatus.Canceled;
                await _appContext.SaveChangesAsync();
                return Ok("Заказ успешно отменен!");
            }
            catch (DbUpdateConcurrencyException e)
            {
                return BadRequest("Произошла неизвестная ошибка.");
            }
           
        }
        
        [Authorize(Roles = "Courier")]
        [HttpPost("accept")]
        public async Task<IActionResult> AcceptOrder(Guid orderId)
        {
            try
            {
                var currentUserId = ControllerContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")
                    ?.Value;
                if (currentUserId is null)
                {
                    return BadRequest("Вы ввели неправильный токен.");
                }
            
                var currentUserGuidId = Guid.Parse(currentUserId);
                var currentUser = _appContext.Users.FirstOrDefault(x => x.Id == currentUserGuidId);
                var order = await _appContext.Orders.FindAsync(orderId);
            
                if (order == null)
                {
                    return NotFound("Такого заказа не существует!");
                }
                if (order.Status == OrderStatus.Free)
                {
                    order.Status = OrderStatus.Accepted;
                    currentUser.Orders.Add(order);
                    await _appContext.SaveChangesAsync();
                    return Ok("Вы успешно приняли заказ. Удачи!");
                }
                return Conflict("Этот заказ уже недоступен.");
            }
            catch (DbUpdateConcurrencyException e)
            {
                return BadRequest("Произошла неизвестная ошибка.");
            }
        }
        
        [Authorize(Roles = "User")]
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmOrder(Guid orderId)
        {
            try
            {
                var currentUserId = ControllerContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")
                    ?.Value;
                if (currentUserId is null)
                {
                    return BadRequest("Вы ввели неправильный токен.");
                }
                var currentUserGuidId = Guid.Parse(currentUserId);
                var currentUser = _appContext.Users.FirstOrDefault(x => x.Id == currentUserGuidId);
                var order = await _appContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
                if (order == null)
                {
                    return NotFound("Такого заказа не существует!");
                }
                if (currentUser.Orders.Contains(order) && order.Status == OrderStatus.Accepted)
                {
                    order.Status = OrderStatus.Done;
                    await _appContext.SaveChangesAsync();
                    return Ok("Заказ успешно подтвержден!");
                }
                return BadRequest("Этот заказ не ваш или еще не в работе.");
            }
            catch (DbUpdateConcurrencyException e)
            {
                return BadRequest("Произошла неизвестная ошибка.");
            }
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allOrders = await _appContext.Orders.Include(x => x.Users).Include(x => x.Items).AsSplitQuery().ToListAsync();
            var allOrdersDtos = allOrders.Select(x => new OrderResponseDto
            {
                DeliveryDate = x.DeliveryDate,
                Id = x.Id,
                Items = x.Items,
                Requests = x.Requests,
                Status = x.Status,
                UserIds = x.Users.Select(x => x.Id).ToList()
            });
            return Ok(allOrdersDtos);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll(Guid orderId)
        {
            var order = await _appContext.Orders.Include(x => x.Users).Include(x => x.Items).AsSplitQuery().FirstOrDefaultAsync(x => x.Id == orderId);
            var orderDto = new OrderResponseDto
            {
                DeliveryDate = order.DeliveryDate,
                Id = order.Id,
                Items = order.Items,
                Requests = order.Requests,
                Status = order.Status,
                UserIds = order.Users.Select(x => x.Id).ToList()
            };
            return Ok(orderDto);
        }
    }
}
