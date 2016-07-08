using CompanyProjects;
using CompanyProjects.ViewModel;
using CompanyProjects.DataAccess;
using CompanyProjects.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CompanyProjects.ViewModel
{
    class AddCompanyViewModel : ViewModelBase
    {
        readonly CompanyRepository _companyRepository;
        readonly CompanyDataContext _repository;
        public ObservableCollection<Company> AllCompaniesCurrent;

        public AddCompanyViewModel(ObservableCollection<Company> AllCompanies)
        {
            AllCompaniesCurrent = AllCompanies;
            _companyRepository = new CompanyRepository();
            _repository = new CompanyDataContext();
        }

        #region Properties
        private string _titleCompany;
        public string TitleCompany
        {
            get
            {
                return _titleCompany;
            }
            set
            {
                if (value != _titleCompany)
                {
                    _titleCompany = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion


        #region Commands

        private RelayCommand _saveCompanyCommand;
        public ICommand SaveCompanyCommand
        {
            get
            {
                if (_saveCompanyCommand == null)
                {
                    _saveCompanyCommand = new RelayCommand(
                        param => this.SaveCompanyCommandExecute(),
                        param => SaveOtpremnicaCommandCanExecute);// this.SaveOtpremnicaCommandCanExecute);
                }
                return _saveCompanyCommand;
            }
        }

        void SaveCompanyCommandExecute()
        {
            Company cmp = new Company();
            cmp.TitleCompany = TitleCompany;
            cmp.AppropriateProjects = new List<Project>();
            
            AllCompaniesCurrent.Add(cmp);          
            _companyRepository.AddCompany(cmp);
            _repository.Company.Attach(cmp);

            CloseAction();
        }

        public bool SaveOtpremnicaCommandCanExecute
        {
            get
            {
                if (String.IsNullOrEmpty(this.TitleCompany))
                    return false;
              
                return true;
            }
        }



        public Action CloseAction { get; set; } // SET uradjen u backend kodu.
        private RelayCommand _zatvoriProzorCommand;
        public ICommand ZatvoriProzorCommand
        {
            get
            {
                if (_zatvoriProzorCommand == null)
                {
                    _zatvoriProzorCommand = new RelayCommand(
                        param => this.ZatvoriProzorCommandExecute(),
                        param => true);
                }
                return _zatvoriProzorCommand;
            }
        }      

        void ZatvoriProzorCommandExecute()
        {
            CloseAction();
        }
        #endregion
    }
}