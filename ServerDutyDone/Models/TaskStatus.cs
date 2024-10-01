using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerDutyDone.Models;

[Table("TaskStatus")]
public partial class TaskStatus
{
    [Key]
    public int StatusId { get; set; }

    [StringLength(100)]
    public string? TypeName { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
