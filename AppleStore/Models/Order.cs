using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleStore.Models;

public class Order
{
    [Key]
    [Column("idorder")]
    public int IDOrder { get; set; }
    [ForeignKey("User")]
    [Column("userid")]
    public int UserID { get; set; }
    [Column("orderdate")]
    public DateTime OrderDate { get; set; }
    [Column("orderaddress")]
    public string OrderAddress { get; set; }
    [ForeignKey("PayWay")]
    [Column("paywayid")]
    public int PayWayID { get; set; }
    [Column("totalcount")]
    public int TotalCount { get; set; }
    
    public virtual User User { get; set; }
    public virtual PayWay PayWay { get; set; }
    public virtual ICollection<OrderProduct> OrderProducts { get; set; }
}