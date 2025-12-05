using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class OrderItem
    {
        [Key]
        [Column(Order = 1)] // Specify the order for composite key
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        [Key]
        [Column(Order = 2)] // Specify the order for composite key
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int quantity { get; set; }
    }
}
