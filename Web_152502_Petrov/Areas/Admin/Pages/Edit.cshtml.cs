using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_152502_Petrov.Data;
using Web_152502_Petrov.Domain.Entities;
using Web_152502_Petrov.Services.CathegoryService;
using Web_152502_Petrov.Services.DrugService;
namespace Web_152502_Petrov.Areas.Admin.Pages
{
    //public class EditModel : PageModel
    //{
    //    private readonly Web_152502_Petrov.Data.Web_152502_PetrovContext _context;

    //    public EditModel(Web_152502_Petrov.Data.Web_152502_PetrovContext context)
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

    //        var drug =  await _context.Drug.FirstOrDefaultAsync(m => m.Id == id);
    //        if (drug == null)
    //        {
    //            return NotFound();
    //        }
    //        Drug = drug;
    //        return Page();
    //    }

    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see https://aka.ms/RazorPagesCRUD.
    //    public async Task<IActionResult> OnPostAsync()
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return Page();
    //        }

    //        _context.Attach(Drug).State = EntityState.Modified;

    //        try
    //        {
    //            await _context.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!DrugExists(Drug.Id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }

    //        return RedirectToPage("./Index");
    //    }

    //    private bool DrugExists(int id)
    //    {
    //      return (_context.Drug?.Any(e => e.Id == id)).GetValueOrDefault();
    //    }
    //}

    public class EditModel : PageModel
    {
        private readonly IDrugService _drugService;
        private readonly ICathegoryService _cathegoryService;

        public EditModel(
            IDrugService pictureService,
            ICathegoryService cathegoryService)
        {
            _drugService = pictureService;
            _cathegoryService = cathegoryService;
        }

        [BindProperty]
        public Drug Drug { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var responseDrug = await _drugService.GetDrugByIdAsync(id.Value);
            var responseCathegory = await _cathegoryService.GetCathegoryListAsync();

            if (!responseDrug.Success || !responseCathegory.Success)
            {
                return NotFound(responseDrug.ErrorMessage + '\n' + responseCathegory.ErrorMessage);
            }

            Drug = responseDrug.Data!;

            ViewData["GenreId"] = new SelectList(responseCathegory.Data!, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _drugService.UpdateDrugAsync(Drug.Id, Drug, Image);
            }
            catch (Exception)
            {
                if (!await DrugExists(Drug.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> DrugExists(int id)
        {
            return (await _drugService.GetDrugByIdAsync(id)).Success;
        }
    }

}
