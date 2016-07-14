using CompanyProjects.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyProjects.DataAccess
{
    class DataEntryRepository : IDisposable
    {
        private CompanyDataContext db = new CompanyDataContext();
        readonly List<DataEntry> _dataEntry;
        public DataEntryRepository()
        {
            if (_dataEntry == null)
            {
                _dataEntry = new List<DataEntry>();
            }
            _dataEntry = db.DataEntry.ToList();
        }
        public List<DataEntry> GetDataEntry()
        {
            return new List<DataEntry>(_dataEntry);
        }
        public bool AddDataEntry(DataEntry dataEntryToAdd)
        {

            _dataEntry.Add(dataEntryToAdd);
            try
            {
                db.DataEntry.Add(dataEntryToAdd);
                db.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool RemoveDataEntry(DataEntry dataEntryToAdd)
        {

            _dataEntry.Remove(dataEntryToAdd);
            try
            {
                db.DataEntry.Remove(dataEntryToAdd);
                db.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool UpdateDataEntry(DataEntry CurrentGridSelectedItem)
        {
            var query = (from r in db.DataEntry where r.DataEntryId == CurrentGridSelectedItem.DataEntryId select r);
            foreach (var q in query)
            {
                q.CompanyId = CurrentGridSelectedItem.CompanyId;
                //q.CompanyTitle = CurrentGridSelectedItem.CompanyTitle;
                //q.DataProject = CurrentGridSelectedItem.DataProject;
                q.Date = CurrentGridSelectedItem.Date;
                q.ProjectId = CurrentGridSelectedItem.ProjectId;
                //q.ProjectTitle = CurrentGridSelectedItem.ProjectTitle;
                q.TextInput = CurrentGridSelectedItem.TextInput;
                //q.TitleDataProject = CurrentGridSelectedItem.TitleDataProject;
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
        public bool UpdateDataEntryCompanyName(Company CurrentGridSelectedItem)
        {
            var query = (from r in db.DataEntry where r.CompanyId == CurrentGridSelectedItem.CompanyId select r);

            foreach (var q in query)
            {                
                q.CompanyTitle = CurrentGridSelectedItem.TitleCompany;                
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
        public bool UpdateDataEntryProjectName(Project CurrentGridSelectedItem)
        {
            var query = (from r in db.DataEntry where r.ProjectId == CurrentGridSelectedItem.ProjectId select r);

            foreach (var q in query)
            {
                q.ProjectTitle = CurrentGridSelectedItem.TitleProject;
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
