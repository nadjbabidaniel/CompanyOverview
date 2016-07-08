using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyProjects.Model
{
    public class Company
    {
        // --- PRIMARNI KLJUCEVI ----
        public int CompanyId { get; set; }

        // --- STRANI KLJUC

        // --- PODACI ---
        [Required]
        [StringLength(64, ErrorMessage = "Title cant be longer then 64 characters")]
        public string TitleCompany { get; set; }

        
        // --- VIRTUALNI NAVIGACIONI OBJEKTI ---
        public virtual IList<Project> AppropriateProjects { get; set; }
       

    }
}
