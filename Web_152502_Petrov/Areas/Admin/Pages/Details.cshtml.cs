using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web_152502_Petrov.Data;
using Web_152502_Petrov.Domain.Entities;
using Web_152502_Petrov.Services.CathegoryService;
using Web_152502_Petrov.Services.DrugService;


//public class DetailsModel : PageModel
//{
//    private readonly Web_152502_Petrov.Data.Web_152502_PetrovContext _context;

//    public DetailsModel(Web_152502_Petrov.Data.Web_152502_PetrovContext context)
//    {
//        _context = context;
//    }

//  public Drug Drug { get; set; } = default!; 

//    public async Task<IActionResult> OnGetAsync(int? id)
//    {
//        if (id == null || _context.Drug == null)
//        {
//            return NotFound();
//        }

//        var drug = await _context.Drug.FirstOrDefaultAsync(m => m.Id == id);
//        if (drug == null)
//        {
//            return NotFound();
//        }
//        else 
//        {
//            Drug = drug;
//        }
//        return Page();
//    }
//}

namespace Web_152502_Petrov.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IDrugService _drugService;
        private readonly ICathegoryService _cathegoryService;

        public DetailsModel(
            IDrugService pictureService,
            ICathegoryService pictureGenreService)
        {
            _drugService = pictureService;
            _cathegoryService = pictureGenreService;
        }

        public Drug Drug { get; set; } = default!;
        public Cathegory Cathegory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var responseDrug = await _drugService.GetDrugByIdAsync(id.Value);
            var responseCathegories = await _cathegoryService.GetCathegoryListAsync();

            if (!responseDrug.Success || !responseCathegories.Success)
            {
                return NotFound(responseDrug.ErrorMessage + '\n' + responseCathegories.ErrorMessage);
            }

            Drug = responseDrug.Data!;
            Cathegory = responseCathegories.Data!.FirstOrDefault(g => g.Id == Cathegory.Id);

            return Page();
        }
    }
}


