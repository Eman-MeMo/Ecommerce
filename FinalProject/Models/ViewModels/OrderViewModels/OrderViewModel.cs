using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.ViewModels.OrderViewModels
{
    public class OrderViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public Dictionary<int, string> Usernames { get; set; }
        public Dictionary<int, string> ProductNames { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
