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
using System.Windows.Threading;

namespace TestingApplication
{
    /// <summary>
    /// Interaction logic for ChoiceWindow.xaml
    /// </summary>
    public partial class ChoiceWindow : Window, ICloseWindowNotify, IProgressResultNoti
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
            //this.Title = "Success!!!";
            // chuyen cua so
            // close
            new SelectDevice(notify, this).Show();
           /* ProgressDialog exportCodesProgress = new ProgressDialog("Wait Choice", "Please wait!", this);
            exportCodesProgress.Show();
            System.Threading.Tasks.Task.Factory.StartNew(
            new Action(() =>
            {
                new SelectDevice(notify, this).Show();
                //update in UI Thread
                System.Windows.Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() =>
                {
                    exportCodesProgress.Close();
                    
                }));
            }));*/
        }

        public void OnSuccessful()
        {
            throw new NotImplementedException();
        }

        public void OnFailure()
        {
            throw new NotImplementedException();
        }

        public void OnCancel()
        {
            throw new NotImplementedException();
        }
    }

    // close
    public interface ICloseWindowNotify
    {
        void CloseWindow();
    }
}
