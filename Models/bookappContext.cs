using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace book_app_server.Models {
  public class BookappContext : DbContext {
    public BookappContext(DbContextOptions<BookappContext> options) : base(options) {}
    public DbSet<Book> Book { get; set; }
    public DbSet<BookShelf> BookShelf { get; set; }
  }

}
