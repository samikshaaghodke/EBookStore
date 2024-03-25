using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShoppingCartMvcUI.Models
{
    [Table("Book")] //A table with this name will be created in Database
    public class Book
    {
        public int Id { get; set; }

        [Required] //Data Annotation
        [MaxLength(40)]
        public string? BookName { get; set; }

        [Required]
        [MaxLength(40)]
        public string? AuthorName { get; set; }

        [Required]
        public double Price { get; set; }

        public string? Image { get; set; }

        [Required]
        public int GenreId { get; set; } //FK

        public Genre Genre { get; set; }

        public List<OrderDetail> OrderDetail { get; set; } //one to many relationship between book and OrderDetail

        public List<CartDetail> CartDetail { get; set; }//one to many relationship between book and CartDetail

        [NotMapped]
        public string GenreName { get; set; }

    }
}
