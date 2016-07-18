using CompanyProjects.Model;
using CompanyProjects.ModelsConfiguration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyProjects.DataAccess
{
    class CompanyDataContext : DbContext
    {
        public CompanyDataContext() : base ("CompanyDataConnection")
        {
        }

        public DbSet<Company> Company { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<DataEntry> DataEntry { get; set; }
        public DbSet<DataItem> DataItem { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ProjectConfiguration());
            //modelBuilder.Configurations.Add(new MeetingConfiguration());
        }

    }
}
