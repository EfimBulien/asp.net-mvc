namespace AppleStore.Models;
    public class Cart
    {
        public int CartID { get; set; }
        public string UserLogin { get; set; }
        public List<Product> CartLines { get; set; } = [];

        public decimal FinalPrice
        {
            get
            {
                return CartLines.Sum(line => line.ProductPrice);
            }
        }
    }
