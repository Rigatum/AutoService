using AutoService.Interfaces;
using AutoService.Repository;
using AutoService.Services;

namespace AutoService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IRepairRepository>(provider =>
                new RepairRepository(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IRepairService, RepairService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Repairs}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
