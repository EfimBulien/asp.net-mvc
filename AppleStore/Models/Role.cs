using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleStore.Models;

public class Role
{
    [Key]
    [Column("idrole")]
    public int IDRole { get; set; }
    [Column("rolename")]
    public string RoleName { get; set; }
}