using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class CartProduct
    {
        public int quantity { get; set; }
        [Key]
        [Column(Order = 1)] // Specify the order for composite key
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Key]
        [Column(Order = 2)] // Specify the order for composite key
        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
