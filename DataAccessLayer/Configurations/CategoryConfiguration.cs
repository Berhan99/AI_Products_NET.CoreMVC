using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(m => m.CategoryId); /*primary key belirleme*/
            builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
            
            builder.HasData(
            new Category { CategoryId=1,Name = "Telefon", Url = "telefon" },
            new Category { CategoryId = 2, Name = "Bilgisayar", Url = "bilgisayar" },
            new Category { CategoryId = 3, Name = "Elektronik", Url = "elektronik" },
            new Category { CategoryId = 4, Name = "Beyaz Eşya", Url = "beyaz-esya" });

        }
    }
}
