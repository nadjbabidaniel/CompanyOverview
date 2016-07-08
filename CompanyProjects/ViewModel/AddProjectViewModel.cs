using CompanyProjects;
using CompanyProjects.ViewModel;
using CompanyProjects.DataAccess;
using CompanyProjects.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data.Entity.Core.Objects;
using System.Windows.Input;
using System.Windows;

namespace CompanyProjects.ViewModel
{
    class AddProjectViewModel : ViewModelBase
    {
        ObservableCollection<Project> AllProjectsCurrent;

        readonly CompanyDataContext db;
        readonly CompanyRepository _companyRepository;
        readonly ProjectRepository _projectRepository;
        public AddProjectViewModel(ObservableCollection<Project> AllProjects, ObservableCollection<Company> gridGridSelectedCollection)
        {
            _companyRepository = new CompanyRepository();
            _projectRepository = new ProjectRepository();
            db = new CompanyDataContext();

            AllAvaivbleCompaniesCurrent = new ObservableCollection<Company>(_companyRepository.GetCompanies());
            AllProjectsCurrent = AllProjects;
            gridCurrentGridSelectedCollection = gridGridSelectedCollection;
            _fileName = gridGridSelectedCollection[0].TitleCompany;

            _startDate = DateTime.Now;
        }

        #region Properties

        private ObservableCollection<Company> _allAvaivbleCompaniesCurrent;
        public ObservableCollection<Company> AllAvaivbleCompaniesCurrent
        {
            get
            {
                return _allAvaivbleCompaniesCurrent;
            }
            set
            {
                if (value != _allAvaivbleCompaniesCurrent)
                {
                    _allAvaivbleCompaniesCurrent = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _titleProject;
        public string TitleProject
        {
            get
            {
                return _titleProject;
            }
            set
            {
                if (value != _titleProject)
                {
                    _titleProject = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _textProject;
        public string TextProject
        {
            get
            {
                return _textProject;
            }
            set
            {
                if (value != _textProject)
                {
                    _textProject = value;
                    OnPropertyChanged();
                }
            }
        }

        //private Company _companySelectedValue;
        ObservableCollection<Company> gridCurrentGridSelectedCollection;
        //public Company CompanySelectedValue
        //{
        //    get { return _companySelectedValue; }
        //    set
        //    {
        //        if (_companySelectedValue != value)
        //        {
        //            _companySelectedValue = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}


        private DateTime _startDate;
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                if (value != _startDate)
                {
                    _startDate = value.Date;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                if (value != _endDate && value != DateTime.MinValue && DateTime.Compare(value.Value, _startDate) > 0)
                {
                    _endDate = value.Value.Date;
                    OnPropertyChanged();
                }
                if (DateTime.Compare(value.Value, _startDate) < 0) MessageBox.Show("Krajnji Datum mora poceti posle Pocetnog. Izaberite ponovo!");
            }
        }

        private String _fileName;
        public String FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                if (value != _fileName)
                {
                    _fileName = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion


        #region Comands

        private RelayCommand _saveProjectCommand;
        public ICommand SaveProjectCommand
        {
            get
            {
                if (_saveProjectCommand == null)
                {
                    _saveProjectCommand = new RelayCommand(
                        param => this.SaveProjectCommandExecute(),
                        param => SaveProjectCommandCanExecute);// this.SaveOtpremnicaCommandCanExecute);
                }
                return _saveProjectCommand;
            }
        }

        void SaveProjectCommandExecute()
        {
            Project proj = new Project();
            proj.TitleProject = TitleProject;
            proj.TextProject = TextProject;
            proj.FKCompanyId = gridCurrentGridSelectedCollection[0].CompanyId;
            proj.StartDate = StartDate.Date;

            if (EndDate != DateTime.MinValue && EndDate != null)
            {
                proj.EndDate = EndDate.Value.Date;
            }


            _projectRepository.AddProject(proj, gridCurrentGridSelectedCollection[0]);
            //Company cmp = db.Company.Find(gridCurrentGridSelectedCollection[0].CompanyId);  //_companyRepository.GetCompany(CompanySelectedValue.CompanyId);
            //cmp.AppropriateProjects.Add(proj);
            //_companyRepository.UpdateCompany(cmp.CompanyId, proj);

            proj.AppropriateCompany = gridCurrentGridSelectedCollection[0];
          

            //gridCurrentGridSelectedCollection[0].AppropriateProjects.Add(proj);
            AllProjectsCurrent.Add(proj);

            //proj = _projectRepository.GetProject(proj.ProjectId);
            //_companyRepository.UpdateCompany(CompanySelectedValue.CompanyId, proj);
            CloseAction();
        }
        public bool SaveProjectCommandCanExecute
        {
            get
            {
                if (String.IsNullOrEmpty(this.TitleProject))
                    return false;
                if (String.IsNullOrEmpty(this.TextProject))
                    return false;
                //if (this.StartDate.ToString().Equals(""))
                //    return false;

                return true;
            }
        }

        public Action CloseAction { get; set; } // SET uradjen u backend kodu.
        private RelayCommand _closeWindowCommand;
        public ICommand CloseWindowCommand
        {
            get
            {
                if (_closeWindowCommand == null)
                {
                    _closeWindowCommand = new RelayCommand(
                        param => this.CloseWindowCommandExecute(),
                        param => true);
                }
                return _closeWindowCommand;
            }
        }      

        void CloseWindowCommandExecute()
        {
            CloseAction();
        }
        #endregion
    }
}
