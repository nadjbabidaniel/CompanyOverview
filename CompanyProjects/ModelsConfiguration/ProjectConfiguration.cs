using CompanyProjects.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyProjects.ModelsConfiguration
{
    class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        internal ProjectConfiguration()
        {
            this.HasRequired(a => a.AppropriateCompany)
                           .WithMany(r => r.AppropriateProjects)
                           .HasForeignKey(u => u.FKCompanyId)
                           .WillCascadeOnDelete(true);
        }
    }
}
