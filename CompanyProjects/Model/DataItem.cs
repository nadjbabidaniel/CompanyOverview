using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyProjects.Model
{
    public class DataItem
    {
        // --- PRIMARNI KLJUCEVI ----
        public int DataItemId { get; set; }

        // --- STRANI KLJUC
        public int DataEntryId { get; set; }

        // --- PODACI ---
        [Required]
        public string DataProject { get; set; }

        [Required]
        public string TitleDataProject { get; set; }



        // --- VIRTUALNI NAVIGACIONI OBJEKTI ---

        public virtual DataEntry AppropriateDataEntry { get; set; }
    }
}
