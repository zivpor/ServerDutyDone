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
            entity.HasKey(e => e.GroupId).HasName("PK__Groups__149AF36A4DC4C0D0");

            entity.HasOne(d => d.GroupAdminNavigation).WithMany(p => p.Groups).HasConstraintName("FK__Groups__GroupAdm__2F10007B");

            entity.HasOne(d => d.GroupTypeNavigation).WithMany(p => p.Groups).HasConstraintName("FK__Groups__GroupTyp__2E1BDC42");
        });

        modelBuilder.Entity<GroupType>(entity =>
        {
            entity.HasKey(e => e.GroupTypeId).HasName("PK__GroupTyp__12195AADDB8538B0");

            entity.Property(e => e.GroupTypeId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Tasks__7C6949B1E71EF5FE");

            entity.HasOne(d => d.Group).WithMany(p => p.Tasks).HasConstraintName("FK__Tasks__GroupId__31EC6D26");

            entity.HasOne(d => d.Status).WithMany(p => p.Tasks).HasConstraintName("FK__Tasks__StatusId__33D4B598");

            entity.HasOne(d => d.TaskTypeNavigation).WithMany(p => p.Tasks).HasConstraintName("FK__Tasks__TaskType__34C8D9D1");

            entity.HasOne(d => d.User).WithMany(p => p.Tasks).HasConstraintName("FK__Tasks__UserId__32E0915F");
        });

        modelBuilder.Entity<TaskStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__TaskStat__C8EE2063756D8547");

            entity.Property(e => e.StatusId).ValueGeneratedNever();
        });

        modelBuilder.Entity<TaskType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__TaskType__516F03B51C567E08");

            entity.Property(e => e.TypeId).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CC02AFEB0");

            entity.HasMany(d => d.GroupsNavigation).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UsersInGroup",
                    r => r.HasOne<Group>().WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UsersInGr__Group__38996AB5"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UsersInGr__UserI__37A5467C"),
                    j =>
                    {
                        j.HasKey("UserId", "GroupId").HasName("PK__UsersInG__A6C1637A1DA022C2");
                        j.ToTable("UsersInGroup");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
