using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRACTICA2.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Getorders()
        {
          if (_context.orders == null)
          {
              return NotFound();
          }
            return await _context.orders.ToListAsync();
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(DateTime id)
        {
          if (_context.orders == null)
          {
              return NotFound();
          }
            var order = await _context.orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(DateTime id, Order order)
        {
            if (id != order.OrderedAt)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Order
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
          if (_context.orders == null)
          {
              return Problem("Entity set 'DataContext.orders'  is null.");
          }
            _context.orders.Add(order);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderExists(order.OrderedAt))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrder", new { id = order.OrderedAt }, order);
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(DateTime id)
        {
            if (_context.orders == null)
            {
                return NotFound();
            }
            var order = await _context.orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(DateTime id)
        {
            return (_context.orders?.Any(e => e.OrderedAt == id)).GetValueOrDefault();
        }


        [HttpPost("load-orders")]
        public IActionResult LoadEntities([FromBody] List<Order> ordersDataLista)
        {
            try
            {
            
                if (ordersDataLista == null)
                {
                    return BadRequest("No data registered.");
                }

                
                foreach (var orderData in ordersDataLista)
                {
                
                    int userId = orderData.UserId;
                    int dishId = orderData.DishId;
                    DateTime orderAt = orderData.OrderedAt;

        
                    var user = _context.Users.Find(userId);
                    var dish = _context.dishes.Find(dishId);

                    if (user != null && dish != null)
                    {
                        var newOrder = new Order
                        {
                            UserId = userId,
                            DishId = dishId,
                            OrderedAt = orderAt
                        };

                    
                        _context.orders.Add(newOrder);
                        _context.SaveChanges();
                    }
                }

                
                return Ok("orders acquired.");
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Error at load: {ex.Message}");
            }
        }


        
        [HttpGet("user-order-stats")]
        public IActionResult GetUserOrderStats()
        {
            DateTime currentDate = DateTime.Now;
            var users = _context.Users.ToList();
            var userStats = new List<object>();
            foreach (var user in users){
                var orderDishes = _context.orders
                    .Where(r => r.UserId == user.Id )
                    .Join(_context.dishes, r => r.DishId, b => b.Id, (r, b) => b)
                    .ToList();


                var userOrderStats = new
                {
                    UserName = user.Name,
                    UserRut = user.Rut,
                    orderDishes = orderDishes
                    
                };
                userStats.Add(userOrderStats);

            }
            return Ok(userStats);
        }

        [HttpGet("dish-order-stats")]
        public IActionResult GetDishOrderStats()
        {
           {
        var dishes = _context.dishes.ToList();
        var dishestats = new List<object>();

        foreach (var dish in dishes)
        {
            var orderByUsers = _context.dishes
                .Where(r => r.Id == dish.Id)
                .Join(_context.Users, r => r.Id, u => u.Id, (r, u) => u)
               
                .ToList();


            var dishOrderStats = new
            {
                DishId = dish.Id,
                DishName = dish.Name,
                
                OrderByUsers = orderByUsers,
                
            };

            dishestats.Add(dishOrderStats);
        }

        return Ok(dishestats);
}
        }

    }

    
}
