namespace FinalProject.Models.ViewModels.CartViewModels
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();
        public bool IsEmpty => !Items.Any();

        public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);
    }
}
