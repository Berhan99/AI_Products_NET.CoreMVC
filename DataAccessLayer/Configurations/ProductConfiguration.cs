using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(m => m.ProductId); /*primary key belirleme*/
            builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
            builder.Property(m => m.DateAdded).HasDefaultValueSql("getdate()"); // sqlde sys_date'i döner: 
            builder.HasData(
            new Product {ProductId = 1 , Name = "Samsung s5", Price = 2000, ImageUrl = "img1.jpg", Description = "Cok iyi telefon", Url = "samsung-s5", IsApproved = true },
            new Product { ProductId = 2,Name = "Samsung s6", Price = 3500, ImageUrl = "img2.jpg", Description = "Cok iyi telefon", Url = "samsung-s6", IsApproved = true },
            new Product { ProductId = 3,Name = "Samsung s7", Price = 5000, ImageUrl = "img3.jpg", Description = "Cok iyi telefon", Url = "samsung-s7", IsApproved = true },
            new Product { ProductId = 4,Name = "Samsung s8", Price = 6000, ImageUrl = "img4.jpg", Description = "Cok iyi telefon", Url = "samsung-s8", IsApproved = false },
            new Product { ProductId = 5,Name = "Samsung s9", Price = 7000, ImageUrl = "img1.jpg", Description = "Cok iyi telefon", Url = "samsung-s9", IsApproved = false }
            );
                
        }
    }
}
