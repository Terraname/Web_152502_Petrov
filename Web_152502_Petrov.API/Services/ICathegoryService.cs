//using Web_152502_Petrov.Domain.Entities;
//using Web_152502_Petrov.Domain.Models;

//namespace Web_152502_Petrov.API.Services
//{
//    public class ICathegoryService
//    {
//    }
//}
﻿using Web_152502_Petrov.Domain.Entities;
using Web_152502_Petrov.Domain.Models;

namespace Web_152502_Petrov.API.Services;

public interface ICathegoryService
{
    public Task<ResponseData<List<Cathegory>>> GetCathegoryListAsync();
    public Task<ResponseData<Cathegory>> GetCathegoryByIdAsync(int id);
    public Task UpdateCathegoryAsync(int id, Cathegory cathegory);
    public Task DeleteCathegoryAsync(int id);
    public Task<ResponseData<Cathegory>> CreateCathegoryAsync(Cathegory cathegory);
    public Task<ResponseData<string>> SaveCathegoryAsync(int id, IFormFile formFile);
}