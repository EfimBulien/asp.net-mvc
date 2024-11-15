namespace AppleStore.Models;

public class Cart
{
    public List<Product> cartLines { get; set; }
    
    public Cart()
    {
        cartLines = new List<Product>();
    }

    public decimal finalPrice
    {
        get
        {
            return cartLines.Sum(line => line.ProductPrice);
        }
    }
}