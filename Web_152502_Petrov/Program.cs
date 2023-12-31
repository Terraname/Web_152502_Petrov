//using Web_152502_Petrov;
//using Web_152502_Petrov.Domain.Entities;
//using Web_152502_Petrov.Models;
//using Web_152502_Petrov.Services.CathegoryService;
//using Web_152502_Petrov.Services.DrugService;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Web_152502_Petrov.Data;

//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<Web_152502_PetrovContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("Web_152502_PetrovContext") ?? throw new InvalidOperationException("Connection string 'Web_152502_PetrovContext' not found.")));

//// Add services to the container.
//builder.Services.AddControllersWithViews();
//builder.Services.AddScoped<ICathegoryService, MemoryCathegoryService>();
//builder.Services.AddScoped<IDrugService, MemoryDrugService>();

//UriData.ApiUri = builder.Configuration.GetSection("UriData")[key: "ApiUri"]!;
//builder.Services.AddHttpClient<IDrugService, ApiDrugService>(client =>
//    client.BaseAddress = new Uri(UriData.ApiUri));

////builder.Services.AddHttpClient<IPictureGenreService, ApiPictureGenreService>(client =>
////    client.BaseAddress = new Uri(UriData.ApiUri));

//var app = builder.Build();
////builder.Services.AddRazorPages();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();
//Web_152502_Petrov
using Web_152502_Petrov.Models;
using Web_152502_Petrov.Services.CathegoryService;
using Web_152502_Petrov.Services.DrugService;

namespace Web_153501_Brykulskii;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        UriData.ApiUri = builder.Configuration.GetSection("UriData")[key: "ApiUri"]!;

        builder.Services.AddHttpClient<IDrugService, ApiDrugService>(client =>
            client.BaseAddress = new Uri(UriData.ApiUri));

        builder.Services.AddScoped<ICathegoryService, MemoryCathegoryService>();
        //builder.Services.AddHttpClient<ICathegoryService, ApiCathegoryService>(client =>
        //  client.BaseAddress = new Uri(UriData.ApiUri));
                           
        builder.Services.AddHttpContextAccessor();                          

        builder.Services.AddAuthentication(opt =>
        {                
            opt.DefaultScheme = "cookie";
            opt.DefaultChallengeScheme = "oidc";
        })
            .AddCookie("cookie")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority =
                builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
                options.ClientId =
                builder.Configuration["InteractiveServiceSettings:ClientId"];
                options.ClientSecret =
                builder.Configuration["InteractiveServiceSettings:ClientSecret"];
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ResponseType = "code";
                options.ResponseMode = "query";
                options.SaveTokens = true;
            });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages().RequireAuthorization();

        app.Run();
    }
    //public static void Main(string[] args)
    //{
    //    var builder = WebApplication.CreateBuilder(args);

    //    // Add services to the container.
    //    builder.Services.AddControllersWithViews();
    //    builder.Services.AddRazorPages();
    //    //builder.Services.AddScoped<IPictureGenreService, MemoryPictureGenreService>();
    //    //builder.Services.AddScoped<IPictureService, MemoryPictureService>();

    //    UriData.ApiUri = builder.Configuration.GetSection("UriData")[key: "ApiUri"]!;

    //    builder.Services.AddHttpClient<IDrugService, ApiDrugService>(client =>
    //        client.BaseAddress = new Uri(UriData.ApiUri));

    //    builder.Services.AddHttpClient<ICathegoryService, ApiCathegoryService>(client =>
    //        client.BaseAddress = new Uri(UriData.ApiUri));

    //    // to delete
    //    //var connectionString = "Data Source=app.db";
    //    //string dataDirectory = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;
    //    //connectionString = string.Format(connectionString!, dataDirectory);
    //    //builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString).EnableSensitiveDataLogging());

    //    var app = builder.Build();

    //    // Configure the HTTP request pipeline.
    //    if (!app.Environment.IsDevelopment())
    //    {
    //        app.UseExceptionHandler("/Home/Error");
    //        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //        app.UseHsts();
    //    }

    //    app.UseHttpsRedirection();
    //    app.UseStaticFiles();

    //    app.UseRouting();

    //    app.UseAuthorization();

    //    app.MapControllerRoute(
    //        name: "default",
    //        pattern: "{controller=Home}/{action=Index}/{id?}");
    //    app.MapRazorPages();

    //    app.Run();
    //}
}