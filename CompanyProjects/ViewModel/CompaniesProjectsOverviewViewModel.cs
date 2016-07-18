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
using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

namespace CompanyProjects.ViewModel
{
    class CompaniesProjectsOverviewViewModel : ViewModelBase
    {
        private CompanyDataContext db;// = new CompanyDataContext();
        //private ProjectRepository _prRepository = new ProjectRepository();
        //private CompanyRepository _companyRepository;

        public CompaniesProjectsOverviewViewModel()
        {
            db = new CompanyDataContext();
            AllCompanies = new ObservableCollection<Company>(db.Company);
            //_companyRepository = new CompanyRepository();
        }


        #region Properties
        private ObservableCollection<Company> _allCompanies;
        public ObservableCollection<Company> AllCompanies
        {
            get
            {
                return _allCompanies;
            }
            set
            {
                if (value != _allCompanies)
                {
                    _allCompanies = value;
//                    GridSelectedItem = AllCompanies.FirstOrDefault();
                    OnPropertyChanged();
                }
            }
        }      



        private ObservableCollection<Project> _allProjects;
        public ObservableCollection<Project> AllProjects
        {
            get
            {
                return _allProjects;
            }
            set
            {
                //if (value != _allProjects)
                {
                    _allProjects = value;
                    OnPropertyChanged();
                }
            }
        }


        private Company _gridSelectedItem;
        public Company GridSelectedItem
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
                //_gridSelectedItem sada ima sadrzaj selektovanog reda iz DataGrid-a.
                try
                {
                    if (GridSelectedItem != null)
                    {
                        //this.AllProjects = new ObservableCollection<Project>(db.Project.Where(i => i.FKCompanyId == _gridSelectedItem.CompanyId));
                        using (ProjectRepository pr2 = new ProjectRepository())
                        {
                            IList<Project> variable = pr2.GetProjects().Where(i => i.FKCompanyId == GridSelectedItem.CompanyId).ToList();
                            this.AllProjects = new ObservableCollection<Project>(variable);
                        }
                    }
                }
                catch (NullReferenceException)
                {
                    this.AllProjects = new ObservableCollection<Project>();
                }

