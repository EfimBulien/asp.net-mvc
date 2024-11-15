using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleStore.Models;

public class OrderProduct
{
    [Key]
    public int IDOrderProduct { get; set; }
    [ForeignKey("Order")]
    public int OrderID { get; set; }
    [ForeignKey("Product")]
    public int ProductID { get; set; }
    public int TotalCount { get; set; }
    public virtual Order Order { get; set; }
    public virtual Product Product { get; set; }
}