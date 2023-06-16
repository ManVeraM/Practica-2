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
    public class DishController : ControllerBase
    {
        private readonly DataContext _context;

        public DishController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Dish
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> Getdishes()
        {
          if (_context.dishes == null)
          {
              return NotFound();
          }
            return await _context.dishes.ToListAsync();
        }

        // GET: api/Dish/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
          if (_context.dishes == null)
          {
              return NotFound();
          }
            var dish = await _context.dishes.FindAsync(id);

            if (dish == null)
            {
                return NotFound();
            }

            return dish;
        }

        // PUT: api/Dish/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDish(int id, Dish dish)
        {
            if (id != dish.Id)
            {
                return BadRequest();
            }

            _context.Entry(dish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(id))
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

        // POST: api/Dish
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Dish>> PostDish(Dish dish)
        {
          if (_context.dishes == null)
          {
              return Problem("Entity set 'DataContext.dishes'  is null.");
          }
            _context.dishes.Add(dish);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDish", new { id = dish.Id }, dish);
        }

        // DELETE: api/Dish/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            if (_context.dishes == null)
            {
                return NotFound();
            }
            var dish = await _context.dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            _context.dishes.Remove(dish);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DishExists(int id)
        {
            return (_context.dishes?.Any(e => e.Id == id)).GetValueOrDefault();
        }


         [HttpPost("load-dishes")]
    public IActionResult LoadDishes([FromBody] List<Dish> dishesDataList)
    {
        try
        {
            
            if (dishesDataList == null)
            {
                return BadRequest("No data was registered.");
            }

        
            foreach (var dishData in dishesDataList)
            {   
                
        
                string? dishName = dishData.Name;

                int price = dishData.Price;
            
                var newDish = new Dish
                {
                    Name = dishName,
                    Price = price
                    
                    
                };

                
                _context.dishes.Add(newDish);
                _context.SaveChanges();
            }

            
            return Ok("dishes acquired.");
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
    }
}
