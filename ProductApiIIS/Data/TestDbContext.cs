using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProductApiIIS.Data;

public partial class TestDbContext : DbContext
{
    public TestDbContext()
    {
    }

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EventLog> EventLogs { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductVersion> ProductVersions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=RX6600M\\SQLEXPRESS;Database=TestDb;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventLog__3214EC27EE23994C");

            entity.ToTable("EventLog");

            entity.HasIndex(e => e.EventDate, "IX_ProductVersion_EventDate");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.EventDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC273176332F");

            entity.ToTable("Product", tb =>
                {
                    tb.HasTrigger("Product_DELETE");
                    tb.HasTrigger("Product_INSERT");
                    tb.HasTrigger("Product_UPDATE");
                });

            entity.HasIndex(e => e.Name, "UQ__Product__737584F660A52571").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<ProductVersion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductV__3214EC2739900E5A");

            entity.ToTable("ProductVersion", tb =>
                {
                    tb.HasTrigger("ProductVersion_DELETE");
                    tb.HasTrigger("ProductVersion_INSERT");
                    tb.HasTrigger("ProductVersion_UPDATE");
                });

            entity.HasIndex(e => e.CreatingDate, "IX_ProductVersion_CreatingDate");

            entity.HasIndex(e => e.Height, "IX_ProductVersion_Height");

            entity.HasIndex(e => e.Length, "IX_ProductVersion_Length");

            entity.HasIndex(e => e.Name, "IX_ProductVersion_Name");

            entity.HasIndex(e => e.Width, "IX_ProductVersion_Width");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.CreatingDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVersions)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ProductVe__Produ__2D27B809");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
