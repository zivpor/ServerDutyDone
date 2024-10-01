using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerDutyDone.Models;

[Table("TaskType")]
public partial class TaskType
{
    [Key]
    public int TypeId { get; set; }

    [StringLength(100)]
    public string? TypeName { get; set; }

    [InverseProperty("TaskTypeNavigation")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
