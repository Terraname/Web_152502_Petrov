//using Microsoft.EntityFrameworkCore;
//using Web_152502_Petrov.API.Data;
//using Web_152502_Petrov.Domain.Models;

//namespace Web_152502_Petrov.API.Services
//{
//    public class DrugService
//    {
//    }
//}

using Microsoft.EntityFrameworkCore;
using Web_152502_Petrov.API.Controllers;
using Web_152502_Petrov.API.Data;
using Web_152502_Petrov.Domain.Entities;
using Web_152502_Petrov.Domain.Models;

namespace Web_152502_Petrov.API.Services;

public class DrugService : IDrugService
{
    private readonly int _maxPageSize = 20;
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    //ILogger<DrugsController> _logger;
    public int MaxPageSize { get; private set; } = 20;

    public DrugService(
        AppDbContext context,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
        //ILogger<DrugsController> logger)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        //_logger = logger;
    }

    public async Task<ResponseData<ListModel<Drug>>> GetDrugListAsync(string? genreNormalizedName, int pageNo = 1, int pageSize = 3)
    {
        if (pageSize > MaxPageSize)
            pageSize = MaxPageSize;

        var query = _context.Drugs.AsQueryable();
        var dataList = new ListModel<Drug>();

        query = query.Where(d => genreNormalizedName == null || d.Cathegory.NormalizedName.Equals(genreNormalizedName));

        var count = query.Count();

        if (count == 0)
        {
            return new ResponseData<ListModel<Drug>>
            {
                Data = dataList,
                Success = false,
                ErrorMessage = "No such drugs"
            };
        }

        int totalPages = (int)Math.Ceiling(count / (double)pageSize);

        if (pageNo > totalPages)
            return new ResponseData<ListModel<Drug>>
            {
                Data = null,
                Success = false,
                ErrorMessage = "No such page"
            };

        dataList.Items = await query
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        dataList.CurrentPage = pageNo;
        dataList.TotalPages = totalPages;

        var response = new ResponseData<ListModel<Drug>>
        {
            Data = dataList,
            Success = true,
        };

        return response;
    }

    public async Task<ResponseData<Drug>> CreateDrugAsync(Drug drug)
    {
        await _context.Drugs.AddAsync(drug);
        await _context.SaveChangesAsync();

        return new ResponseData<Drug>()
        {
            Data = drug,
            Success = true
        };
    }

    public async Task DeleteDrugAsync(int id)
    {
        var drug = await _context.Drugs.FindAsync(id);
        if (drug != null)
        {
            _context.Drugs.Remove(drug);
            await _context.SaveChangesAsync();
        }
        else
            throw new ArgumentException("Drug with such id not found");
    }

    public async Task<ResponseData<Drug>> GetDrugByIdAsync(int id)
    {
        var drug = await _context.Drugs.FindAsync(id);
        if (drug != null)
        {
            return new ResponseData<Drug>()
            {
                Data = drug,
                Success = true
            };
        }
        else
            return new ResponseData<Drug>()
            {
                Data = null,
                Success = false,
                ErrorMessage = "Drug with such id not found"
            };
    }

    public async Task<ResponseData<string>> SaveDrugAsync(int id, IFormFile formFile)
    {
        var responseData = new ResponseData<string>();
        var drug = await _context.Drugs.FindAsync(id);

        if (drug == null)
        {
            responseData.Success = false;
            responseData.ErrorMessage = "No item found";
            return responseData;
        }

        //var host = "https://" + _httpContextAccessor.HttpContext?.Request.Host;
        //var imageFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

        //if (formFile != null)
        //{
        //    //if (!string.IsNullOrEmpty(drug.Image))
        //    //{
        //    //    var prevImage = Path.GetFileName(drug.Image);
        //    //    var prevImagePath = Path.Combine(imageFolder, prevImage);

        //    //    if (File.Exists(prevImagePath))
        //    //        File.Delete(prevImagePath);
        //    //}



        //    //var ext = Path.GetExtension(formFile.FileName);
        //    //var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
        //    //var filePath = Path.Combine(imageFolder, fName);

        //    //using (var stream = new FileStream(filePath, FileMode.Create))
        //    //{
        //    //    await formFile.CopyToAsync(stream);
        //    //}

        //    //drug.Image = $"{host}/Images/{fName}";
        //    //drug.ImageMimeType = formFile.ContentType;

        //    _context.Entry(drug).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //}

        responseData.Data = drug.Image;
        return responseData;
    }

    public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
    {
        var responseData = new ResponseData<string>();
        var drug = await _context.Drugs.FindAsync(id);

        if (drug == null)
        {
            responseData.Success = false;
            responseData.ErrorMessage = "No item found";
            return responseData;
        }

        var host = "https://" + _httpContextAccessor.HttpContext?.Request.Host;
        var imageFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Drugs");

        if (formFile != null)
        {
            if (!string.IsNullOrEmpty(drug.Image))
            {
                var prevImage = Path.GetFileName(drug.Image);
                var prevImagePath = Path.Combine(imageFolder, prevImage);

                if (File.Exists(prevImagePath))
                    File.Delete(prevImagePath);
            }



            var ext = Path.GetExtension(formFile.FileName);
            var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
            var filePath = Path.Combine(imageFolder, fName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            drug.Image = $"{host}/Images/{fName}";
            //drug.ImageMimeType = formFile.ContentType;

            _context.Entry(drug).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        responseData.Data = drug.Image;
        return responseData;
    }

    public async Task UpdateDrugAsync(int id, Drug drug)
    {

        var drugToUpdate = await _context.Drugs.FindAsync(id);
        if (drugToUpdate != null)
        {
            drugToUpdate.Name = drug.Name;
            drugToUpdate.Description = drug.Description;
            drugToUpdate.Price = drug.Price;
            //pictureToUpdate.Author = drug.Author;
            drugToUpdate.Cathegory = drug.Cathegory;
            //drugToUpdate.GenreId = drug.GenreId;
            await _context.SaveChangesAsync();
        }
        else
            throw new ArgumentException("Drug with such id not found");
    }
}