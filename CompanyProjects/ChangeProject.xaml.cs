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
    /// Interaction logic for ChangeProject.xaml
    /// </summary>
    public partial class ChangeProject : Window
    {
        public ChangeProject(ObservableCollection<Project> AllProjects, Project ProjectGridSelectedItem)
        {
            InitializeComponent();

            var variable = new ChangeProjectViewModel(AllProjects, ProjectGridSelectedItem);
            this.DataContext = variable; // ovde se daje DataContext

            if (variable.CloseAction == null)
                variable.CloseAction = new Action(() => this.Close());
        }
    }
}
