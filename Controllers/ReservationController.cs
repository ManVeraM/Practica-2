using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRACTICA2.Models;

namespace PRACTICA2.Controllers
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


    [HttpGet("books")]
    public IActionResult GetAllBooks()
    {
        try
        {
            // Obtener todos los libros de la base de datos
            var books = _context.Books.ToList();

            // Devolver una respuesta exitosa con la lista de libros
            return Ok(books);
        }
        catch (Exception ex)
        {
            // En caso de que ocurra una excepción, devolver un mensaje de error
            return StatusCode(500, $"Error al obtener los libros: {ex.Message}");
        }
    }

    [HttpGet("users")]
    public IActionResult GetAllUsers()
    {
        try
        {
            
            var users = _context.Users.ToList();

            
            return Ok(users);
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, $"Error {ex.Message}");
        }
    }

    [HttpGet("reservations")]
    public IActionResult GetAllReservations()
    {
        try
        {
            
            var reservations = _context.Reserveds.ToList();

            
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, $"Error {ex.Message}");
        }
    }

        [HttpPost("load-reserves")]
    public IActionResult LoadEntities([FromBody] List<Reserved> reservationDataList)
    {
        try
        {
           
            if (reservationDataList == null)
            {
                return BadRequest("No data registered.");
            }

            
            foreach (var reservationData in reservationDataList)
            {
               
                int userId = reservationData.UserId;
                int bookId = reservationData.BookId;
                DateTime reservedAt = reservationData.ReservedAt;

       
                var user = _context.Users.Find(userId);
                var book = _context.Books.Find(bookId);

                if (user != null && book != null)
                {
                    var newReservation = new Reserved
                    {
                        UserId = userId,
                        BookId = bookId,
                        ReservedAt = reservedAt
                    };

                  
                    _context.Reserveds.Add(newReservation);
                    _context.SaveChanges();
                }
            }

            
            return Ok("Reservations acquired.");
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, $"Error at load: {ex.Message}");
        }
    }


    [HttpPost("load-users")]
    public IActionResult LoadUsers([FromBody] List<User> userDataList)
    {
        try
        {
            
            if (userDataList == null)
            {
                return BadRequest("No data was registered.");
            }

        
            foreach (var userData in userDataList)
            {   
                
        
                string? userName = userData.Name;

                string? faculty = userData.Faculty;

                string? code = userData.Code;
            
                var newUser = new User
                {
                    Name = userName,
                    Faculty = faculty,
                    Code = code
                    
                    
                };

                
                _context.Users.Add(newUser);
                _context.SaveChanges();
            }

            
            return Ok("users acquired.");
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }


    [HttpPost("load-books")]
public IActionResult LoadBooks([FromBody] List<Book> bookDataList)
{
    try
    {
       
        if (bookDataList == null)
        {
            return BadRequest("No data was registered.");
        }

      
        foreach (var bookData in bookDataList)
        {
            
            string? bookTitle = bookData.Title;

            string? code = bookData.Code;

            string? description = bookData.Description;

      
            var newBook = new Book
            {
                Title = bookTitle,
                Description = description,
                Code = code
         
            };

            _context.Books.Add(newBook);
            _context.SaveChanges();
        }

        
        return Ok("books acquired.");
    }
    catch (Exception ex)
    {
        
        return StatusCode(500, $"Error {ex.Message}");
    }
}




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
        private bool ReservedExists(DateTime id)
        {
            return (_context.Reserveds?.Any(e => e.ReservedAt == id)).GetValueOrDefault();
        }

        [HttpGet("user-reservation-stats")]
        public IActionResult GetUserReservationStats()
        {
            DateTime currentDate = DateTime.Now;
            var users = _context.Users.ToList();
            var userStats = new List<object>();
            foreach (var user in users){
                int bookReservationCount = _context.Reserveds
                .Count(r => r.UserId == user.Id && r.ReservedAt.Year == currentDate.Year && r.ReservedAt.Month == currentDate.Month);

                var reservedBooks = _context.Reserveds
                    .Where(r => r.UserId == user.Id && r.ReservedAt.Year == currentDate.Year && r.ReservedAt.Month == currentDate.Month)
                    .Join(_context.Books, r => r.BookId, b => b.Id, (r, b) => b)
                    .Distinct()
                    .ToList();

                var lastReservedDate = _context.Reserveds
                    .Where(r=> r.UserId == user.Id)
                    .OrderByDescending(r => r.ReservedAt)
                    .Select(r => r.ReservedAt)
                    .FirstOrDefault();

                var userReservedstats = new
                {
                    UserName = user.Name,
                    UserFaculty = user.Faculty,
                    UserReservationCount = bookReservationCount,
                    LastReservedDate = lastReservedDate,
                    reservedBooks = reservedBooks
                    
                };
                userStats.Add(userReservedstats);

            }
            return Ok(userStats);
        }
        
        [HttpGet("book-reservation-stats")]
        public IActionResult GetBooksReservationStats()
       {
        var books = _context.Books.ToList();
        var bookStats = new List<object>();

        foreach (var book in books)
        {
            var reservedByUsers = _context.Reserveds
                .Where(r => r.BookId == book.Id)
                .Join(_context.Users, r => r.UserId, u => u.Id, (r, u) => u)
                .Distinct()
                .ToList();


            var bookReservationStats = new
            {
                BookId = book.Id,
                BookName = book.Title,
                
                ReservedByUsers = reservedByUsers,
                
            };

            bookStats.Add(bookReservationStats);
        }

        return Ok(bookStats);
}



    
        
    }

}
