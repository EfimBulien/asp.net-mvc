using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleStore.Models;

public class Review
{
    [Key] [Column("idreview")] public int IDReview { get; set; }

    [ForeignKey("User")]
    [Column("userid")]
    public int UserID { get; set; }

    public virtual User User { get; set; }

    [ForeignKey("Product")]
    [Column("productid")]
    public int ProductID { get; set; }

    public virtual Product Product { get; set; }

    [Column("rating")] public int Rating { get; set; }

    [Column("reviewtext")] public string ReviewText { get; set; }

    [Column("createdat")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedAt { get; set; }
}