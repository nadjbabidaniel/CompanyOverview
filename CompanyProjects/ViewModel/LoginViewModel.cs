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
using System.Windows.Controls;
using System.Windows.Input;

namespace CompanyProjects.ViewModel
{
    class LoginViewModel : ViewModelBase
    {
        CompanyDataContext db;

        public LoginViewModel()
        {
            db = new CompanyDataContext();
            LoginCommand = new RelayCommand(Login);
            RegistrationCommand = new RelayCommand(RegistrationCommandExecute);
        }


        #region Properties
        private string _username;
        public string UsernameLogin
        {
            get
            {
                return _username;
            }
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }
        
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
        public ICommand LoginCommand
        {
            get;
            private set;
        }
        private void Login(object parameter)
        {
            var passwordContainer = parameter as IHavePassword;
            if (passwordContainer != null)
            {
                var secureString = passwordContainer.Password;
                string PasswordInVM = ConvertToUnsecureString(secureString);
                //MessageBox.Show(PasswordInVM);
                string pass = GetSHA512(PasswordInVM);
                User user = null;
                user = db.User.FirstOrDefault(i => i.Username == UsernameLogin);

                if (user != null)
                {
                    if (user.Password.Equals(pass))
                    {
                        MessageBox.Show("Uspesno ste se logovali");
                        MainWindow mw = new MainWindow();
                        mw.Show();
                        CloseAction();
                    }
                    else
                    {
                        MessageBox.Show("Pogresna sifra");
                    }
                }
                else
                {
                    MessageBox.Show("Ne postoji korisnik sa tim korisnickim imenom");
                }
            }                                     
        }
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
                //MessageBox.Show(PasswordInVM);
                var secureString2 = passwordContainer.Password2;
                string PasswordInVMRepeat = ConvertToUnsecureString(secureString2);
                //MessageBox.Show(PasswordInVMRepeat);

                try {
                    User user = new User();
                    user.Username = UsernameRegistration;
                    user.Name = NameRegistration;
                    user.Email = Email;
                    if (!String.IsNullOrEmpty(PasswordInVM))
                    {
                        if (PasswordInVM.Equals(PasswordInVMRepeat))
                        {
                            user.Password = GetSHA512(PasswordInVM);
                            User userReg = null;
                            userReg = db.User.FirstOrDefault(i => i.Username == UsernameRegistration);
                            if (userReg == null)
                            {
                                if (!String.IsNullOrEmpty(UsernameRegistration) && !String.IsNullOrEmpty(NameRegistration) && !String.IsNullOrEmpty(Email)) {
                                    try
                                    {
                                        db.User.Add(user);
                                        db.SaveChanges();

                                        MessageBox.Show("Registracija uspesno obavljena");
                                        MainWindow mw = new MainWindow();
                                        mw.Show();
                                        CloseAction();
                                    }
                                    catch
                                    {
                                        MessageBox.Show("Neuspesna registracija! Pokusajte ponovo");
                                        //user = null;
                                        //db.Dispose();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Niste uneli sva polja!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Postoji korisnik sa ovim Korisnickim imenom!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Sifre se razlikuju");
                        }
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




        private RelayCommand _registrationWindowCommand;
        public ICommand RegistrationWindowCommand
        {
            get
            {
                if (_registrationWindowCommand == null)
                {
                    _registrationWindowCommand = new RelayCommand(
                        param => this.RegistrationWindowCommandExecute(),
                        param => true);// this.SaveOtpremnicaCommandCanExecute);
                }
                return _registrationWindowCommand;
            }
        }
        void RegistrationWindowCommandExecute()
        {
            Registration window = new Registration();
            window.Show();
            CloseAction();
        }

        
        private RelayCommand _closeRegistrationWindow;
        public ICommand CloseRegistrationWindow
        {
            get
            {
                if (_closeRegistrationWindow == null)
                {
                    _closeRegistrationWindow = new RelayCommand(
                        param => this.CloseRegistrationWindowCommand(),
                        param => true);
                }
                return _closeRegistrationWindow;
            }
        }

        void CloseRegistrationWindowCommand()
        {
            Loging lg = new Loging();
            lg.Show();
            CloseAction();
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
