using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyProjects.Model
{
    public class DataEntry
    {
        // --- PRIMARNI KLJUCEVI ----
        public int DataEntryId { get; set; }


        // --- //STRANI KLJUCevi

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public string CompanyTitle { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public string ProjectTitle { get; set; }

        // --- PODACI ---
        [Required]    
        public DateTime Date { get; set; }        

        //[Required] // za ovo jos proveri da li je required
        //[StringLength(512, ErrorMessage = "Title cant be longer then 512 characters")]
        public string TextInput { get; set; }

        public string DataProject { get; set; }

        public string TitleDataProject { get; set; }


    }
}
