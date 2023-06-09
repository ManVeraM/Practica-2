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
    public class ReservationController : ControllerBase
    
    {
        private readonly DataContext _context;

        public ReservationController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]

        // GET: api/Reservation
        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<Reserved>>> CantReservation()
        {
          if (_context.Reserveds == null)
          {
              return NotFound();
          }

            return await _context.Reserveds.ToListAsync();
        }



        public async Task<ActionResult<IEnumerable<Reserved>>> GetReserveds()
        {
          if (_context.Reserveds == null)
          {
              return NotFound();
          }
            return await _context.Reserveds.ToListAsync();
        }

        // GET: api/Reservation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reserved>> GetReserved(DateTime id)
        {
          if (_context.Reserveds == null)
          {
              return NotFound();
          }
            var reserved = await _context.Reserveds.FindAsync(id);

            if (reserved == null)
            {
                return NotFound();
            }

            return reserved;
        }

        // PUT: api/Reservation/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserved(DateTime id, Reserved reserved)
        {
            if (id != reserved.ReservedAt)
            {
                return BadRequest();
            }

            _context.Entry(reserved).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservedExists(id))
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

        // POST: api/Reservation
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Reserved>> PostReserved(Reserved reserved)
        {
          if (_context.Reserveds == null)
          {
              return Problem("Entity set 'DataContext.Reserveds'  is null.");
          }
            _context.Reserveds.Add(reserved);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ReservedExists(reserved.ReservedAt))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetReserved", new { id = reserved.ReservedAt }, reserved);
        }

        // DELETE: api/Reservation/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserved(DateTime id)
        {
            if (_context.Reserveds == null)
            {
                return NotFound();
            }
            var reserved = await _context.Reserveds.FindAsync(id);
            if (reserved == null)
            {
                return NotFound();
            }

            _context.Reserveds.Remove(reserved);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservedExists(DateTime id)
        {
            return (_context.Reserveds?.Any(e => e.ReservedAt == id)).GetValueOrDefault();
        }




        [HttpGet("last-reserved-date")]
        public IActionResult GetLastReservedDate()
        {
            var lastReservedDate = _context.Reserveds
                .OrderByDescending(r => r.ReservedAt)
                .Select(r => r.ReservedAt)
                .FirstOrDefault();

            return Ok(lastReservedDate);
        }

        [HttpGet("books-reserved")]
        public IActionResult GetBooksReservedThisMonth()
        {
            DateTime currentDate = DateTime.Now;
            var reservedBooks = _context.Reserveds
                .Where(r => r.ReservedAt.Year == currentDate.Year && r.ReservedAt.Month == currentDate.Month)
                .Join(_context.Books, r => r.BookId, b => b.Id, (r, b) => b)
                .ToList();

            return Ok(reservedBooks);
        }

        [HttpGet("book-count")]
        public IActionResult GetBookReservationCount()
        {
            DateTime currentDate = DateTime.Now;
            int count = _context.Reserveds
                .Count(r => r.ReservedAt.Year == currentDate.Year && r.ReservedAt.Month == currentDate.Month);

            return Ok(count);
        }


        
    }
}
