using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicaretUygulamasi.WebUI.Models;
using Business.Abstract;
using Entities;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using ETicaretUygulamasi.WebUI.Extensions;
using Microsoft.AspNetCore.Identity;
using ETicaretUygulamasi.WebUI.Identity;

namespace ETicaretUygulamasi.WebUI.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;

        public AdminController(IProductService productService, ICategoryService categoryService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> RoleEdit(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);

            var members = new List<User>();
            var nonMembers = new List<User>();

            foreach (var user in _userManager.Users.ToList())
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }

            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }

                        }

                    }
                }


                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }

                        }

                    }
                }
            }
            return Redirect("/admin/role/" + model.RoleId);
        }

        public IActionResult UserList()
        {
            return View(_userManager.Users);
        }
        [HttpGet]
        public async  Task<IActionResult> UserEdit(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(i => i.Name);

                ViewBag.Roles = roles;
                return View(new UserDetailsModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = selectedRoles
                });
            }
            return Redirect("~/admin/user/list");
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailsModel model,string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles ?? new string[] { };

                        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>()); //selected roles'de olup userRoles'de olmayanları ekle yanı userRoles'de olanları birdaha ekleme.
                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>()); //user roles'de olup selectedRoles'de olmayanları sil yanı selectedRoles'de olmayan userRoles'leri ekleme cıkar.

                        return Redirect("/admin/user/list");
                    }
                }

                return Redirect("/admin/user/list");

            }

            return View(model);
        }

        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }
        [HttpGet]
        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));

                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }


            return View(model);
        }

        public async Task<IActionResult> ProductList()
        {
            return View(new ProductListViewModels()
            {
                Products = await _productService.GetAll()
            });
        }

        public async Task<IActionResult> CategoryList()
        {
            return View(new CategoryListViewModel()
            {
                Categories = await _categoryService.GetAll()
            });
        }

        // get metodunun gorevı ılgılı sayfayı (viewi) bize getirmek
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        //get metodunun getirdigi bos formu bilgilerle doldurduktan sonra post metoduyla içeri göndericez,
        //Aradaki fark post metodun dolu bir product parametresi alması

        [HttpPost]
        public IActionResult CreateProduct(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    Url = model.Url,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl,
                    Price = model.Price
                };

                if (_productService.Create(entity))
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Info",
                        Message = "Saving Successful",
                        AlertType = "success"
                    });


                    return RedirectToAction("ProductList"); // kullanıcı bılgılerı doldurup submıt ettıkten sonra kullanıcıyı ProductList sayfasına redirect etmek gerekir.
                }

            }
            TempData.Put("message", new AlertMessage()
            {
                Title = _productService.ErrorMessage,
                Message = _productService.ErrorMessage,
                AlertType = "danger"
            });


            return View(model);
        }

        // get metodunun gorevı ılgılı sayfayı (viewi) bize getirmek
        [HttpGet]
        public async Task<IActionResult> EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _productService.GetByIdWithCategories((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
                Price = entity.Price,
                IsApproved = entity.IsApproved,
                IsHome = entity.IsHome,
                Categories = entity.ProductCategories.Select(x => x.Category).ToList()
            };

            ViewBag.Categories = await _categoryService.GetAll();

            return View(model);
        }

        //get metodunun getirdigi bos formu bilgilerle doldurduktan sonra post metoduyla içeri göndericez,
        //Aradaki fark post metodun dolu bir product parametresi alması

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel model, int[] categoryIds, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var entity = await _productService.GetById(model.ProductId);
                if (entity == null)
                {
                    return NotFound();
                }

                entity.Name = model.Name;
                entity.Url = model.Url;
                entity.Description = model.Description;
                entity.ImageUrl = model.ImageUrl;
                entity.Price = model.Price;
                entity.IsHome = model.IsHome;
                entity.IsApproved = model.IsApproved;

                if (file != null)
                {

                    var extension = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{Guid.NewGuid()}{extension}");
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                if (_productService.Update(entity, categoryIds))
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Info",
                        Message = "Record has been updated.",
                        AlertType = "success"
                    });

                    return RedirectToAction("ProductList");
                }
                TempData.Put("message", new AlertMessage()
                {
                    Title = _productService.ErrorMessage,
                    Message = _productService.ErrorMessage,
                    AlertType = "danger"
                });


            }
            ViewBag.Categories = await _categoryService.GetAll();
            return View(model);


        }


        public async  Task<IActionResult> DeleteProduct(int productId)
        {
            var entity =  await _productService.GetById(productId);
            if (entity != null)
            {
                _productService.Delete(entity);
            }

            TempData.Put("message", new AlertMessage()
            {
                Title = "Info",
                Message = $"Product with name of {entity.Name} deleted",
                AlertType = "danger"
            });


            return RedirectToAction("ProductList");
        }


        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new Category()
                {
                    Name = model.Name,
                    Url = model.Url
                };

                _categoryService.Create(entity);


                TempData.Put("message", new AlertMessage()
                {
                    Title = "Info",
                    Message = $"Category with name of {entity.Name} added.",
                    AlertType = "success"
                });


                return RedirectToAction("categorylist");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult EditCategory(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var entity = _categoryService.GetByIdWithProducts((int)Id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new CategoryModel()
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductCategories.Select(x => x.Product).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = await _categoryService.GetById(model.CategoryId);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Url = model.Url;
                _categoryService.Update(entity);

                TempData.Put("message", new AlertMessage()
                {
                    Title = "Info",
                    Message = $"Product with name of {entity.Name} updated.",
                    AlertType = "warning"
                });


                return RedirectToAction("categorylist");
            }
            return View(model);
        }

        public async  Task<IActionResult> DeleteCategory(int categoryId)
        {
            var entity = await _categoryService.GetById(categoryId);
            if (entity == null)
            {
                return NotFound();
            }
            _categoryService.Delete(entity);

            TempData.Put("message", new AlertMessage()
            {
                Title = "Info",
                Message = $"Product with name of {entity.Name} deleted.",
                AlertType = "danger"
            });


            return RedirectToAction("categorylist");
        }

        [HttpPost]
        public IActionResult DeleteFromCategory(int productId, int categoryId)
        {
            _categoryService.DeleteFromCategory(productId, categoryId);
            return Redirect("/admin/categories/" + categoryId);
        }


    }
}
