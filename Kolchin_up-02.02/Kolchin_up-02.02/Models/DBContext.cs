using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kolchin_up_02._02.Models;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeRole> EmployeeRoles { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<PickupPoint> PickupPoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=up-02.02;Username=postgres;Password=postgres;SSL Mode=Disable");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.HasIndex(e => e.CategoryName, "categories_category_name_key").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("employees_pkey");

            entity.ToTable("employees");

            entity.HasIndex(e => e.Login, "employees_login_key").IsUnique();

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("login");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("middle_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_employees_role");
        });

        modelBuilder.Entity<EmployeeRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("employee_roles_pkey");

            entity.ToTable("employee_roles");

            entity.HasIndex(e => e.RoleName, "employee_roles_role_name_key").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.ManufacturerId).HasName("manufacturers_pkey");

            entity.ToTable("manufacturers");

            entity.HasIndex(e => e.ManufacturerName, "manufacturers_manufacturer_name_key").IsUnique();

            entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");
            entity.Property(e => e.ManufacturerName)
                .HasMaxLength(150)
                .HasColumnName("manufacturer_name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ClientFullName)
                .HasMaxLength(255)
                .HasColumnName("client_full_name");
            entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date");
            entity.Property(e => e.OrderDate).HasColumnName("order_date");
            entity.Property(e => e.PickupCode).HasColumnName("pickup_code");
            entity.Property(e => e.PickupPointId).HasColumnName("pickup_point_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.PickupPoint).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PickupPointId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_orders_pickup_point");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_orders_status");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("order_items_pkey");

            entity.ToTable("order_items");

            entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductArticle)
                .HasMaxLength(50)
                .HasColumnName("product_article");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_order_items_order");

            entity.HasOne(d => d.ProductArticleNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductArticle)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_order_items_product");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("order_statuses_pkey");

            entity.ToTable("order_statuses");

            entity.HasIndex(e => e.StatusName, "order_statuses_status_name_key").IsUnique();

            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(100)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<PickupPoint>(entity =>
        {
            entity.HasKey(e => e.PickupPointId).HasName("pickup_points_pkey");

            entity.ToTable("pickup_points");

            entity.HasIndex(e => e.Address, "pickup_points_address_key").IsUnique();

            entity.Property(e => e.PickupPointId).HasColumnName("pickup_point_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Article).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.Article)
                .HasMaxLength(50)
                .HasColumnName("article");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.Discount)
                .HasPrecision(5, 2)
                .HasColumnName("discount");
            entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");
            entity.Property(e => e.Photo)
                .HasMaxLength(255)
                .HasColumnName("photo");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("product_name");
            entity.Property(e => e.StockQuantity).HasColumnName("stock_quantity");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_products_category");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_products_manufacturer");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_products_supplier");

            entity.HasOne(d => d.Unit).WithMany(p => p.Products)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_products_unit");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("suppliers_pkey");

            entity.ToTable("suppliers");

            entity.HasIndex(e => e.SupplierName, "suppliers_supplier_name_key").IsUnique();

            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(150)
                .HasColumnName("supplier_name");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("units_pkey");

            entity.ToTable("units");

            entity.HasIndex(e => e.UnitName, "units_unit_name_key").IsUnique();

            entity.Property(e => e.UnitId).HasColumnName("unit_id");
            entity.Property(e => e.UnitName)
                .HasMaxLength(50)
                .HasColumnName("unit_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
