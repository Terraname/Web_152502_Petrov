using Web_152502_Petrov.Domain.Entities;
using Web_152502_Petrov.Domain.Models;

namespace Web_152502_Petrov.Services.CathegoryService
{
    public class MemoryCathegoryService : ICathegoryService
    {
        public Task<ResponseData<List<Cathegory>>>
        GetCathegoryListAsync()
        {
            var cathegories = new List<Cathegory>
            { 
                new Cathegory {Id=1, Name="Антибиотики",
                NormalizedName="antibiotics"},
                new Cathegory {Id=2, Name="ПротивоАлиргенные",
                NormalizedName="anti-alergy"},

            };
            var result = new ResponseData<List<Cathegory>>();
            result.Data = cathegories;
            return Task.FromResult(result);
        }
    }
}
