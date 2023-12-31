using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web_152502_Petrov.Data;
using Web_152502_Petrov.Domain.Entities;

namespace Web_152502_Petrov
{
    public class IndexModel : PageModel
    {
        private readonly Web_152502_Petrov.Data.Web_152502_PetrovContext _context;

        public IndexModel(Web_152502_Petrov.Data.Web_152502_PetrovContext context)
        {
            _context = context;
        }

        public IList<Drug> Drug { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Drug != null)
            {
                Drug = await _context.Drug.ToListAsync();
            }
        }
    }
}
