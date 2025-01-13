using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerDutyDone.Models;

public partial class Group
{
    [Key]
    public int GroupId { get; set; }

    public int? GroupAdmin { get; set; }

    [StringLength(100)]
    public string? GroupName { get; set; }

    public int? Capacity { get; set; }

    public int? GroupType { get; set; }

    [ForeignKey("GroupAdmin")]
    [InverseProperty("Groups")]
    public virtual User? GroupAdminNavigation { get; set; }

    [ForeignKey("GroupType")]
    [InverseProperty("Groups")]
    public virtual GroupType? GroupTypeNavigation { get; set; }

    [InverseProperty("Group")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    [ForeignKey("GroupId")]
    [InverseProperty("GroupsNavigation")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
