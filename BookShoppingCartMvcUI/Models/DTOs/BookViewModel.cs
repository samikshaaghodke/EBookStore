
using System.ComponentModel.DataAnnotations;

public class BookViewModel
{
    public int Id { get; set; }

    [Required]
    [MaxLength(40)]
    public string BookName { get; set; }

    [Required]
    [MaxLength(40)]
    public string AuthorName { get; set; }

    [Required]  
    public double Price { get; set; }

    [Required]
    public int GenreId { get; set; }
}
