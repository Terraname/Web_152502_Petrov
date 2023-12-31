//using Web_152502_Petrov.Domain.Models;

//namespace Web_152502_Petrov.API.Services
//{
//    public class IDrugService
//    {
//    }
//}

using Web_152502_Petrov.Domain.Entities;
using Web_152502_Petrov.Domain.Models;

namespace Web_152502_Petrov.API.Services;

public interface IDrugService
{
    /// <param name="pageNo">номер страницы списка</param>
    /// <param name="pageSize">количество объектов на странице</param>
    /// <returns></returns>
    public Task<ResponseData<ListModel<Drug>>> GetDrugListAsync(string? cathegoryNormalizedName, int pageNo = 1, int pageSize = 3);
    public Task<ResponseData<Drug>> GetDrugByIdAsync(int id);
    public Task UpdateDrugAsync(int id, Drug drug);
    public Task DeleteDrugAsync(int id);
    public Task<ResponseData<Drug>> CreateDrugAsync(Drug drug);
    public Task<ResponseData<string>> SaveDrugAsync(int id, IFormFile formFile);

    public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
}