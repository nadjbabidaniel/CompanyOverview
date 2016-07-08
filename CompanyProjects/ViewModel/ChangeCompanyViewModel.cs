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
    class ChangeCompanyViewModel : ViewModelBase
    {
        
        //Company CurrentGridSelectedItem;
        readonly CompanyRepository _companyRepository;
        readonly DataEntryRepository _dataEntryRepository;


        public ChangeCompanyViewModel(Company GridSelectedItem)
        {
            _companyRepository = new CompanyRepository();
            _dataEntryRepository = new DataEntryRepository();

           
            CurrentGridSelectedItem = GridSelectedItem;
            TitleCompany = GridSelectedItem.TitleCompany;
        }

        #region Properties
        private Company _currentGridSelectedItem;
        public Company CurrentGridSelectedItem
        {
            get
            {
                return _currentGridSelectedItem;
            }
            set
            {
                if (value != _currentGridSelectedItem)
                {
                    _currentGridSelectedItem = value;
                    OnPropertyChanged();
                }
            }
        }

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

        #region Comands

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
            CurrentGridSelectedItem.TitleCompany = TitleCompany;

            if(_companyRepository.UpdateComponent(CurrentGridSelectedItem))
            {
                _dataEntryRepository.UpdateDataEntryCompanyName(CurrentGridSelectedItem);
            }            
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
