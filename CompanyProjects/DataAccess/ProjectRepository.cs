using CompanyProjects.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyProjects.DataAccess
{
    class ProjectRepository : IDisposable
    {
        private CompanyDataContext db = new CompanyDataContext();
        private CompanyRepository compRepo = new CompanyRepository();

        readonly List<Project> _project;

        public ProjectRepository()
        {
            if (_project == null)
            {
                _project = new List<Project>();
            }
            //_project = db.Project.ToList();
            _project = db.Project.Include("AppropriateCompany").ToList();
        }
        public List<Project> GetProjects()
        {
            return new List<Project>(_project);
        }
        public Project GetProject(int id)
        {
            return db.Project.FirstOrDefault(i=> i.ProjectId == id);
        }
        public bool AddProject(Project projectToAdd, Company CompanySelectedValue)
        {
            _project.Add(projectToAdd);
            try
            {
                db.Project.Add(projectToAdd);
                db.SaveChanges();
            }
            catch
            {
                return false;
            }
            //try
            //{
            //    db.Entry(CompanySelectedValue).State = EntityState.Modified;
            //    db.SaveChanges();
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}

            //db.Entry(projectToAdd).Property(i=>i.AppropriateCompany);
            //db.Entry(oz).Collection(i => i.MaticneStavkeOtpremnice).Load();

            return true;
        }
        public bool UpdateProject(Project projectToChange)
        {
            var query = (from r in db.Project where r.ProjectId == projectToChange.ProjectId select r);
            foreach (var q in query)
            {
                q.TitleProject = projectToChange.TitleProject;
                q.TextProject = projectToChange.TextProject;
                q.StartDate = projectToChange.StartDate;
                q.EndDate = projectToChange.EndDate;              
            }
            try
            {
                db.SaveChanges();
            }
            catch
            {
                return false;
            }
           
            return true;
        }

        public IQueryable<Project> GetProjectBasedOnCompanyId(int companyId)
        {
            IQueryable<Project> var = db.Project.Where(q=>q.FKCompanyId ==companyId).Select(q=>q);

            return var;
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}