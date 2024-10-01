using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerDutyDone.Models;

[Table("GroupType")]
public partial class GroupType
{
    [Key]
    public int GroupTypeId { get; set; }

    [StringLength(100)]
    public string? GroupTypeName { get; set; }

    [InverseProperty("GroupTypeNavigation")]
    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}
