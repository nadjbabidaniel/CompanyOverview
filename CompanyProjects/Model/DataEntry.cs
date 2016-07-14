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

        //[Required]
        //public string CompanyTitle { get; set; }

        [Required]
        public int ProjectId { get; set; }

        //[Required]
        //public string ProjectTitle { get; set; }

        // --- PODACI ---
        [Required]    
        public DateTime Date { get; set; }        

        public string TextInput { get; set; }

        //public string DataProject { get; set; }//zakoment

        //public string TitleDataProject { get; set; }//zakoment



        // --- VIRTUALNI NAVIGACIONI OBJEKTI ---
        public virtual IList<DataItem> AppropriatDataItems { get; set; }

    }
}
