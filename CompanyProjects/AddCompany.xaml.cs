using CompanyProjects.Model;
using CompanyProjects.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddCompany.xaml
    /// </summary>
    public partial class AddCompany : Window
    {
        public AddCompany(ObservableCollection<Company> AllCompanies)
        {
            InitializeComponent();
            var dodavanje = new AddCompanyViewModel(AllCompanies);
            this.DataContext = dodavanje; // ovde se daje DataContext

            if (dodavanje.CloseAction == null)
                dodavanje.CloseAction = new Action(() => this.Close());
        }
    }
}
