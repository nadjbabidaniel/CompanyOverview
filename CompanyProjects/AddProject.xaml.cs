﻿using CompanyProjects.Model;
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
    /// Interaction logic for AddProject.xaml
    /// </summary>
    public partial class AddProject : Window
    {
        public AddProject(ObservableCollection<Project> AllProjects, ObservableCollection<Company> gridGridSelectedCollection)
        {
            InitializeComponent();

            var variable = new AddProjectViewModel(AllProjects, gridGridSelectedCollection);
            this.DataContext = variable; // ovde se daje DataContext

            if (variable.CloseAction == null)
                variable.CloseAction = new Action(() => this.Close());
        }
    }
}
