using CompanyProjects.DataAccess;
using CompanyProjects.Model;
using MvvmPassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CompanyProjects.ViewModel
{
    public class ChangeDetailsAndPasswordViewModel : ViewModelBase
    {
        CompanyDataContext db;
        User LoggedUserVM = null;

        public ChangeDetailsAndPasswordViewModel(User LoggedUser)
        {
            db = new CompanyDataContext();
            RegistrationCommand = new RelayCommand(RegistrationCommandExecute);

            LoggedUserVM = LoggedUser;
            NameRegistration = LoggedUser.Name;
            UsernameRegistration = LoggedUser.Username;
            Email = LoggedUser.Email;
        }

        #region Properties  

        private string _nameRegistration;
        public string NameRegistration
        {
            get
            {
                return _nameRegistration;
            }
            set
            {
                if (value != _nameRegistration)
                {
                    _nameRegistration = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _usernameRegistration;
        public string UsernameRegistration
        {
            get
            {
                return _usernameRegistration;
            }
            set
            {
                if (value != _usernameRegistration)
                {
                    _usernameRegistration = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (value != _email)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion


        #region Commands            

        private string ConvertToUnsecureString(System.Security.SecureString securePassword)
        {
            if (securePassword == null)
            {
                return string.Empty;
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public ICommand RegistrationCommand
        {
            get;
            private set;
        }
        private void RegistrationCommandExecute(object parameter)
        {
            var passwordContainer = parameter as IHavePassword;
            if (passwordContainer != null)
            {
                var secureString = passwordContainer.Password;
                string PasswordInVM = ConvertToUnsecureString(secureString);
                var secureString2 = passwordContainer.Password2;
                string PasswordInVMRepeat = ConvertToUnsecureString(secureString2);
                //MessageBox.Show(PasswordInVMRepeat);
                try
                {
                    if (!String.IsNullOrEmpty(PasswordInVM))
                    {
                        if (PasswordInVM.Equals(PasswordInVMRepeat))
                        {
                            if (!String.IsNullOrEmpty(UsernameRegistration) && !String.IsNullOrEmpty(NameRegistration) && !String.IsNullOrEmpty(Email))
                            {                                                                
                                    LoggedUserVM.Password = GetSHA512(PasswordInVM);
                                    try
                                    {
                                        var userDB = db.User.Where(x => x.Username == LoggedUserVM.Username).SingleOrDefault();
                                        userDB.Password = LoggedUserVM.Password;
                                        userDB.Email = Email;
                                        userDB.Name = NameRegistration;
                                        try
                                        {
                                            db.SaveChanges();
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.ToString());
                                        }
                                        MessageBoxResult m = MessageBox.Show(String.Format("Promena podataka/lozinke uspesno obavljena"), "Promena podataka/lozinke", MessageBoxButton.OK);

                                        CloseAction();
                                    }
                                    catch
                                    {
                                        MessageBox.Show("Neuspesna Promena podataka/lozinke! Pokusajte ponovo");
                                    }                                 
                            }
                            else
                            {
                                MessageBox.Show("Niste popunili sva polja!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Lozinke moraju biti iste!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sifre se razlikuju");
                    }
                }
                catch { }
            }
        }
        private static string GetSHA512(string String)
        {

            UnicodeEncoding ue = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = ue.GetBytes(String);

            SHA512Managed hashString = new SHA512Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);

            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }

            return hex;
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
                        param => this.ZatvoriProzorCommandExecute(),
                        param => true);
                }
                return _closeWindowCommand;
            }
        }

        void ZatvoriProzorCommandExecute()
        {
            CloseAction();
        }
        #endregion
    }
}
