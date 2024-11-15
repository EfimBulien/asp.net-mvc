using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleStore.Models;

public class PayWay
{
    [Key]
    [Column("idpayway")]
    public int IDPayWay { get; set; }
    [Column("payway")]
    public string PayWayMethod { get; set; }
}