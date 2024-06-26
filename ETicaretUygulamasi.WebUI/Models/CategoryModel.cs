﻿using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaretUygulamasi.WebUI.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Kategori için 5-100 arasında bir değer giriniz.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Url adı zorunludur.")]
        public string Url { get; set; }

        public List<Product> Products { get; set; }
    }
}
