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
            _dataEntry = db.DataEntry.Include("AppropriateProject").ToList();
            _dataEntry = db.DataEntry.Include("AppropriateDataItems").ToList();
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
            var q = db.DataEntry.Where(c => c.DataEntryId == CurrentGridSelectedItem.DataEntryId).SingleOrDefault();

            q.Date = CurrentGridSelectedItem.Date;
            q.ProjectId = CurrentGridSelectedItem.ProjectId;
            q.TextInput = CurrentGridSelectedItem.TextInput;

            var forDeleting = new List<DataItem>();
            foreach (var item in q.AppropriateDataItems)
            {
                if (!(CurrentGridSelectedItem.AppropriateDataItems.Any(c => c.DataItemId == item.DataItemId)))
                {
                    forDeleting.Add(item);
                }
            }
            foreach (var item in forDeleting)
            {
                db.DataItem.Remove(item);
            }

            foreach (var item in CurrentGridSelectedItem.AppropriateDataItems)
            {
                //DataItem existingItem = null;
                //existingItem = q.AppropriateDataItems.Where(c => c.DataItemId == item.DataItemId).SingleOrDefault();//whyyyy????wont work
                //if (existingItem == null)

                if (item.DataItemId == 0)
                {
                    q.AppropriateDataItems.Add(item);
                }
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
