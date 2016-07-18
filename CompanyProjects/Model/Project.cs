using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyProjects.Model
{
    public class Project
    {
        // --- PRIMARNI KLJUCEVI ----
        public int ProjectId { get; set; }

        // --- STRANI KLJUC
        public int FKCompanyId { get; set; }

        // --- PODACI ---
        [Required]
        [StringLength(64, ErrorMessage = "Title cant be longer then 64 characters")]
        public string TitleProject { get; set; }

        [Required]
        [StringLength(128, ErrorMessage = "Text cant be longer then 128 characters")]
        public string TextProject { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]  
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? EndDate { get; set; }


        // --- VIRTUALNI NAVIGACIONI OBJEKTI ---

        public virtual Company AppropriateCompany { get; set; }

        public virtual IList<DataEntry> AppropriateDataEntries { get; set; }


    }
}
