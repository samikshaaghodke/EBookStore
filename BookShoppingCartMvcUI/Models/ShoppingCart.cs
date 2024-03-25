using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShoppingCartMvcUI.Models
{
    [Table("ShoppingCart")]
    public class ShoppingCart
    {
        public int Id { get; set; } //PK

        [Required]
        public string UserId { get; set; }

        public bool IsDeleted { get; set; } = false; //logical deletion to maintain history

        public ICollection<CartDetail> CartDetails { get; set; } //represents a one-to-many relationship between the ShoppingCart table and the CartDetail table.

    }
}
