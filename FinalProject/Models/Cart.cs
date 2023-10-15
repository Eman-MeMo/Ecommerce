namespace FinalProject.Models
{
    public class Cart
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CartProduct> CartProducts { get; set; }
    }
}
