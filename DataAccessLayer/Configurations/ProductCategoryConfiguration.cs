using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configurations
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasKey(x => new { x.CategoryId, x.ProductId });
            builder.HasData(
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
