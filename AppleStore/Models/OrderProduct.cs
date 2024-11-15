using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleStore.Models;

public class OrderProduct
{
    [Key]
    [Column("idorderproduct")]
    public int IDOrderProduct { get; set; }
    [ForeignKey("Order")]
    [Column("orderid")]
    public int OrderID { get; set; }
    [ForeignKey("Product")]
    public int ProductID { get; set; }
    public int TotalCount { get; set; }
    public virtual Order Order { get; set; }
    public virtual Product Product { get; set; }
}