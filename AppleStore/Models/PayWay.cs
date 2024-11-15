using System.ComponentModel.DataAnnotations;

namespace AppleStore.Models;

public class PayWay
{
    [Key]
    public int IDPayWay { get; set; }
    public string PayWayMethod { get; set; }
}