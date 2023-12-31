using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_152502_Petrov.Data;
using Web_152502_Petrov.Domain.Entities;
using Web_152502_Petrov.Services.CathegoryService;
using Web_152502_Petrov.Services.DrugService;


namespace Web_152502_Petrov.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        //private readonly Web_152502_Petrov.Data.Web_152502_PetrovContext _context;

        //public CreateModel(Web_152502_Petrov.Data.Web_152502_PetrovContext context)
        //{
        //    _context = context;
        //}
        private readonly IDrugService _drugService;
        private readonly ICathegoryService _cathegoryService;

        public CreateModel(
            IDrugService drugService,
            ICathegoryService cathegoryService)
        {
            _drugService = drugService;
            _cathegoryService = cathegoryService;
        }

        public async Task<IActionResult> OnGet()
        {
            var response = await _cathegoryService.GetCathegoryListAsync();

            if (!response.Success)
            {
                return NotFound(response.ErrorMessage);
            }

            ViewData["CathegoryId"] = new SelectList(response.Data, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Drug Drug { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var response = await _drugService.CreateDrugAsync(Drug, Image);

            if (!response.Success)
                return NotFound(response.ErrorMessage);

            return RedirectToPage("./Index");
        }
    }
}


