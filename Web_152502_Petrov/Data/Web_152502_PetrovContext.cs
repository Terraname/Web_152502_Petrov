using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web_152502_Petrov.Domain.Entities;

namespace Web_152502_Petrov.Data
{
    public class Web_152502_PetrovContext : DbContext
    {
        public Web_152502_PetrovContext (DbContextOptions<Web_152502_PetrovContext> options)
            : base(options)
        {
        }

        public DbSet<Web_152502_Petrov.Domain.Entities.Drug> Drug { get; set; } = default!;
    }
}
