//using ETicaretUygulamasi.WebUI.Data;
using Business.Abstract;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaretUygulamasi.WebUI.ViewComponents
{
    public class CategoriesViewComponent: ViewComponent
    {
        private ICategoryService _categoryService;
        public CategoriesViewComponent(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (RouteData.Values["category"]!=null)
            {
                ViewBag.SelectedCategory = RouteData?.Values["category"];
            }
            return View(await _categoryService.GetAll());
        }
    }
}
