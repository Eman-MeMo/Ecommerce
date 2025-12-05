namespace FinalProject.Models.ViewModels.CartViewModels
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int mount { get; set; }

        public string? Image { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
