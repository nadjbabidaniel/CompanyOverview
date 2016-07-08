using CompanyProjects.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyProjects.DataAccess
{
    class CompanyRepository : IDisposable
    {
        private CompanyDataContext db = new CompanyDataContext();
        readonly List<Company> _company;

        public CompanyRepository()
        {
            if (_company == null)
            {
                _company = new List<Company>();
            }
            _company = db.Company.Include("AppropriateProjects").ToList();
        }

        public List<Company> GetCompanies()
        {
            return new List<Company>(_company);
        }
        public bool AddCompany(Company companyToAdd)
        {

            _company.Add(companyToAdd);
            
            try
            {
                db.Company.Add(companyToAdd);
                db.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool UpdateCompany(int id, Project proj)
        {
            //Company comp = GetCompany(id);
            var query = (from r in db.Company where r.CompanyId == id select r);
            foreach (var q in query)
            {                
                q.AppropriateProjects.Add(proj);
            }

            //db.Entry(comp).Collection(i => i.AppropriateProjects).Load();
            //comp.AppropriateProjects.Add(proj);
            try
            {
                //db.Entry(comp).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {                
                return false;
            }
            return true;
        }

        public Company GetCompany(int id)
        {
            return _company.FirstOrDefault(i => i.CompanyId == id);
        }

        public bool UpdateComponent(Company currentGridSelectedItem)
        {
            var query = (from r in db.Company where r.CompanyId == currentGridSelectedItem.CompanyId select r);

            foreach (var q in query)
            {
                q.TitleCompany = currentGridSelectedItem.TitleCompany;                        
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

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}
