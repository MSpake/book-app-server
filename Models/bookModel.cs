using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace book_app_server.Models {
  [Table("Book")]
  public class Book {      
    [Key, Required]  
    public long Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string ISBN { get; set; }
    public string Author { get; set; }

    public string Bookshelf { get; set; }
    [ForeignKey("BookShelfId")]
    public long BookShelfId { get; set; }
  }
}