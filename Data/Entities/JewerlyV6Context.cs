using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

public partial class JewerlyV6Context : DbContext
{
    public JewerlyV6Context()
    {
    }

    public JewerlyV6Context(DbContextOptions<JewerlyV6Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Cashier> Cashiers { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Gem> Gems { get; set; }

    public virtual DbSet<Gold> Golds { get; set; }

    public virtual DbSet<OldProduct> OldProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductBill> ProductBills { get; set; }

    public virtual DbSet<ProductGem> ProductGems { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    public virtual DbSet<Warranty> Warranties { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Jewerly_v6;User ID=sa;Password=12345;Trust Server Certificate=True", x => x.UseNetTopologySuite());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PK__Bill__11F2FC6A0EEBB711");

            entity.ToTable("Bill");

            entity.Property(e => e.BillId)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CashierId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CashierID");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PublishDay).HasColumnType("datetime");
            entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.VoucherVoucherId)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Cashier).WithMany(p => p.Bills)
                .HasForeignKey(d => d.CashierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cashier_Bill");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bills)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Bill_Customer");

            entity.HasOne(d => d.VoucherVoucher).WithMany(p => p.Bills)
                .HasForeignKey(d => d.VoucherVoucherId)
                .HasConstraintName("FKBill513577");
        });

        modelBuilder.Entity<Cashier>(entity =>
        {
            entity.HasKey(e => e.CashId).HasName("PK__Cashier__6B801A6BA3FF51A3");

            entity.ToTable("Cashier");

            entity.Property(e => e.CashId)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.EndCash).HasColumnType("datetime");
            entity.Property(e => e.Income).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.StartCash).HasColumnType("datetime");
            entity.Property(e => e.Status).HasDefaultValue(1);
            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Cashiers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKCash_User");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8B594E3F9");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Phone, "UQ__Customer__5C7E359E605DE34D").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Customer__A9D1053497710982").IsUnique();

            entity.Property(e => e.CustomerId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CustomerID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Rate)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("PK__Discount__E43F6D9615B7584D");

            entity.ToTable("Discount");

            entity.Property(e => e.DiscountId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Discounts)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKDiscount651255");

            entity.HasMany(d => d.ProductProducts).WithMany(p => p.DiscountDiscounts)
                .UsingEntity<Dictionary<string, object>>(
                    "DiscountProduct",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductProductId")
                        .HasConstraintName("FKDiscount_P804639"),
                    l => l.HasOne<Discount>().WithMany()
                        .HasForeignKey("DiscountDiscountId")
                        .HasConstraintName("FKDiscount_P628904"),
                    j =>
                    {
                        j.HasKey("DiscountDiscountId", "ProductProductId").HasName("PK__Discount__77CD65F20E017925");
                        j.ToTable("Discount_Product");
                        j.IndexerProperty<string>("DiscountDiscountId")
                            .HasMaxLength(100)
                            .IsUnicode(false);
                        j.IndexerProperty<string>("ProductProductId")
                            .HasMaxLength(50)
                            .IsUnicode(false)
                            .HasColumnName("ProductProductID");
                    });
        });

        modelBuilder.Entity<Gem>(entity =>
        {
            entity.HasKey(e => e.GemId).HasName("PK__Gem__F101D5808D1C1743");

            entity.ToTable("Gem");

            entity.Property(e => e.GemId)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Desc).HasColumnType("text");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Rate).HasColumnName("rate");
        });

        modelBuilder.Entity<Gold>(entity =>
        {
            entity.HasKey(e => e.GoldId).HasName("PK__Gold__5CD52C53446DB5EB");

            entity.ToTable("Gold");

            entity.Property(e => e.GoldId)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.GoldName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.GoldPercent)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("GoldPErcent");
            entity.Property(e => e.Kara)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.SalePrice).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.WorldPrice).HasColumnType("decimal(10, 3)");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.Golds)
                .HasForeignKey(d => d.ModifiedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Gold_User");
        });

        modelBuilder.Entity<OldProduct>(entity =>
        {
            entity.HasKey(e => e.OproductId).HasName("PK__OldProdu__167AC33B07124BF8");

            entity.ToTable("OldProduct");

            entity.Property(e => e.OproductId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("OProductID");
            entity.Property(e => e.BillBillId)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Desc)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProductProductId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ProductProductID");

            entity.HasOne(d => d.BillBill).WithMany(p => p.OldProducts)
                .HasForeignKey(d => d.BillBillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Oldproduct_Bill");

            entity.HasOne(d => d.ProductProduct).WithMany(p => p.OldProducts)
                .HasForeignKey(d => d.ProductProductId)
                .HasConstraintName("FKOldProduct499685");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6ED0485C739");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ProductID");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Desc).HasColumnType("text");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MarkupRate).HasDefaultValue(1f);
            entity.Property(e => e.Material)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.MaterialNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Material)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Gold");
        });

        modelBuilder.Entity<ProductBill>(entity =>
        {
            entity.HasKey(e => new { e.ProductProductId, e.BillBillId }).HasName("PK__Product___275EBE856603BFB2");

            entity.ToTable("Product_Bill");

            entity.Property(e => e.ProductProductId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ProductProductID");
            entity.Property(e => e.BillBillId)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.BillBill).WithMany(p => p.ProductBills)
                .HasForeignKey(d => d.BillBillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKProduct_Bi56046");

            entity.HasOne(d => d.ProductProduct).WithMany(p => p.ProductBills)
                .HasForeignKey(d => d.ProductProductId)
                .HasConstraintName("FKProduct_Bi592576");
        });

        modelBuilder.Entity<ProductGem>(entity =>
        {
            entity.HasKey(e => new { e.ProductProductId, e.GemGemId }).HasName("PK__Product___C3A826368213BCF7");

            entity.ToTable("Product_Gem");

            entity.Property(e => e.ProductProductId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ProductProductID");
            entity.Property(e => e.GemGemId)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.GemGem).WithMany(p => p.ProductGems)
                .HasForeignKey(d => d.GemGemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKProduct_Ge596162");

            entity.HasOne(d => d.ProductProduct).WithMany(p => p.ProductGems)
                .HasForeignKey(d => d.ProductProductId)
                .HasConstraintName("FKProduct_Ge731455");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC780CA7BF");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ__User__536C85E4A212F767").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__User__5C7E359EC370652B").IsUnique();

            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("UserID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.VoucherId).HasName("PK__Voucher__3AEE7921BAC48A66");

            entity.ToTable("Voucher");

            entity.Property(e => e.VoucherId)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CustomerCustomerId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CustomerCustomerID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKVoucher301107");

            entity.HasOne(d => d.CustomerCustomer).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.CustomerCustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKVoucher633196");
        });

        modelBuilder.Entity<Warranty>(entity =>
        {
            entity.HasKey(e => e.WarrantyId).HasName("PK__Warranty__2ED318F36C5BA3A4");

            entity.ToTable("Warranty");

            entity.Property(e => e.WarrantyId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("WarrantyID");
            entity.Property(e => e.CustomerCustomerId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CustomerCustomerID");
            entity.Property(e => e.Desc)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ProductID");

            entity.HasOne(d => d.CustomerCustomer).WithMany(p => p.Warranties)
                .HasForeignKey(d => d.CustomerCustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKWarranty22608");

            entity.HasOne(d => d.Product).WithMany(p => p.Warranties)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FKProduct700894");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
