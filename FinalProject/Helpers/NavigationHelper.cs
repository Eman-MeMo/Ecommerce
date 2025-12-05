namespace FinalProject.Helpers
{
    public static class NavigationHelper
    {
        public static List<(string Name, string Link)> GetNavItems(bool isCustomer)
        {
            if (isCustomer)
            {
                return new List<(string, string)>
                {
                    ("Home", "/Product/ShowForUser"),
                    ("Shop", "/Product/ShowForUser"),
                    ("Cart", "/Cart/Index"),
                    ("Profile", "/User/Profile")
                };
            }
            else 
            {
                return new List<(string, string)>
                {
                    ("Products", "/Product/ShowForAdmin"),
                    ("Categories", "/Category/Index"),
                    ("Customers", "/User/Index"),
                    ("Orders", "/Order/Index")
                };
            }
        }
    }
}
