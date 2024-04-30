using Business.Abstract;
using Business.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace ETicaretUygulamasi.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowOrigins = "_myAllowOrigins";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ETicaretUygulamasiContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));

            //DATA ACCESS LAYER
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //BUSINESS LAYER
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<ICartService, CartManager>();
            services.AddScoped<IOrderService, OrderManager>();

            services.AddControllers();


            // JAVASCRÝPT TARAFINDAN APÝYE ÝSTEK ATILDIGINDA
            // APÝYE ULAÞABÝLCEK ORÝGÝNÝ HEADERI VEYA METHODU BELÝRLEMEK ÝCÝN CORS
            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: "MyAllowOrigins",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();

                    }
                        );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            //ROUTE VE AUTHORÝZATÝON ARASINA YAZILMALI
            app.UseCors(MyAllowOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Varsayýlan olarak çaðrýlacak controller ve action belirlenir.
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=Products}");
            });
        }
    }
}
