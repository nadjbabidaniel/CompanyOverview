using CompanyProjects.DataAccess;
using CompanyProjects.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CompanyProjects.ViewModel
{
    class ChangeProjectViewModel : ViewModelBase
    {        
        //readonly ProjectRepository _projectRepository;        
        //readonly DataEntryRepository _dataEntryRepository;

        public ObservableCollection<Project> AllCurrentProjects;

        public ChangeProjectViewModel(ObservableCollection<Project> AllProjects, Project ProjectGridSelectedItem)
        {
            AllCurrentProjects = AllProjects;

            _projectSelectedValue = ProjectGridSelectedItem;
            _titleProject = ProjectGridSelectedItem.TitleProject;
            _textProject = ProjectGridSelectedItem.TextProject;
            using (CompanyDataContext db = new CompanyDataContext())
            {
                FileName = db.Company.FirstOrDefault(i => i.CompanyId == ProjectGridSelectedItem.FKCompanyId).TitleCompany;
            }

            _startDate = ProjectGridSelectedItem.StartDate;
            _endDate = ProjectGridSelectedItem.EndDate;           
        }


        #region Properties
      
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

        private Project _projectSelectedValue;
        public Project ProjectSelectedValue
        {
            get { return _projectSelectedValue; }
            set
            {
                if (_projectSelectedValue != value)
                {
                    _projectSelectedValue = value;
                    OnPropertyChanged();
                }
            }
        }


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
                    _startDate = value;
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
            proj.ProjectId = ProjectSelectedValue.ProjectId;
            proj.TitleProject = TitleProject;
            proj.TextProject = TextProject;
            proj.FKCompanyId = _projectSelectedValue.FKCompanyId; // _companyRepository.GetCompany(Current.FKCompanyId).CompanyId;
            proj.StartDate = StartDate;

            if (EndDate != DateTime.MinValue && EndDate != null)
            {
                proj.EndDate = EndDate.Value;
            }
            else { proj.EndDate = null; }

            using (ProjectRepository _projectRepository = new ProjectRepository())
            {
                if (_projectRepository.UpdateProject(proj))
                {
                    using (DataEntryRepository _dataEntryRepository = new DataEntryRepository())
                    {
                        _dataEntryRepository.UpdateDataEntryProjectName(proj);
                    }
                }
            }           
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
