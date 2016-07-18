using CompanyProjects;
using CompanyProjects.ViewModel;
using CompanyProjects.DataAccess;
using CompanyProjects.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace CompanyProjects.ViewModel
{


    public class MainViewModel : ViewModelBase
    {
        private CompanyDataContext db = new CompanyDataContext();

        public MainViewModel(User user)
        {
            initilazeFilter();
            LoggedUser = user;
        }

        #region Properties      

        ObservableCollection<Project> _filterAvaivbleProjects;
        // Ovo i AllProjects moze da bude jedno osim ako ne budu u istom prozoru
        public ObservableCollection<Project> FilterAvaivbleProjects
        {
            get
            {
                return _filterAvaivbleProjects;
            }
            set
            {
                if (value != _filterAvaivbleProjects)
                {
                    _filterAvaivbleProjects = value;
                    OnPropertyChanged();
                }
            }
        }

        private Project _projectFilterSelectedValue;
        public Project ProjectFilterSelectedValue
        {
            get { return _projectFilterSelectedValue; }
            set
            {
                if (_projectFilterSelectedValue != value)
                {
                    _projectFilterSelectedValue = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<Company> _filterAllCompanies;
        public ObservableCollection<Company> FilterAllCompanies
        {
            get
            {
                return _filterAllCompanies;
            }
            set
            {
                if (value != _filterAllCompanies)
                {
                    _filterAllCompanies = value;
                    OnPropertyChanged();
                }
            }
        }

        private Company _filterCompanySelectedValue;
        public Company FilterCompanySelectedValue
        {
            get { return _filterCompanySelectedValue; }
            set
            {
                if (_filterCompanySelectedValue != value)
                {
                    _filterCompanySelectedValue = value;

                    if (_filterCompanySelectedValue != null)
                    {
                        this.FilterAvaivbleProjects = new ObservableCollection<Project>(_filterCompanySelectedValue.AppropriateProjects);
                    }
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<DataEntry> _allDataEntries;
        public ObservableCollection<DataEntry> AllDataEntries
        {
            get
            {
                return _allDataEntries;
            }
            set
            {
                if (value != _allDataEntries)
                {
                    _allDataEntries = value;
                    OnPropertyChanged();
                }
            }
        }

        private DataEntry _gridSelectedItem;
        public DataEntry GridSelectedItem
        {
            get
            {
                return _gridSelectedItem;
            }
            set
            {
                if (_gridSelectedItem == value)
                {
                    return;
                }
                _gridSelectedItem = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _startFilterDate;
        public DateTime? StartFilterDate
        {
            get
            {
                return _startFilterDate;
            }
            set
            {
                if (value != _startFilterDate && value != DateTime.MinValue)
                {
                    //if (value == null)
                    //{
                    //    _startFilterDate = null;
                    //}
                    //else
                    //{
                    //    _startFilterDate = value.Value;
                    //} 

                    _startFilterDate = value;
                    OnPropertyChanged();
                }                
            }
        }

        private DateTime? _endFilterDate;
        public DateTime? EndFilterDate
        {
            get
            {
                return _endFilterDate;
            }
            set
            {
                if (value != _endFilterDate && value != DateTime.MinValue)
                {
                    if(value != null && _startFilterDate != null)
                    {
                        if (DateTime.Compare(value.Value, _startFilterDate.Value) >= 0)
                        {
                            _endFilterDate = value.Value;                            
                        }
                        else
                        {
                            MessageBox.Show("Krajnji Datum mora poceti posle Pocetnog. Izaberite ponovo!");
                        }                       
                    }
                    else {
                        _endFilterDate = value;
                    }
                    OnPropertyChanged();
                }
                //if (DateTime.Compare(value.Value, StartFilterDate.Value) < 0) MessageBox.Show("Krajnji Datum mora poceti posle Pocetnog. Izaberite ponovo!");
            }
        }

        private User _loggedUser;
        public User LoggedUser
        {
            get
            {
                return _loggedUser;
            }
            set
            {
                if (_loggedUser == value)
                {
                    return;
                }
                _loggedUser = value;
                OnPropertyChanged();
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
                        param => this.AddDataEntryCommandExecute(),
                        param => true);// (umesto CanExecute)
                }
                return _addDataEntryCommand;
            }
        }
        void AddDataEntryCommandExecute()
        {
            AddDataEntry window = new AddDataEntry(AllDataEntries);
            window.Show();
            window.Closing += Window_Closing;
            
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            initilazeFilter();
            //SearchCommandExecute();
        }



        private RelayCommand _companiesProjectsCommand;
        public ICommand CompaniesProjectsCommand
        {
            get
            {
                if (_companiesProjectsCommand == null)
                {
                    _companiesProjectsCommand = new RelayCommand(
                        param => this.CompaniesProjectsCommandExecute(),
                        param => true);// (umesto CanExecute)
                }
                return _companiesProjectsCommand;
            }
        }
        void CompaniesProjectsCommandExecute()
        {
            CompaniesProjectsOverview window = new CompaniesProjectsOverview();
            window.Show();
            window.Closing += Window_Closing1;
        }

        private void Window_Closing1(object sender, CancelEventArgs e)
        {
            initilazeFilter();
        }

        private RelayCommand _activateFileCommand;
        public ICommand ActivateFileCommand
        {
            get
            {
                if (_activateFileCommand == null)
                {
                    _activateFileCommand = new RelayCommand(
                        param => this.ActivateFileCommandExecute(),
                        param => true);// (umesto CanExecute)
                }
                return _activateFileCommand;
            }
        }
        void ActivateFileCommandExecute()
        {
            //Process.Start("C:\\1.rtf");
            //if (!String.IsNullOrEmpty(GridSelectedItem.DataProject))
            //{
            //    Process.Start(GridSelectedItem.DataProject);
            //}
            //else
            //{
            //    MessageBox.Show("Unesite validan dokument! ");
            //}
        }



        private RelayCommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand(
                        param => this.SearchCommandExecute(),
                        param => SearchCommandCanExecute);// (umesto CanExecute)
                }
                return _searchCommand;
            }
        }
        void SearchCommandExecute()
        {         
            IEnumerable<DataEntry> filterCompanies = db.DataEntry;
            if (FilterCompanySelectedValue != null)
            {
                filterCompanies = filterCompanies.Where(chanel => chanel.AppropriateProject.AppropriateCompany.CompanyId == (FilterCompanySelectedValue.CompanyId));
            }

            if (ProjectFilterSelectedValue != null)
            {
                filterCompanies = filterCompanies.Where(chanel => chanel.ProjectId == (ProjectFilterSelectedValue.ProjectId));
            }

            if (StartFilterDate != null)
            {
                filterCompanies = (from o in filterCompanies where o.Date >= StartFilterDate select o);
            }

            if (EndFilterDate != null)
            {
                filterCompanies = (from o in filterCompanies where o.Date <= EndFilterDate select o);
            }

            if (filterCompanies != null)
            {
                AllDataEntries = new ObservableCollection<DataEntry>(filterCompanies);
            }
        }
        public bool SearchCommandCanExecute
        {
            get
            {
                return true;
            }
        }


        private RelayCommand _inicializationCommand;
        public ICommand InicializationCommand
        {
            get
            {
                if (_inicializationCommand == null)
                {
                    _inicializationCommand = new RelayCommand(
                        param => this.InicializationCommandExecute(),
                        param => true);// (umesto CanExecute)
                }
                return _inicializationCommand;
            }
        }
        void InicializationCommandExecute()
        {
            initilazeFilter();
        }

        private RelayCommand _deleteDataEntryCommand;
        public ICommand DeleteDataEntryCommand
        {
            get
            {
                if (_deleteDataEntryCommand == null)
                {
                    _deleteDataEntryCommand = new RelayCommand(
                        param => this.DeleteDataEntryCommandExecute(),
                        param => CanDeleteDataEntryCommandExecute);// (umesto CanExecute)
                }
                return _deleteDataEntryCommand;
            }
        }
        void DeleteDataEntryCommandExecute()
        {
            MessageBoxResult m = MessageBox.Show(String.Format("Da li ste sigurni da zelite da obrisete unos kompanije: {0}?", GridSelectedItem.AppropriateProject.TitleProject), "Obrisi Unos", MessageBoxButton.YesNoCancel);

            if (m == MessageBoxResult.Yes)
            {
                db.DataEntry.Attach(GridSelectedItem);
                db.DataEntry.Remove(GridSelectedItem);
                db.SaveChanges();

                //de.RemoveDataEntry(GridSelectedItem); Ne radi opet onaj deo sa db.SaveChanges();!!
                AllDataEntries.Remove(GridSelectedItem);
            }
        }
        bool CanDeleteDataEntryCommandExecute
        {
            get
            {
                if (!(GridSelectedItem is DataEntry))
                    return false;

                return true;
            }
        }

        private RelayCommand _viewEditDataEntryCommand;
        public ICommand ViewEditDataEntryCommand
        {
            get
            {
                if (_viewEditDataEntryCommand == null)
                {
                    _viewEditDataEntryCommand = new RelayCommand(
                        param => this.ViewEditDataEntryCommandExecute(),
                        param => ViewEditDataEntryCommandCanExecute);// (umesto CanExecute)
                }
                return _viewEditDataEntryCommand;
            }
        }
        void ViewEditDataEntryCommandExecute()
        {
            EditDataEntry window = new EditDataEntry(GridSelectedItem);
            window.Show();
            window.Closing += Window_Closing2;
        }
        private void Window_Closing2(object sender, CancelEventArgs e)
        {
            AllDataEntries = new ObservableCollection<DataEntry>(AllDataEntries);
            SearchCommandExecute();
        }
        bool ViewEditDataEntryCommandCanExecute
        {
            get
            {
                if (!(GridSelectedItem is DataEntry))
                    return false;

                return true;
            }
        }

        private RelayCommand _changePasswordCommand;
        public ICommand ChangePasswordCommand
        {
            get
            {
                if (_changePasswordCommand == null)
                {
                    _changePasswordCommand = new RelayCommand(
                        param => this.ChangePasswordCommandExecute(),
                        param => true);// (umesto CanExecute)
                }
                return _changePasswordCommand;
            }
        }
        void ChangePasswordCommandExecute()
        {
            ChangeDetailsAndPassword window = new ChangeDetailsAndPassword(LoggedUser);
            window.Show();

        }
            #endregion

            #region Funcitons

            void initilazeFilter()
        {
            db = new CompanyDataContext();
            //using (db = new CompanyDataContext())
            //{
                FilterAllCompanies = new ObservableCollection<Company>(db.Company);
                FilterAvaivbleProjects = new ObservableCollection<Project>(db.Project);
                AllDataEntries = new ObservableCollection<DataEntry>(db.DataEntry); 
            //}

            FilterCompanySelectedValue = null;
            ProjectFilterSelectedValue = null;

            StartFilterDate = null;
            EndFilterDate = null; 
        }      
        #endregion
    }
}
