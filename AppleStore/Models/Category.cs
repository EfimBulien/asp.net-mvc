using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleStore.Models;

public class Category
{
    [Key]
    [Column("idcategory")]
    public int IdCategory { get; set; }
    [Column("category")]
    public string CategoryType { get; set; }
}