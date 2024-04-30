using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configurations
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<Product>().HasKey(m => m.ProductId); /*primary key belirleme*/
            builder.Entity<Product>().Property(m => m.Name).IsRequired().HasMaxLength(100);
            builder.Entity<Product>().Property(m => m.DateAdded).HasDefaultValueSql("getdate()"); // sqlde sys_date'i döner: 
            builder.Entity<Product>().HasData(
            new Product { ProductId = 1, Name = "Samsung s5", Price = 2000, ImageUrl = "img1.jpg", Description = "Cok iyi telefon", Url = "samsung-s5", IsApproved = true },
            new Product { ProductId = 2, Name = "Samsung s6", Price = 3500, ImageUrl = "img2.jpg", Description = "Cok iyi telefon", Url = "samsung-s6", IsApproved = true },
            new Product { ProductId = 3, Name = "Samsung s7", Price = 5000, ImageUrl = "img3.jpg", Description = "Cok iyi telefon", Url = "samsung-s7", IsApproved = true },
            new Product { ProductId = 4, Name = "Samsung s8", Price = 6000, ImageUrl = "img4.jpg", Description = "Cok iyi telefon", Url = "samsung-s8", IsApproved = false },
            new Product { ProductId = 5, Name = "Samsung s9", Price = 7000, ImageUrl = "img1.jpg", Description = "Cok iyi telefon", Url = "samsung-s9", IsApproved = false }
            );


            builder.Entity<Category>().HasKey(m => m.CategoryId); /*primary key belirleme*/
            builder.Entity<Category>().Property(m => m.Name).IsRequired().HasMaxLength(100);
            builder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Telefon", Url = "telefon" },
            new Category { CategoryId = 2, Name = "Bilgisayar", Url = "bilgisayar" },
            new Category { CategoryId = 3, Name = "Elektronik", Url = "elektronik" },
            new Category { CategoryId = 4, Name = "Beyaz Eşya", Url = "beyaz-esya" });



            builder.Entity<ProductCategory>().HasKey(x => new { x.CategoryId, x.ProductId });
            builder.Entity<ProductCategory>().HasData(
                new ProductCategory { ProductId = 1, CategoryId = 1 },
                new ProductCategory { ProductId = 1, CategoryId = 2 },
                new ProductCategory { ProductId = 1, CategoryId = 3 },
                new ProductCategory { ProductId = 2, CategoryId = 1 },
                new ProductCategory { ProductId = 2, CategoryId = 2 },
                new ProductCategory { ProductId = 2, CategoryId = 3 },
                new ProductCategory { ProductId = 3, CategoryId = 4 },
                new ProductCategory { ProductId = 4, CategoryId = 3 },
                new ProductCategory { ProductId = 5, CategoryId = 3 },
                new ProductCategory { ProductId = 5, CategoryId = 1 }
                );
        }
    }
}
