//using Microsoft.EntityFrameworkCore;
//using Web_152502_Petrov.API.Data;
//using Web_152502_Petrov.API.Services;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//var connStr = builder.Configuration.GetConnectionString("Default");
//var dataDirectory = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;
////connStr = string.Format(connStr!, dataDirectory);

//builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connStr));
////builder.Services.AddScoped<, PictureService>();
//builder.Services.AddScoped<ICathegoryService, CathegoryService>();
//builder.Services.AddScoped<IDrugService, DrugService>();
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

////builder.Services.addControllers();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}



//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();
//await DbInitializer.SpeedData(app);
//app.Run();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Web_152502_Petrov.API.Data;
//using Web_153501_Brykulskii.API.Data;
using Web_152502_Petrov.API.Services;

namespace Web_152502_Petrov.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connStr = builder.Configuration.GetConnectionString("Default");
        var dataDirectory = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;
        connStr = string.Format(connStr!, dataDirectory);

        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connStr));
        builder.Services.AddScoped<IDrugService, DrugService>();
        builder.Services.AddScoped<ICathegoryService, CathegoryService>();

        builder.Services.AddControllers();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSwaggerGen();

        builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Authority = builder
        .Configuration
        .GetSection("isUri").Value;
        opt.TokenValidationParameters.ValidateAudience = false;
        opt.TokenValidationParameters.ValidTypes =
        new[] { "at+jwt" };
    });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        //app.UseAuthentication();
        //app.UseAuthorization();

        app.MapControllers();

        await DbInitializer.SeedDataAsync(app);

        app.Run();
    }
}