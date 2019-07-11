using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace book_app_server.Models {
  [Table("Bookshelf")]
  public class BookShelf {        
    [Key, Required]
    public long BookShelfId { get; set; }
    [Required]
    public string BookShelfName { get; set; }
    public List<Book> Books { get; set; }
  }
}