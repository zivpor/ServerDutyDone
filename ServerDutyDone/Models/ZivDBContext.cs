using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ServerDutyDone.Models;

public partial class ZivDBContext : DbContext
{
    public ZivDBContext()
    {
    }

    public ZivDBContext(DbContextOptions<ZivDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupType> GroupTypes { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskStatus> TaskStatuses { get; set; }

    public virtual DbSet<TaskType> TaskTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=DutyDone_DB;User ID=TaskAdminUser;Password=kukuPassword;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK__Groups__149AF36A8F9EBB8A");

            entity.HasOne(d => d.GroupAdminNavigation).WithMany(p => p.Groups).HasConstraintName("FK__Groups__GroupAdm__300424B4");

            entity.HasOne(d => d.GroupTypeNavigation).WithMany(p => p.Groups).HasConstraintName("FK__Groups__GroupTyp__2F10007B");
        });

        modelBuilder.Entity<GroupType>(entity =>
        {
            entity.HasKey(e => e.GroupTypeId).HasName("PK__GroupTyp__12195AAD7E6E388E");

            entity.Property(e => e.GroupTypeId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Tasks__7C6949B120EA9882");

            entity.HasOne(d => d.Group).WithMany(p => p.Tasks).HasConstraintName("FK__Tasks__GroupId__36B12243");

            entity.HasOne(d => d.Status).WithMany(p => p.Tasks).HasConstraintName("FK__Tasks__StatusId__38996AB5");

            entity.HasOne(d => d.TaskTypeNavigation).WithMany(p => p.Tasks).HasConstraintName("FK__Tasks__TaskType__398D8EEE");

            entity.HasOne(d => d.User).WithMany(p => p.Tasks).HasConstraintName("FK__Tasks__UserId__37A5467C");
        });

        modelBuilder.Entity<TaskStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__TaskStat__C8EE206378AB155D");

            entity.Property(e => e.StatusId).ValueGeneratedNever();
        });

        modelBuilder.Entity<TaskType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__TaskType__516F03B597BB6298");

            entity.Property(e => e.TypeId).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CF2D75098");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
