namespace AppleStore.Models;

public class Cart
{
    public List<Product> CartLines { get; set; } = [];

    public decimal FinalPrice
    {
        get
        {
            return CartLines.Sum(line => line.ProductPrice);
        }
    }
}
