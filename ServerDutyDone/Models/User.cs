﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerDutyDone.Models;

public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? Username { get; set; }

    [StringLength(100)]
    public string? UserPassword { get; set; }

    [InverseProperty("GroupAdminNavigation")]
    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    [InverseProperty("User")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}