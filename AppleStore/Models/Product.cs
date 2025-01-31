using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleStore.Models;

public class Product
{
    [Key]
    [Column("idproduct")]
    public int IDProduct { get; set; }
    [ForeignKey("Category")]
    [Column("categoryid")]
    public int CategoryID { get; set; }
    [Column("productbrand")]
    public string ProductBrand { get; set; }
    [Column("productname")]
    public string ProductName { get; set; }
    [Column("productdescription")]
    public string ProductDescription { get; set; }
    [Column("productprice")]
    public decimal ProductPrice { get; set; }
    [Column("productimage")]
    public string ProductImage { get; set; }
    [Column("productcount")]
    public int ProductCount { get; set; }
    public virtual Category Category { get; set; }
    public ICollection<Review> Reviews { get; set; }
}