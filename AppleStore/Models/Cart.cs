namespace AppleStore.Models;
public class Cart
{
    public int CartId { get; init; }
    public string? UserLogin { get; init; }
    public List<Product> CartLines { get; init; } = [];
    public decimal FinalPrice
    {
        get
        {
            return CartLines.Sum(line => line.ProductPrice);
        }
    }
}
