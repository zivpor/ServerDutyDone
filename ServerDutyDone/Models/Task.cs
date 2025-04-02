using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerDutyDone.Models;

public partial class Task
{
    [Key]
    public int TaskId { get; set; }

    public int? TaskType { get; set; }

    public DateOnly? DueDay { get; set; }

    public int? UserId { get; set; }

    [StringLength(100)]
    public string? TaskName { get; set; }

    public int? GroupId { get; set; }

    public int? StatusId { get; set; }

    [StringLength(500)]
    public string? TaskDescription { get; set; }

    [StringLength(500)]
    public string? TaskUpdate { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("Tasks")]
    public virtual Group? Group { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("Tasks")]
    public virtual TaskStatus? Status { get; set; }

    [ForeignKey("TaskType")]
    [InverseProperty("Tasks")]
    public virtual TaskType? TaskTypeNavigation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Tasks")]
    public virtual User? User { get; set; }
}
