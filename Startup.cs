using Microsoft.AspNetCore.Authentication.Cookies;
using INTEX2.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using INTEX2.Data;
using Microsoft.AspNetCore.Identity.UI;

namespace INTEX2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })

                .AddCookie(options =>
                {
                    options.LoginPath = "/account/google-login";
                })
                .AddGoogle(options =>
                {
                    options.ClientId = "800903118366-iu0bvkiq52girde4a529lbbsfhiaid2c.apps.googleusercontent.com";
                    options.ClientSecret = "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";
                });


            services.AddControllersWithViews();
            services.AddDbContext<CrashDbContext>(options =>
            {
                options.UseMySql(Configuration["ConnectionStrings:CrashDataDbConnection"]);
            });

            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;

            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddFido(options =>
            {
                options.Licensee = "DEMO";
                options.LicenseKey =
                "eyJTb2xkRm9yIjowLjAsIktleVByZXNldCI6NiwiU2F2ZUtleSI6ZmFsc2UsIkxlZ2FjeUtleSI6ZmFsc2UsIlJlbmV3YWxTZW50VGltZSI6IjAwMDEtMDEtMDFUMDA6MDA6MDAiLCJhdXRoIjoiREVNTyIsImV4cCI6IjIwMjItMDUtMDRUMDE6MDA6MDMuNTMzNDU5OSswMDowMCIsImlhdCI6IjIwMjItMDQtMDRUMDE6MDA6MDMiLCJvcmciOiJERU1PIiwiYXVkIjo2fQ==.fKuw59Ym4xwWB6dSYpfENPLblFROIzjj6P5LehisgGVioN+9H1K6wKdiP5aIHuJgLgVbx02emmSK9E4navvKR4/SxXabY1ebMD8uTzqfzfsPZA8zPONiH6qYwdciSIPpdNOQEbYjdJgRmECyfj3P5pxZjSYaqQoJacKf1ex30ULOXVLopi656kNKB3EIK5Pbvs+nNM97hfbBXLTFsvlAjsMABbQ4gZ4PCFTjTlsQxolze7CYfZTv0JUBmdIsfvQ1KpHvXFCogQbQVIT8sSPUaZjLfJZycnkMK/K9PWvedcmHUDVb7RK39W6O0XWRLjwDJLRwTAUVt0lsQJutO1gSMlbYoLC3L4fU5sUscu0cFhHm39Fe9AnN3ltDu/x0yyjRNzdghSdFC+1xz5Oo1ZBXkEc6PCX47KQ44jWvffUWsf2jLeR9LeUeKEQWoEX/J9gtKPtjxyl0WHo0NDf0CkauaMEQ1nPqH65CmwqeSKSvk1r0w7im2a5JWknM3kt6EPRJ5YIca8k4U2ONiyND0paUBcN+40cQEOJlmplLYS7r8jt6LRTNUrrOtY0EZ1w5A1ZXF5vNFx48wn+r1vURGcqGbqzdTHmePd2hkOFK7h8+vloqbNZ5XvoD+I1bkm+oFqZcRdcE0xW+f1hKAE/QpvFsZPY+M37bXjPo1Hob1llKT2o";
            }).AddInMemoryKeyStore();

            services.AddScoped<ICrashRepository, EFCrashRepository>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapDefaultControllerRoute();

                endpoints.MapRazorPages();

                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/admin/{*catchall}", "/admin/Index");
            });
        }
    }
}