                OnPropertyChanged();
            }
        }



        private Project _projectGridSelectedItem;
        public Project ProjectGridSelectedItem
        {
            get
            {
                return _projectGridSelectedItem;
            }
            set
            {
                if (_projectGridSelectedItem == value)
                {
                    return;
                }
                _projectGridSelectedItem = value;             
                OnPropertyChanged();
            }
        }
        #endregion




        #region Commands
        private RelayCommand _addCompanyCommand;
        public ICommand AddCompanyCommand
        {
            get
            {
                if (_addCompanyCommand == null)
                {
                    _addCompanyCommand = new RelayCommand(
                        param => this.AddCompanyCommandExecute(),
                        param => true);// (umesto CanExecute)
                }
                return _addCompanyCommand;
            }
        }
        void AddCompanyCommandExecute()
        {
            AddCompany window = new AddCompany(AllCompanies);
            window.Show();
            window.Closing += Window_Closing1;
        }
        private void Window_Closing1(object sender, CancelEventArgs e)
        {
            GridSelectedItem = AllCompanies[AllCompanies.Count-1];
        }



        private RelayCommand _removeCompanyCommand;
        public ICommand RemoveCompanyCommand
        {
            get
            {
                if (_removeCompanyCommand == null)
                {
                    _removeCompanyCommand = new RelayCommand(
                        param => this.RemoveCompanyCommandExecute(),
                        param => AddProjectCommandCanExecute);// (umesto CanExecute)
                }
                return _removeCompanyCommand;
            }
        }
        void RemoveCompanyCommandExecute()
        {
            var query = db.DataEntry.FirstOrDefault(x=> x.AppropriateProject.AppropriateCompany.CompanyId == GridSelectedItem.CompanyId);
            if(query != null)
            {
                MessageBox.Show("Kompanija se nalazi u Unosu na pocetnoj strani, molimo obrisite je prvo odatle.");
            }
            else
            {
                MessageBoxResult m = MessageBox.Show(String.Format("Da li ste sigurni da zelite da obrisete Kompaniju {0}?", GridSelectedItem.TitleCompany), "Obrisi Kompaniju", MessageBoxButton.YesNoCancel);

                if (m == MessageBoxResult.Yes)
                {
                    var variable = db.Project.Where(q => q.FKCompanyId == GridSelectedItem.CompanyId).Select(q => q);

                    foreach (var item in variable)
                    {
                        db.Project.Remove(item);
                    }
                    
                    db.Company.Attach(GridSelectedItem);
                    db.Company.Remove(GridSelectedItem);
                    db.SaveChanges();

                    var prom = AllCompanies.IndexOf(GridSelectedItem);
                    if (prom > 0) GridSelectedItem = AllCompanies[prom - 1];
                    else GridSelectedItem = null;


                    AllCompanies.Remove(AllCompanies[prom]);
                    if (AllCompanies.Count == 0)
                        this.AllProjects = new ObservableCollection<Project>();
                }
            }
        }



        private RelayCommand _changeCompanyCommand;
        public ICommand ChangeCompanyCommand
        {
            get
            {
                if (_changeCompanyCommand == null)
                {
                    _changeCompanyCommand = new RelayCommand(
                        param => this.ChangeCompanyCommandExecute(),
                        param => AddProjectCommandCanExecute); // (umesto CanExecute)
                }
                return _changeCompanyCommand;
            }
        }
        void ChangeCompanyCommandExecute()
        {
            ChangeCompany chproj = new ChangeCompany(GridSelectedItem);
            chproj.Show();
            
            chproj.Closing += Window_Closing2;
        }
        private void Window_Closing2(object sender, CancelEventArgs e)
        {
            var prom = AllCompanies.IndexOf(GridSelectedItem);
            
            AllCompanies = new ObservableCollection<Company>(db.Company);

            GridSelectedItem = AllCompanies[prom];
            if (GridSelectedItem != null)
            {
                using (ProjectRepository pr2 = new ProjectRepository())
                {
                    IList<Project> variable = pr2.GetProjects().Where(i => i.FKCompanyId == GridSelectedItem.CompanyId).ToList();
                    this.AllProjects = new ObservableCollection<Project>(variable);
                }
                //this.AllProjects = new ObservableCollection<Project>(db.Project.Where(i => i.FKCompanyId == GridSelectedItem.CompanyId));
            }
        }

        private RelayCommand _addProjectCommand;
        public ICommand AddProjectCommand
        {
            get
            {
                if (_addProjectCommand == null)
                {
                    _addProjectCommand = new RelayCommand(
                        param => this.AddProjectCommandExecute(),
                        param => this.AddProjectCommandCanExecute);// (umesto CanExecute)
                }
                return _addProjectCommand;
            }
        }
        void AddProjectCommandExecute()
        {
            ObservableCollection<Company> gridGridSelectedCollection = new ObservableCollection<Company>();
            gridGridSelectedCollection.Add(GridSelectedItem);
            AddProject window = new AddProject(AllProjects, gridGridSelectedCollection);          
            window.Show();
            //window.Closing += Window_Closing3;          
        }
        private void Window_Closing3(object sender, CancelEventArgs e)
        {
            //GridSelectedItem.AppropriateProjects.Add(AllProjects[AllProjects.Count-1]);
        }
        public bool AddProjectCommandCanExecute
        {
            get
            {
                if (!(this.GridSelectedItem is Company))
                    return false;

                return true;
            }          
        }




        private RelayCommand _removeProjectCommand;
        public ICommand RemoveProjectCommand
        {
            get
            {
                if (_removeProjectCommand == null)
                {
                    _removeProjectCommand = new RelayCommand(
                        param => this.RemoveProjectCommandExecute(),
                        param => this.RemoveProjectCommandCanExecute);// (umesto CanExecute)
                }
                return _removeProjectCommand;
            }
        }
        void RemoveProjectCommandExecute()
        {
            var query = db.DataEntry.FirstOrDefault(x => x.ProjectId == ProjectGridSelectedItem.ProjectId);
            if (query != null)
            {
                MessageBox.Show("Projekat se nalazi u Unosu na pocetnoj strani, molimo obrisite ga prvo odatle.");
            }
            else
            {
                MessageBoxResult m = MessageBox.Show(String.Format("Da li ste sigurni da zelite da obrisete Projekat: {0}?", ProjectGridSelectedItem.TitleProject), "Obrisi Projekat", MessageBoxButton.YesNoCancel);

                if (m == MessageBoxResult.Yes)
                {
                    //db.Project.Attach(ProjectGridSelectedItem);                    
                    //db.Project.Remove(ProjectGridSelectedItem);
                    
                    var variable = db.Project.Where(q => q.ProjectId == ProjectGridSelectedItem.ProjectId).Select(q => q);
                    foreach (var item in variable)
                    {
                        db.Project.Remove(item);
                    }
                    db.SaveChanges();

                    AllProjects.Remove(ProjectGridSelectedItem);
                }
            }
        }

        public bool RemoveProjectCommandCanExecute
        {
            get
            {
                if (!(this.ProjectGridSelectedItem is Project))
                    return false;

                return true;
            }
        }

        private RelayCommand _changeProjectCommand;
        public ICommand ChangeProjectCommand
        {
            get
            {
                if (_changeProjectCommand == null)
                {
                    _changeProjectCommand = new RelayCommand(
                        param => this.ChangeProjectCommandExecute(),
                        param => this.ChangeProjectCommandCanExecute);// (umesto CanExecute)
                }
                return _changeProjectCommand;
            }
        }
        void ChangeProjectCommandExecute()
        {
            ChangeProject window = new ChangeProject(AllProjects, ProjectGridSelectedItem);
            window.Show();
            window.Closing += Window_Closing;
           
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            using (ProjectRepository pr2 = new ProjectRepository())
            {
                IList<Project> variable = pr2.GetProjects().Where(i => i.FKCompanyId == GridSelectedItem.CompanyId).ToList();
                this.AllProjects = new ObservableCollection<Project>(variable);
            }
        }
        public bool ChangeProjectCommandCanExecute
        {
            get
            {
                if (!(this.ProjectGridSelectedItem is Project))
                    return false;

                return true;
            }
        }

        public Action CloseAction { get; set; } // SET uradjen u backend kodu.
        private RelayCommand _closeWindowCommand;
        public ICommand CloseWindowCommandCommand
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
