using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web_152502_Petrov.Data;
using Web_152502_Petrov.Domain.Entities;
//using Web_152502_Petrov.Domain.Entities;
using Web_152502_Petrov.Services.DrugService;

namespace Web_152502_Petrov.Areas.Admin.Pages
{
    //public class DeleteModel : PageModel
    //{
    //    private readonly Web_152502_PetrovContext _context;

    //    public DeleteModel(Web_152502_PetrovContext context)
    //    {
    //        _context = context;
    //    }

    //    [BindProperty]
    //    public Drug Drug { get; set; } = default!;

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

    //    public async Task<IActionResult> OnPostAsync(int? id)
    //    {
    //        if (id == null || _context.Drug == null)
    //        {
    //            return NotFound();
    //        }
    //        var drug = await _context.Drug.FindAsync(id);

    //        if (drug != null)
    //        {
    //            Drug = drug;
    //            _context.Drug.Remove(Drug);
    //            await _context.SaveChangesAsync();
    //        }

    //        return RedirectToPage("./Index");
    //    }


    //}



    public class DeleteModel : PageModel
    {
        private readonly IDrugService _drugService;

        public DeleteModel(IDrugService drugService)
        {
            _drugService = drugService;
        }

        [BindProperty]
        public Drug Drug { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await _drugService.GetDrugByIdAsync(id.Value);

            if (!response.Success)
            {
                return NotFound(response.ErrorMessage);
            }

            Drug = response.Data!;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // todo: если переделаю метод то добавить проверку
            await _drugService.DeleteDrugAsync(id.Value);

            return RedirectToPage("./Index");
        }
    }
    
}
