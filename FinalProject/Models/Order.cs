namespace FinalProject.Models
{
    public class Order
    {
        public int id { get; set; }
        public decimal totalPrice { get; set; }
        public DateTime date { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}
