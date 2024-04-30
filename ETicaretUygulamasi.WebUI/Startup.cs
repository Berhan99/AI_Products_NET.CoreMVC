using Business.Abstract;
using Business.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EfCore;
using ETicaretUygulamasi.WebUI.EmailServices;
using ETicaretUygulamasi.WebUI.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaretUygulamasi.WebUI
{
    public class Startup
    {        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;//bu configuratýon uzerýnden app settýngsteký býlgilere ulaþabýlýrýz
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));
            services.AddDbContext<ETicaretUygulamasiContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
            
            services.Configure<IdentityOptions>(options =>
            {
                //password
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;

                //lockout

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                //options.User.AllowedUserNameCharacters = "";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;


            });

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".ETicaretUygulamasi.Security.Cookie"
                };

            });

            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            //product ile ilgili olarak "IProductRepository" cagrýldýgýnda bana "EfCoreProductRepository" yi ver demek.
            //"IProductService" cagrýldýgýnda "ProductManager"'ý ver demek

            // DATA ACCESS LAYER

            // Bu asaðýdaki dataacceslayer classlarý business'e UnitOfWork üzerinden gidecek
            //services.AddScoped<IProductRepository, EfCoreProductRepository>();
            //services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
            //services.AddScoped<ICartRepository, EfCoreCartRepository>();
            //services.AddScoped<IOrderRepository, EfCoreOrderRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //BUSINESS LAYER
            services.AddScoped<IProductService, ProductManager>();                        
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<ICartService, CartManager>();
            services.AddScoped<IOrderService, OrderManager>();


            services.AddScoped<IEmailSender, SmtpEmailSender>(i=>
                new SmtpEmailSender(
                    Configuration["EmailSender:Host"],
                    Configuration.GetValue<int>("EmailSender:Port"),
                    Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                    Configuration["EmailSender:UserName"],
                    Configuration["EmailSender:Password"])
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IConfiguration configuration,UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ICartService cartService)
        {
            if (env.IsDevelopment())//burasý true donuyosa demekký uygulama geliþtirme aþamasýndayýz debug mode true gýbý
            {
                app.UseDeveloperExceptionPage();                
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            SeedIdentity.Seed(userManager,roleManager,cartService,configuration).Wait();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                   name: "orders",
                   pattern: "orders",
                   defaults: new { controller = "Order", action = "GetOrders" }
                   );

                endpoints.MapControllerRoute(
                   name: "checkout",
                   pattern: "checkout",
                   defaults: new { controller = "Cart", action = "Checkout" }
                   );

                endpoints.MapControllerRoute(
                    name: "cart",
                    pattern: "cart",
                    defaults: new { controller = "Cart", action = "Index" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminuseredit",
                    pattern: "admin/user/{id?}",
                    defaults: new { controller = "admin", action = "useredit" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminusers",
                    pattern: "admin/user/list",
                    defaults: new { controller = "admin", action = "userlist" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminroles",
                    pattern: "admin/role/list",
                    defaults: new { controller = "admin", action = "rolelist" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminrolecreate",
                    pattern: "admin/role/create",
                    defaults: new { controller = "admin", action = "rolecreate" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminproducts",
                    pattern: "admin/products",
                    defaults: new { controller = "admin", action = "productlist" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminproductcreate",
                    pattern: "admin/products/create",
                    defaults: new { controller = "admin", action = "createproduct" }
                    );

                endpoints.MapControllerRoute(
                  name: "adminproductedit",
                  pattern: "admin/products/{id}",
                  defaults: new { controller = "admin", action = "editproduct" }
                  );

                endpoints.MapControllerRoute(
                  name: "adminroleedit",
                  pattern: "admin/role/{id}",
                  defaults: new { controller = "admin", action = "roleedit" }
                  );

                endpoints.MapControllerRoute(
                    name: "admincategories",
                    pattern: "admin/categories",
                    defaults: new { controller = "admin", action = "categorylist" }
                    );

                endpoints.MapControllerRoute(
                    name: "admincategorycreate",
                    pattern: "admin/categories/create",
                    defaults: new { controller = "admin", action = "createcategory" }
                    );

                endpoints.MapControllerRoute(
                 name: "admincategoryedit",
                 pattern: "admin/categories/{id}",
                 defaults: new { controller = "admin", action = "editcategory" }
                 );


                endpoints.MapControllerRoute(
                    name: "search",
                    pattern: "search",
                    defaults: new { controller = "Shop", action = "Search" }
                    );

                endpoints.MapControllerRoute(
                    name: "productdetails",
                    pattern: "{url}",
                    defaults: new { controller = "Shop", action = "details" }
                    );

                endpoints.MapControllerRoute(
                    name: "products",
                    pattern:"products/{category?}",
                    defaults: new {controller="Shop",action="list"}
                    ) ;

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}",
                    defaults: new { controller = "home", action = "index" }
                    );

            });
        }
    }
}
