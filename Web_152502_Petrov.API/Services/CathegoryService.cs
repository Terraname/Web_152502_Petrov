//using Web_152502_Petrov.API.Data;
//using Web_152502_Petrov.API.Services;
//using Web_152502_Petrov.Domain.Models;

//namespace Web_152502_Petrov.API.Services
//{
//    public class CathegoryService
//    {
//    }
//}
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Web_152502_Petrov.API.Data;
using Web_152502_Petrov.Domain.Entities;
using Web_152502_Petrov.Domain.Models;

namespace Web_152502_Petrov.API.Services;

public class CathegoryService : ICathegoryService
{
    private readonly AppDbContext _context;
    //private readonly IHttpContextAccessor _httpContextAccessor;
    //private readonly IWebHostEnvironment _webHostEnvironment;

    public CathegoryService(AppDbContext context)
    {
        _context = context;
        //_httpContextAccessor = httpContextAccessor;
        //_webHostEnvironment = webHostEnvironment;
    }

    public Task<ResponseData<List<Cathegory>>> GetCathegoryListAsync()
    {
        var cathegories = _context.Cathegories.ToList();

        if (cathegories == null)
        {
            return Task.FromResult(new ResponseData<List<Cathegory>>
            {
                Success = false,
                ErrorMessage = "Genres not found"
            });
        }

        var result = new ResponseData<List<Cathegory>>
        {
            Data = cathegories,
            Success = true,
        };

        return Task.FromResult(result);
    }

    public async Task<ResponseData<Cathegory>> CreateCathegoryAsync(Cathegory cathegory)
    {
        await _context.Cathegories.AddAsync(cathegory);
        await _context.SaveChangesAsync();

        return new ResponseData<Cathegory>()
        {
            Data = cathegory,
            Success = true
        };
    }

    public async Task DeleteCathegoryAsync(int id)
    {
        var cathegory = await _context.Cathegories.FindAsync(id);
        if (cathegory != null)
        {
            _context.Cathegories.Remove(cathegory);
            await _context.SaveChangesAsync();
        }
        else
            throw new ArgumentException("Cathegory with such id not found");
    }

    public async Task<ResponseData<Cathegory>> GetCathegoryByIdAsync(int id)
    {
        var cathegory = await _context.Cathegories.FindAsync(id);
        if (cathegory != null)
        {
            return new ResponseData<Cathegory>()
            {
                Data = cathegory,
                Success = true
            };
        }
        else
            return new ResponseData<Cathegory>()
            {
                Data = null,
                Success = false,
                ErrorMessage = "Cathegory with such id not found"
            };
    }

    public async Task<ResponseData<string>> SaveCathegoryAsync(int id, IFormFile formFile)
    {
        var responseData = new ResponseData<string>();
        var cathegory = await _context.Cathegories.FindAsync(id);

        if (cathegory == null)
        {
            responseData.Success = false;
            responseData.ErrorMessage = "No item found";
            return responseData;
        }

        //var host = "https://" + _httpContextAccessor.HttpContext?.Request.Host;
        //var imageFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

        //if (formFile != null)
        //{
        //    //if (!string.IsNullOrEmpty(cathegory.Image))
        //    //{
        //    //    var prevImage = Path.GetFileName(cathegory.Image);
        //    //    var prevImagePath = Path.Combine(imageFolder, prevImage);

        //    //    if (File.Exists(prevImagePath))
        //    //        File.Delete(prevImagePath);
        //    //}



        //    var ext = Path.GetExtension(formFile.FileName);
        //    var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
        //    var filePath = Path.Combine(imageFolder, fName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await formFile.CopyToAsync(stream);
        //    }

        //    //cathegory.Image = $"{host}/Images/{fName}";
        //    //drug.ImageMimeType = formFile.ContentType;

        //    _context.Entry(cathegory).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //}

        //responseData.Data = drug.Image;
        return responseData;
    }


    public async Task UpdateCathegoryAsync(int id, Cathegory cathegory)
    {

        var cathegoryToUpdate = await _context.Cathegories.FindAsync(id);
        if (cathegoryToUpdate != null)
        {
            cathegoryToUpdate.Name = cathegory.Name;
            cathegoryToUpdate.NormalizedName = cathegory.NormalizedName;
            //drugToUpdate.Price = cathegory.Price;
            //pictureToUpdate.Author = drug.Author;
            //drugToUpdate.Cathegory = cathegory.Cathegory;
            //drugToUpdate.GenreId = drug.GenreId;
            await _context.SaveChangesAsync();
        }
        else
            throw new ArgumentException("Cathegory with such id not found");
    }
}
