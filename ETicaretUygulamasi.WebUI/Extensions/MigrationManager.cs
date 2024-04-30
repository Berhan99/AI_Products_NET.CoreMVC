using DataAccessLayer.Concrete.EfCore;
using ETicaretUygulamasi.WebUI.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaretUygulamasi.WebUI.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>())
                {
                    try
                    {
                        applicationContext.Database.Migrate();
                    }
                    catch (Exception)
                    {
                        //loglama
                        throw;
                    }
                }

                using (var eticaretContext = scope.ServiceProvider.GetRequiredService<ETicaretUygulamasiContext>())
                {
                    try
                    {
                        eticaretContext.Database.Migrate();
                    }
                    catch (Exception)
                    {
                        //loglama
                        throw;
                    }
                }
            }

            return host;
        }
    }
}
