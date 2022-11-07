using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DymsaInventory.Models;

public partial class _ApplicationDbContext : DbContext
{
    public _ApplicationDbContext()
    {
    }

    public _ApplicationDbContext(DbContextOptions<_ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemCode> ItemCodes { get; set; }

    public virtual DbSet<ItemGenre> ItemGenres { get; set; }

    public virtual DbSet<PurchaseTransaction> PurchaseTransactions { get; set; }

    public virtual DbSet<SaleTransaction> SaleTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("Item");

            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.ItemCode).WithMany(p => p.Items)
                .HasForeignKey(d => d.ItemCodeId)
                .HasConstraintName("FK_Item_ItemCode");

            entity.HasOne(d => d.ItemGenre).WithMany(p => p.Items)
                .HasForeignKey(d => d.ItemGenreId)
                .HasConstraintName("FK_Item_ItemGenre");
        });

        modelBuilder.Entity<ItemCode>(entity =>
        {
            entity.ToTable("ItemCode");

            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<ItemGenre>(entity =>
        {
            entity.ToTable("ItemGenre");

            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<PurchaseTransaction>(entity =>
        {
            entity.ToTable("PurchaseTransaction");

            entity.Property(e => e.AdditionalFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");

            entity.HasOne(d => d.Item).WithMany(p => p.PurchaseTransactions)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_PurchaseTransaction_Item");
        });

        modelBuilder.Entity<SaleTransaction>(entity =>
        {
            entity.HasKey(e => e.SaleTransactionId).HasName("PK__SaleTran__FB5296A4C23B15E5");

            entity.ToTable("SaleTransaction");

            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DiscountOrCommission).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            entity.Property(e => e.Net)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("NET");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");

            entity.HasOne(d => d.Item).WithMany(p => p.SaleTransactions)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__SaleTrans__ItemI__6477ECF3");
        });

        base.OnModelCreating(modelBuilder);
    }

 
}
