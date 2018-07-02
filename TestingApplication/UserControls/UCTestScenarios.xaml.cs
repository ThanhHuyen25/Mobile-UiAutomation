// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:21 PM 2018/3/27
using GUI_Testing_Automation;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestingApplication
{
    /// <summary>
    /// Display test scenarios
    /// </summary>
    public partial class UCTestScenarios : UserControl
    {
        private ObservableCollection<IScreen> screens;
        public ObservableCollection<IScreen> Screens
        {
            get { return screens; }
            set
            {
                screens = value;
            }
        }

        public UCTestScenarios()
        {
            InitializeComponent();
        }
        public UCTestScenarios(ObservableCollection<IScreen> listScreens)
        {
            InitializeComponent();
            MainGrid.DataContext = this;
            this.screens = listScreens;
        }
        public UCTestScenarios(List<SpecScreen> listScreens)
        {
            InitializeComponent();
            MainGrid.DataContext = this;
            this.screens = new ObservableCollection<IScreen>(listScreens);
        }

        public void LoadData(List<SpecScreen> listScreens)
        {
            MainGrid.DataContext = this;
            this.screens = new ObservableCollection<IScreen>(listScreens);
            tab_screens.ItemsSource = this.screens;
        }
    }
}