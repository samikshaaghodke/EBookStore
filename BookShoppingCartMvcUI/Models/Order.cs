using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShoppingCartMvcUI.Models
{
    [Table("Order")]
    public class Order
    {
        public int Id { get; set; } //Creates PK Id

        [Required]
        public string UserId { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; //Default value

        [Required]
        public int OrderStatusId { get; set; } //Creates FK (ParentTableNameId)

        public bool IsDeleted { get; set; } = false;

        public OrderStatus OrderStatus { get; set; }

        public List<OrderDetail> OrderDetail { get; set; }
    }
}
