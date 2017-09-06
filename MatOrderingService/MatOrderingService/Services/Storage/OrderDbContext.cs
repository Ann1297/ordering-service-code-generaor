using MatOrderingService.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatOrderingService.Services.Storage
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MapOrder(modelBuilder.Entity<Order>());
            MapProduct(modelBuilder.Entity<Product>());
            MapOrderItems(modelBuilder.Entity<OrderItem>());
        }

        private void MapOrderItems(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.Property(p => p.Id)
                .IsRequired();

            builder.Property(p => p.Count)                
                .IsRequired();

            builder.Property(p => p.OrderId)
                .IsRequired();

            builder.Property(p => p.ProductId)
                .IsRequired();

            builder.HasKey(p => p.Id);

            builder
                .HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId)
                .HasPrincipalKey(p => p.Id);
        }

        private void MapProduct(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.Property(p => p.Id)
                .IsRequired();

            builder.Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex(p => p.Code)                
                .IsUnique();

            builder.Property(p => p.Code)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasKey(p => p.Id);            
        }

        private void MapOrder(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.Property(p => p.Id)
                .IsRequired();

            builder.HasIndex(p => p.OrderCode)
                .IsUnique();

            builder.Property(p => p.OrderCode)
                .IsRequired();

            builder.Property(p => p.OrderStatus)
                .HasDefaultValue(OrderStatus.New)
                .IsRequired();

            builder.Property(p => p.CreatorId)
                .HasMaxLength(50)
                .IsRequired();
            
            builder.Property(p => p.IsDeleted)
                .IsRequired();

            builder.Property(p => p.CreateDate)
                .ForSqlServerHasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.HasKey(p => p.Id);

            builder
                .HasMany(p => p.OrderItems)
                .WithOne()
                .HasForeignKey(p => p.OrderId)
                .HasPrincipalKey(o => o.Id);
        }
    }
}
