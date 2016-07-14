using CompanyProjects.DataAccess;
using CompanyProjects.Model;
using CompanyProjects.ViewModel;
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
    class AddDataEntryViewModel : ViewModelBase
    {
        readonly CompanyRepository _companyRepository;
        readonly DataEntryRepository _dataEntryrepository;
        ObservableCollection<DataEntry> AllDataEntriesCurrent;
        string filePath = String.Empty;     

        public AddDataEntryViewModel(ObservableCollection<DataEntry> AllDataEntries)
        {
            _companyRepository = new CompanyRepository();
            _dataEntryrepository = new DataEntryRepository();

            AvaivbleCompanies = new ObservableCollection<Company>(_companyRepository.GetCompanies());
            AllDataEntriesCurrent = AllDataEntries;

            _entryDate = DateTime.Now;
        }

        #region Properties
        ObservableCollection<Company> _avaivbleCompanies;
        public ObservableCollection<Company> AvaivbleCompanies
        {
            get
            {
                return _avaivbleCompanies;
            }
            set
            {
                if (value != _avaivbleCompanies)
                {
                    _avaivbleCompanies = value;
                    OnPropertyChanged();
                }
            }
        }

        private Company _companySelectedValue;
        public Company CompanySelectedValue
        {
            get { return _companySelectedValue; }
            set
            {
                if (_companySelectedValue != value)
                {
                    _companySelectedValue = value;
                  
                    this.AvaivbleProjects = new ObservableCollection<Project>(_companySelectedValue.AppropriateProjects.Where(i=>i.EndDate >= DateTime.Now || i.EndDate == null));
                    OnPropertyChanged();
                }
            }
        }

        ObservableCollection<Project> _avaivbleProjects;
        public ObservableCollection<Project> AvaivbleProjects
        {
            get
            {
                return _avaivbleProjects;
            }
            set
            {
                if (value != _avaivbleProjects)
                {
                    _avaivbleProjects = value;                    
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

        private DateTime _entryDate;
        public DateTime EntryDate
        {
            get
            {
                return _entryDate;
            }
            set
            {
                if (value != _entryDate)
                {
                    _entryDate = value;
                    OnPropertyChanged();
                }
            }
        }      

        private string _textInput;
        public string TextInput
        {
            get
            {
                return _textInput;
            }
            set
            {
                if (value != _textInput)
                {
                    _textInput = value;
                    OnPropertyChanged();
                }
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


        #region Commands

        private RelayCommand _addDataEntryCommand;
        public ICommand AddDataEntryCommand
        {
            get
            {
                if (_addDataEntryCommand == null)
                {
                    _addDataEntryCommand = new RelayCommand(
                        param => this.SaveEntryCommandExecute(),
                        param => this.SaveEntryCommandCanExecute);// this.SaveEntryCommandCanExecute);
                }
                return _addDataEntryCommand;
            }
        }

        void SaveEntryCommandExecute()
        {
            DataEntry dte = new DataEntry();
            //dte.CompanyId = CompanySelectedValue.CompanyId;
            //dte.CompanyTitle = CompanySelectedValue.TitleCompany;
            dte.Date = EntryDate;
            dte.ProjectId = ProjectSelectedValue.ProjectId;
            //dte.ProjectTitle = ProjectSelectedValue.TitleProject;
            dte.TextInput = TextInput;            
            //dte.DataProject = filePath;
            //dte.TitleDataProject = FileName;            

            _dataEntryrepository.AddDataEntry(dte);
            AllDataEntriesCurrent.Add(dte);

            CloseAction();
        }

        bool SaveEntryCommandCanExecute
        {
            get
            {
                if (!(this.CompanySelectedValue is Company))
                    return false;

                if (!(this.ProjectSelectedValue is Project))
                    return false;

                //if (String.IsNullOrWhiteSpace(this.TextInput) || (TextInput.Length > 512))
                //{
                //    return false;
                //}             

                return true;
            }
        }

        private RelayCommand _addFileToEntryCommand;
        public ICommand AddFileToEntryCommand
        {
            get
            {
                if (_addFileToEntryCommand == null)
                {
                    _addFileToEntryCommand = new RelayCommand(
                        param => this.AddFileToEntryCommandExecute(),
                        param => true);// this.SaveOtpremnicaCommandCanExecute);
                }
                return _addFileToEntryCommand;
            }
        }

        void AddFileToEntryCommandExecute()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.InitialDirectory = @"C:\";
            //dlg.Title = "Seleccione los archivos";
            Nullable<bool> result = dlg.ShowDialog();

            if (result.HasValue && result.Value)
            {
                // Open document 
                filePath = dlg.FileName;

                FileName = dlg.FileName;
                int i = FileName.LastIndexOf('\\');
                if (i > -1) { FileName = FileName.Substring(i + 1); }

                MessageBox.Show(FileName);                               
            }
        }

        private RelayCommand _removeFileFromEntryCommand;
        public ICommand RemoveFileFromEntryCommand
        {
            get
            {
                if (_removeFileFromEntryCommand == null)
                {
                    _removeFileFromEntryCommand = new RelayCommand(
                        param => this.RemoveFileFromEntryCommandExecute(),
                        param => /*true); */this.RemoveFileFromEntryCommandBool);
                }
                return _removeFileFromEntryCommand;
            }
        }

        void RemoveFileFromEntryCommandExecute()
        {
            FileName = String.Empty;
            filePath = String.Empty;
        }

        bool RemoveFileFromEntryCommandBool
        {
            get
            {
                if (String.IsNullOrEmpty(FileName))
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
