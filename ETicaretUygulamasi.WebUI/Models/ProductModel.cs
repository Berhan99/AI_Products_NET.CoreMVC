using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaretUygulamasi.WebUI.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        //[Display(Name = "Name", Prompt = "Enter Product Name")]
        //[Required(ErrorMessage = "Name zorunlu bit alan.")]
        //[StringLength(20, MinimumLength = 5, ErrorMessage = "Name alanı 5-20 karakter arasında olmalıdır.")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "Price zorunlu bit alan.")]
        //[Range(1,10000,ErrorMessage ="Fiyat için 1-10000 arasında değer giriniz.")]
        public double? Price { get; set; }

        [Required(ErrorMessage = "Description zorunlu bit alan.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name alanı 5-100 karakter arasında olmalıdır.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ImageUrl zorunlu bit alan.")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Url zorunlu bit alan.")]
        public string Url { get; set; }

        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
        public List<Category> Categories { get; set; }

    }
}
