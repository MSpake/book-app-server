using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using book_app_server.Models;

namespace book_app_server.Controllers
{
    [Route("/")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookappContext _context;

        public BookController(BookappContext context) {
          _context = context;
        }

        [HttpGet("books")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Book.ToListAsync();
        }

        [HttpGet("books/{id}")]
        public async Task<ActionResult<Book>> GetBook(long id)
        {
            var book = await _context.Book.FindAsync(id);

            if(book == null) {
              return NotFound();
            }
            return book;
        }

        [HttpPost("books")]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
          var shelf = _context.BookShelf.Where(b => b.BookShelfName == book.Bookshelf).FirstOrDefault();

          if(shelf == null) {
            BookShelf toAdd = new BookShelf();
            toAdd.BookShelfName = book.Bookshelf;

            _context.BookShelf.Add(toAdd);
            await _context.SaveChangesAsync();

            shelf = _context.BookShelf.Where(b => b.BookShelfName == book.Bookshelf).FirstOrDefault();
          }

          book.BookShelfId = shelf.BookShelfId;

          _context.Book.Add(book);
          await _context.SaveChangesAsync();

          return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpPut("books/{id}")]
        public async Task<ActionResult<Book>> PutBook(long id, Book book)
        {
          if(id != book.Id) 
          {
            return BadRequest();
          }

          var result = _context.Book.FirstOrDefault(b => b.Id == book.Id);


          var shelf = _context.BookShelf.Where(b => b.BookShelfName == book.Bookshelf).FirstOrDefault();

          if(shelf == null) {
            BookShelf toAdd = new BookShelf();
            toAdd.BookShelfName = book.Bookshelf;

            _context.BookShelf.Add(toAdd);
            await _context.SaveChangesAsync();

            shelf = _context.BookShelf.Where(b => b.BookShelfName == book.Bookshelf).FirstOrDefault();
          }
          
          result.Title = book.Title;
          result.Bookshelf = book.Bookshelf;
          result.Author = book.Author;
          result.ISBN = book.ISBN;
          result.BookShelfId = shelf.BookShelfId;

          _context.Update(result);
          await _context.SaveChangesAsync();

          var newlySavedBook = await _context.Book.FindAsync(id);

          return newlySavedBook;
        }

        [HttpDelete("books/{id}")]
        public async Task<ActionResult> DeleteBook(long id)
        {
          var bookToDelete = await _context.Book.FindAsync(id);

          if(bookToDelete == null)
          {
            return NotFound();
          }

          _context.Book.Remove(bookToDelete);
          await _context.SaveChangesAsync();
          return NoContent();
        }

        [HttpGet("bookshelves")]
        public async Task<ActionResult<IEnumerable<BookShelf>>> GetBookShelves()
        {
           var booklist = await _context.BookShelf.ToListAsync();

           booklist.ForEach( shelf => {
            shelf.Books =  _context.Book.Where(b => b.BookShelfId == shelf.BookShelfId).ToList();
           });

           return booklist;
        }

        [HttpGet("bookshelves/{id}")]
        public async Task<ActionResult<BookShelf>> GetBookShelf(long id)
        {
            var bookShelf = await _context.BookShelf.FindAsync(id);

            bookShelf.Books = await _context.Book.Where(b => b.BookShelfId == bookShelf.BookShelfId).ToListAsync();

            if(bookShelf == null) {
              return NotFound();
            }
            return bookShelf;
        }

        [HttpPost("bookshelves")]
        public async Task<ActionResult<BookShelf>> PostBookShelf(BookShelf bookShelf)
        {
          _context.BookShelf.Add(bookShelf);
          await _context.SaveChangesAsync();

          return CreatedAtAction(nameof(GetBookShelf), new {id = bookShelf.BookShelfId}, bookShelf);
        }

        [HttpPut("bookshelves/{id}")]
        public async Task<ActionResult<BookShelf>> PutBookShelf(long id, BookShelf bookShelf)
        {
          //Need to add functionality to change the name of the bookshelf in all associated books

          if(id != bookShelf.BookShelfId) {
            return BadRequest();
          }

          _context.Entry(bookShelf).State = EntityState.Modified;
          await _context.SaveChangesAsync();

          BookShelf newlySavedBookShelf = await _context.BookShelf.FindAsync(id);

          // List<Book> books = await _context.Book.Where(b => b.BookShelfId == id).ToListAsync();
          
          // books.ForEach(book => {
          //   book.BookShelfId = id;
          //   book.Bookshelf = newlySavedBookShelf.BookShelfName;
          // });

          return newlySavedBookShelf;
        }

        [HttpDelete("bookshelves/{id}")]
        public async Task<ActionResult> DeleteBookShelf(long id)
        {
          var bookShelfToDelete = await _context.BookShelf.FindAsync(id);

          if(bookShelfToDelete == null)
          {
            return NotFound();
          }

          List<Book> books = await _context.Book.Where(b => b.BookShelfId == bookShelfToDelete.BookShelfId).ToListAsync();
          
          books.ForEach(book => {
            book.BookShelfId = 0;
            book.Bookshelf = null;
          });

          _context.BookShelf.Remove(bookShelfToDelete);
          await _context.SaveChangesAsync();
          return NoContent();
        }
    }
}
