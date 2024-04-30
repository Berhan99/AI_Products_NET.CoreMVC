using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            //var context = new ETicaretUygulamasiContext();

            //if (context.Database.GetPendingMigrations().Count() == 0)//butun migrationlar uygulanmıs demektır.
            //{
            //    if (context.Categories.Count() == 0) // veritabanındaki category tablosundakı satır sayısını getiriyor
            //    {
            //        context.Categories.AddRange(Categories);
            //    }

            //    if (context.Products.Count() == 0) // veritabanındaki Products tablosundakı satır sayısını getiriyor
            //    {
            //        context.Products.AddRange(Products);

            //        //productcategory entity içerisinde navıgatıon propertyler (product,category) old ıcın direk yerlerıne eklıyo
            //        context.AddRange(ProductCategories);
            //    }
            //}

            //context.SaveChanges();

        }

        public static Category[] Categories =
        {
            new Category{Name="Telefon",Url="telefon" },
            new Category{Name="Bilgisayar",Url="bilgisayar" },
            new Category{Name="Elektronik",Url="elektronik" },
            new Category{Name="Beyaz Eşya",Url="beyaz-esya" }
        };

        public static Product[] Products =
        {
            new Product{Name="Samsung s5",Price=2000,ImageUrl="img1.jpg",Description="Cok iyi telefon",Url="samsung-s5" ,IsApproved=true },
            new Product{Name="Samsung s6",Price=3500,ImageUrl="img2.jpg",Description="Cok iyi telefon",Url="samsung-s6",IsApproved=true },
            new Product{Name="Samsung s7",Price=5000,ImageUrl="img3.jpg",Description="Cok iyi telefon",Url="samsung-s7",IsApproved=true },
            new Product{Name="Samsung s8",Price=6000,ImageUrl="img4.jpg",Description="Cok iyi telefon",Url="samsung-s8",IsApproved=false },
            new Product{Name="Samsung s9",Price=7000,ImageUrl="img1.jpg",Description="Cok iyi telefon",Url="samsung-s9",IsApproved=false }
        };

        private static ProductCategory[] ProductCategories ={
            new ProductCategory{Product=Products[0],Category=Categories[0] },
            new ProductCategory{Product=Products[0],Category=Categories[2] },
            new ProductCategory{Product=Products[1],Category=Categories[0] },
            new ProductCategory{Product=Products[1],Category=Categories[2] },
            new ProductCategory{Product=Products[2],Category=Categories[0] },
            new ProductCategory{Product=Products[2],Category=Categories[2] },
            new ProductCategory{Product=Products[3],Category=Categories[0] },
            new ProductCategory{Product=Products[3],Category=Categories[2] }
        };
    }
}


