using Web_152502_Petrov.Services.CathegoryService;
using Web_152502_Petrov.Domain.Entities;
using Web_152502_Petrov.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using Web_152502_Petrov.Models;

namespace Web_152502_Petrov.Services.DrugService
{
    public class MemoryDrugService : IDrugService
    {
        List<Drug> _dishes;
        List<Cathegory> _cathegories;
        IConfiguration _config;
        
        /*
         * public MemoryProductService(
[FromServices] IConfiguration config,
ICategoryService categoryService,
int pageNo)
        */
        public MemoryDrugService(
[FromServices] IConfiguration config,
ICathegoryService cathegoryService)
        {
            _config = config;
            _cathegories = cathegoryService.GetCathegoryListAsync()
            .Result
            .Data;
            SetupData();
            //_cathegories.Find(c => c.NormalizedName.Equals("soups"));
        }
        /*public MemoryDrugService(ICathegoryService cathegoryService)
        {S
            _cathegories = cathegoryService.GetCathegoryListAsync()
            .Result
            .Data;
            SetupData();
            //_cathegories.Find(c => c.NormalizedName.Equals("soups"));
        }*/

        public Task<ResponseData<Drug>> CreateDrugAsync(Drug product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDrugAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Drug>>> GetDrugListAsync(string?
        cathegoryNormalizedName, int pageNo = 1)
        {
            var itemsOnPage = int.Parse(_config.GetSection("ItemsPerPage").Value);
            if (_dishes is null)
            {
                SetupData();
            }
            var result = new ResponseData<ListModel<Drug>>();
            ListModel<Drug> drugs = new ListModel<Drug>();
            //drugs.Items = _dishes.Where(d => cathegoryNormalizedName == null ||
            //d.Cathegory.NormalizedName != null)
            //.ToList();
            var drugs_results = _dishes.Where(d => cathegoryNormalizedName == null ||
            d.Cathegory.NormalizedName.Equals(cathegoryNormalizedName));
            int numberOfPages = drugs_results.Count() / itemsOnPage + (drugs_results.Count() % itemsOnPage == 0 ? 0 : 1);
            var items = drugs_results
                .Skip((pageNo - 1) * itemsOnPage)
                .Take(itemsOnPage)
                .ToList();
            result.Data = drugs;
            return Task.FromResult(new ResponseData<ListModel<Drug>>
            {
                Success = true,
                Data = new ListModel<Drug>
                {
                    Items = items,
                    CurrentPage = pageNo,
                    TotalPages = numberOfPages
                },
                ErrorMessage = string.Empty
            }
            );
        }

        public Task<ResponseData<Drug>> GetDrugByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDrugAsync(int id, Drug product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Инициализация списков
        /// </summary>
        private void SetupData()
        {
            _dishes = new List<Drug>
            {
                new Drug {Id = 1, Name="AntibioticName",
                Description="kills everythin, very dangerous",
                Price =200, Image="Images/Суп.jpg",
                Cathegory=
                _cathegories.Find(c=>c.NormalizedName.Equals("antibiotics"))},
                new Drug { Id = 2, Name="Борщ",
                Description="Много сала, без сметаны",
                Price =330, Image="Images/Борщ.jpg",
                Cathegory=
                _cathegories.Find(c => c.NormalizedName.Equals("antibiotics"))},
                new Drug { Id = 2, Name="Med5",
                Description="Много сала, без сметаны",
                Price =330, Image="Images/Борщ.jpg",
                Cathegory=
                _cathegories.Find(c => c.NormalizedName.Equals("anti-alergy"))},
                new Drug { Id = 2, Name="AntialergyName",
                Description="Много сала, без сметаны",
                Price =330, Image="Images/Борщ.jpg",
                Cathegory=
                _cathegories.Find(c => c.NormalizedName.Equals("anti-alergy"))},
                new Drug { Id = 2, Name="gome",
                Description="kills 50% of bacteria",
                Price =330, Image="Images/Борщ.jpg",
                Cathegory=
                _cathegories.Find(c => c.NormalizedName.Equals("antibiotics"))},
            };
        }

    }
}
