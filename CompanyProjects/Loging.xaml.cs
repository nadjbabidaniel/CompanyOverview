using CompanyProjects.ViewModel;
using MvvmPassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CompanyProjects
{
    /// <summary>
    /// Interaction logic for Loging.xaml
    /// </summary>
    public partial class Loging : Window, IHavePassword
    {
        public Loging()
        {
            InitializeComponent();

            var variable = new LoginViewModel();
            this.DataContext = variable;

            if (variable.CloseAction == null)
                variable.CloseAction = new Action(() => this.Close());
        }
        public System.Security.SecureString Password
        {
            get
            {
                return txtPassword.SecurePassword;
            }
        }
        public System.Security.SecureString Password2
        {
            get
            {
                return null;
            }
        }
    }
}
