using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MiniBidlo.Models;

public partial class FlowerMagazinContext : DbContext
{
    public FlowerMagazinContext()
    {
    }

    public FlowerMagazinContext(DbContextOptions<FlowerMagazinContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<FlowerOrder> FlowerOrders { get; set; }

    public virtual DbSet<FlowerSupplier> FlowerSuppliers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<PosOrder> PosOrders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LALALALA\\SQLEXPRESS01;Initial Catalog=FlowerMagazin;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("PK__Category__E548B67311650905");

            entity.ToTable("Category");

            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("category_name");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
        });

        modelBuilder.Entity<FlowerOrder>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PK__FlowerOr__DD5B8F3FDDAB1F0E");

            entity.ToTable("FlowerOrder");

            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.ShippingAddress)
                .HasColumnType("text")
                .HasColumnName("shipping_address");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Sum)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("sum");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.FlowerOrders)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__FlowerOrd__id_us__1CBC4616");
        });

        modelBuilder.Entity<FlowerSupplier>(entity =>
        {
            entity.HasKey(e => e.IdSupplier).HasName("PK__FlowerSu__F6C576E67E9CA685");

            entity.ToTable("FlowerSupplier");

            entity.Property(e => e.IdSupplier).HasColumnName("id_supplier");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Phone_number");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PK__Order__DD5B8F3F0A9381CD");

            entity.ToTable("Order");

            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.ShippingAddress)
                .HasColumnType("text")
                .HasColumnName("shipping_address");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Sum)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("sum");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__Order__id_user__5070F446");
        });

        modelBuilder.Entity<PosOrder>(entity =>
        {
            entity.HasKey(e => e.IdPosOrder).HasName("PK__PosOrder__50E5BFC40C698A28");

            entity.ToTable("PosOrder");

            entity.Property(e => e.IdPosOrder).HasColumnName("id_pos_order");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.PosOrders)
                .HasForeignKey(d => d.IdOrder)
                .HasConstraintName("FK__PosOrder__id_ord__619B8048");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.PosOrders)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK__PosOrder__id_pro__628FA481");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PK__Product__BA39E84F408F79E2");

            entity.ToTable("Product");

            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.StockQuantity).HasColumnName("stock_quantity");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Product__categor__5DCAEF64");

            entity.HasMany(d => d.IdSuppliers).WithMany(p => p.IdProducts)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductSupplier",
                    r => r.HasOne<FlowerSupplier>().WithMany()
                        .HasForeignKey("IdSupplier")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ProductSu__id_su__0C85DE4D"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("IdProduct")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ProductSu__id_pr__0B91BA14"),
                    j =>
                    {
                        j.HasKey("IdProduct", "IdSupplier").HasName("PK__ProductS__C555BF21943AD086");
                        j.ToTable("ProductSupplier");
                        j.IndexerProperty<int>("IdProduct").HasColumnName("id_product");
                        j.IndexerProperty<int>("IdSupplier").HasColumnName("id_supplier");
                    });
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.IdSupplier).HasName("PK__Supplier__F6C576E6F07CF848");

            entity.ToTable("Supplier");

            entity.Property(e => e.IdSupplier).HasColumnName("id_supplier");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Phone_number");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__User__D2D146374C43F36D");

            entity.ToTable("User");

            entity.HasIndex(e => e.Login, "UQ__User__7838F272EB467B64").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__AB6E6164AFBA7218").IsUnique();

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Login)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
