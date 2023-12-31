using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_152502_Petrov.Domain.Entities
{
    public class Drug
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Cathegory Cathegory { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }

        //public int CathegoryID { get; set; }
        //public Cathegory? cathegory { get; set; }
    }
}
