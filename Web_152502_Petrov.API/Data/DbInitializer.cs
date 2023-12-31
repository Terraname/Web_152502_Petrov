using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Web_152502_Petrov.Domain.Entities;

namespace Web_152502_Petrov.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedDataAsync(WebApplication app)
        {

            var cathegories = new List<Cathegory>
            {
                new Cathegory {Name="Антибиотики",
                NormalizedName="antibiotics"},
                new Cathegory {Name="ПротивоАлиргенные",
                NormalizedName="anti-alergy"},

            };

            var antibiotics = cathegories[0];
            var antialergy = cathegories[1];

            var drugs = new List<Drug>
            {
                new Drug {Name="AntibioticName",
                Description="kills everythin, very dangerous",
                Price =200, Image="Images/Суп.jpg",
                Cathegory=
                antibiotics},
                new Drug { Name="Борщ",
                Description="Много сала, без сметаны",
                Price =330, Image="Images/Борщ.jpg",
                Cathegory=
                antibiotics},
                new Drug { Name="Med5",
                Description="Много сала, без сметаны",
                Price =330, Image="Images/Борщ.jpg",
                Cathegory=
                antialergy},
                new Drug { Name="AntialergyName",
                Description="Много сала, без сметаны",
                Price =330, Image="Images/Борщ.jpg",
                Cathegory=
                antialergy},
                new Drug { Name="gome",
                Description="kills 50% of bacteria",
                Price =330, Image="Images/Борщ.jpg",
                Cathegory=
                antibiotics},
            };

            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var Database = context.Database;
            await Database.EnsureDeletedAsync();
            await Database.EnsureCreatedAsync();
            var databaseCreator = (Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator);
            //databaseCreator.CreateTables();

            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.MigrateAsync();
            }

            if (!context.Cathegories.Any())
            {
                await context.Cathegories.AddRangeAsync(cathegories);
                await context.SaveChangesAsync();
            }

            if (!context.Drugs.Any())
            {
                var imagesUrl = app.Configuration.GetSection("DrugsUrl").Value;

                //foreach (var picture in cathegories)
                //{
                //    picture.ImagePath = $"{imagesUrl}{picture.ImagePath}";
                //}

                await context.Drugs.AddRangeAsync(drugs);
                await context.SaveChangesAsync();
            }
        }
    }
}
