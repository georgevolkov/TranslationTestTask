using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TranslateAppDAL;
using TranslateAppDAL.Repositories;
using TranslateAppDAL.Repositories.Interfaces;
using TranslateApplication.Automapper;
using TranslateApplication.Models.Translation;

namespace TranslateApplication
{
   public class Startup
   {
      private const string ConnectionStringName = "TranslatorConnectionString";
      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         services.Configure<CookiePolicyOptions>(options =>
         {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
         });

         services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

         services.AddAutoMapper(typeof(TranslationProfile));
         services.AddDbContext<TranslateDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString(ConnectionStringName)), ServiceLifetime.Transient);
         services.AddTransient(prv =>
         {
            var context = prv.GetService<TranslateDbContext>();
            return new TranslateWordProcessor(context);
         });

         services.AddTransient<ITranslationRepository>(prv =>
         {
            var context = prv.GetService<TranslateDbContext>();
            return new TranslationRepository(context);
         });

         services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
               options.LoginPath = new PathString("/Account/Login");
               options.AccessDeniedPath = new PathString("/Account/Login");
            });
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
         else
         {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
         }

         app.UseHttpsRedirection();
         app.UseStaticFiles();
         app.UseAuthentication();

         app.UseMvc(routes =>
         {
            routes.MapRoute(
                   name: "default",
                   template: "{controller=Home}/{action=Index}/{id?}");
         });
      }
   }
}
