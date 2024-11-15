using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleStore.Models;

public class User
{
    [Key]
    [Column("iduser")]
    public int IDUser  { get; set; }
    [Column("userlogin")]
    public string UserLogin { get; set; }
    [Column("userpassword")]
    public string UserPassword { get; set; }
    [Column("useremail")]
    public string UserEmail { get; set; }
    [Column("creationdate")]
    public DateTime CreationDate { get; set; }
    
    [ForeignKey("Role")]
    [Column("roleid")]
    public int RoleID { get; set; }
    public virtual Role Role { get; set; }
}