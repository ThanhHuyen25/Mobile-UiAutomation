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

namespace TestingApplication
{
    /// <summary>
    /// Interaction logic for ChoiceWindow.xaml
    /// </summary>
    public partial class ChoiceWindow : Window, ICloseWindowNotify
    {
        private ISelectedDeviceNotify notify;
        // close
        private SelectDevice selectDevice;
        private ChoiceWindow()
        {
            InitializeComponent();
        }
        

        public ISelectedDeviceNotify Notify { get => notify; set => notify = value; }


        public ChoiceWindow(ISelectedDeviceNotify notify) : this()
        {
            this.notify = notify;
        }

        
        // close
        public void CloseWindow()
        {
            this.Close();
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
            // chuyen cua so
            // close
            new SelectDevice(notify, this).Show();
        }
    }

    // close
    public interface ICloseWindowNotify
    {
        void CloseWindow();
    }
}
