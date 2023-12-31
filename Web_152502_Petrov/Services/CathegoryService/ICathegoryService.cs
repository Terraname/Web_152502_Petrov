using Web_152502_Petrov.Domain.Entities;
using Web_152502_Petrov.Domain.Models;

namespace Web_152502_Petrov.Services.CathegoryService
{
    public interface ICathegoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<Cathegory>>> GetCathegoryListAsync();
    }
}
