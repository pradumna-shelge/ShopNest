using System;
using System.Collections.Generic;
using BackEnd.DTOs.spDTO;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Models;

public partial class ShopNestContext : DbContext
{
    public ShopNestContext()
    {
    }

    public ShopNestContext(DbContextOptions<ShopNestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CartItemDTO> CartItems { get; set; }
    public virtual DbSet<MstLocation> MstLocations { get; set; }

    public virtual DbSet<MstProduct> MstProducts { get; set; }

    public virtual DbSet<MstUser> MstUsers { get; set; }

    public virtual DbSet<MstUserRole> MstUserRoles { get; set; }

    public virtual DbSet<TrnAddToCart> TrnAddToCarts { get; set; }

    public virtual DbSet<TrnOrder> TrnOrders { get; set; }

    public virtual DbSet<TrnOrdersOrderItem> TrnOrdersOrderItems { get; set; }

    public virtual DbSet<TrnUserRoleMapping> TrnUserRoleMappings { get; set; }

    public virtual DbSet<TrnUsersDeliveryAddress> TrnUsersDeliveryAddresses { get; set; }

    public virtual DbSet<VwUserInfo> VwUserInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MstLocation>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__mst_Loca__E7FEA4775ECA08C6");

            entity.ToTable("mst_Location");

            entity.Property(e => e.LocationId)
                .ValueGeneratedNever()
                .HasColumnName("LocationID");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.ParentLocationId).HasColumnName("ParentLocationID");

            entity.HasOne(d => d.ParentLocation).WithMany(p => p.InverseParentLocation)
                .HasForeignKey(d => d.ParentLocationId)
                .HasConstraintName("FK_Location_ParentLocation");
        });

        modelBuilder.Entity<MstProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__mst_Prod__B40CC6ED7F3CD116");

            entity.ToTable("mst_Products");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Mrprice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MstUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__mst_User__1788CCAC7B3EF138");

            entity.ToTable("mst_Users");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.LoginPcno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LoginPCNo");
            entity.Property(e => e.Otp)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("OTP");
            entity.Property(e => e.OtpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("OTPDateTime");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
            entity.Property(e => e.ResetLink)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ResetLinkExpiration).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MstUserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__mst_User__8AFACE3A81C2A62C");

            entity.ToTable("mst_UserRoles");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TrnAddToCart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__trn_AddT__51BCD79735114AE0");

            entity.ToTable("trn_AddToCart");

            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.AddedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Product).WithMany(p => p.TrnAddToCarts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__trn_AddTo__Produ__47DBAE45");

            entity.HasOne(d => d.User).WithMany(p => p.TrnAddToCarts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__trn_AddTo__UserI__46E78A0C");
        });

        modelBuilder.Entity<TrnOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__trn_Orde__C3905BAF3E728CB8");

            entity.ToTable("trn_Orders");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.TrnOrders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__trn_Order__UserI__403A8C7D");
        });

        modelBuilder.Entity<TrnOrdersOrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__trn_Orde__57ED06A1345F112E");

            entity.ToTable("trn_Orders_OrderItems");

            entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Order).WithMany(p => p.TrnOrdersOrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__trn_Order__Order__4316F928");

            entity.HasOne(d => d.Product).WithMany(p => p.TrnOrdersOrderItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__trn_Order__Produ__440B1D61");
        });

        modelBuilder.Entity<TrnUserRoleMapping>(entity =>
        {
            entity.HasKey(e => e.MappingId).HasName("PK__trn_User__8B5781BDFA61797A");

            entity.ToTable("trn_UserRoleMapping");

            entity.Property(e => e.MappingId).HasColumnName("MappingID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Role).WithMany(p => p.TrnUserRoleMappings)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__trn_UserR__RoleI__3B75D760");

            entity.HasOne(d => d.User).WithMany(p => p.TrnUserRoleMappings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__trn_UserR__UserI__3A81B327");
        });

        modelBuilder.Entity<TrnUsersDeliveryAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__trn_User__091C2A1B43DD515B");

            entity.ToTable("trn_Users_DeliveryAddresses");

            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.City)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Zip)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ZIP");

            entity.HasOne(d => d.User).WithMany(p => p.TrnUsersDeliveryAddresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserID");
        });

        modelBuilder.Entity<VwUserInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_UserInfo");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
