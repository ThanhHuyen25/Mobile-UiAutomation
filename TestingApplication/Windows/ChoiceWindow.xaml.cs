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

namespace TestingApplication.Windows
{
    /// <summary>
    /// Interaction logic for ChoiceWindow.xaml
    /// </summary>
    public partial class ChoiceWindow : Window
    {
        public ChoiceWindow()
        {
            InitializeComponent();
        }
        private void BtnWeb_Click(object render, RoutedEventArgs e)
        {
            this.Title = "Clicked";
           
            
        }
        private void BtnDesk_Click(object render, RoutedEventArgs e)
        {
            this.Title = "Clicked";
        }
        private void BtnAndroi_Click(object render, RoutedEventArgs e)
        {
            this.Title = "Success!!!";
            new SelectDevice().Show(); //chuyen cua so
        }

    }
}
