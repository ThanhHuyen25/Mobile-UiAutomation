// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:45 PM 2018/3/3
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TestingApplication
{
    /// <summary>
    /// Interaction logic for AppSelection.xaml
    /// </summary>
    public partial class AppSelection : Window
    {
        private List<AppRunning> runningApps = new List<AppRunning>();
        private AppRunning selectedItem;
        private ISelectedAppNotify noti;
        public AppSelection()
        {
            InitializeComponent();
            lvDataBinding.ItemsSource = runningApps;
        }

        public AppSelection(ISelectedAppNotify noti) : this()
        {
            this.noti = noti;
        }

        public ISelectedAppNotify Noti
        {
            get { return noti; }
            set
            {
                noti = value;
            }
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (selectedItem != null)
            {
                noti.AppSelectedCallBack(selectedItem);
            }
            else
            {
                System.Windows.MessageBox.Show(
                       "Please choose an application!",
                       "Inspect application",
                       MessageBoxButton.OK);
            }
        }

        private void lvDataBinding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = (sender as ListView).SelectedItem as AppRunning;
        }

        private void btnDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lvDoubleClick_Item(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            noti.AppSelectedCallBack(selectedItem);
        }

        //Shortcut for refreshing = Ctrl + R    
        //start
        public static RoutedCommand RefreshListAppCommand = new RoutedCommand("RefreshApps", typeof(MainWindow), null);

        void DoRefreshListApp(object sender, ExecutedRoutedEventArgs e)
        {
            RefreshListApp();
        }
        //end

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshListApp();
        }

        public void RefreshListApp()
        {
            UpdateRunningApps();
            lvDataBinding.Items.Refresh();
        }

        public void UpdateRunningApps()
        {
            //remove all old running apps.
            runningApps.Clear();

            Process[] processes = Process.GetProcesses();
            List<Process> listProcesses = Filter(processes);
            foreach (Process p in listProcesses)
            {
                ImageSource ico = GetIcon(p);
                var app = new AppRunning(ico, p.Id, p.MainWindowTitle);
                app.Process = p;
                runningApps.Add(app);
            }
        }

        private ImageSource GetIcon(Process p)
        {
            System.Drawing.Bitmap ico = null;
            try
            {
                ico = System.Drawing.Icon.ExtractAssociatedIcon(p.MainModule.FileName)
                    .ToBitmap();
            }
            catch (Exception ex)
            {                
            }
            
            if (ico == null)
            {
                ico = (Bitmap)Properties.Resources.default_windows_app;
            }

            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                  ico.GetHbitmap(),
                  IntPtr.Zero,
                  Int32Rect.Empty,
                  BitmapSizeOptions.FromEmptyOptions());
        }

        private List<Process> Filter(Process[] processes)
        {
            List<Process> re = new List<Process>();
            foreach (var p in processes)
            {
                if (!String.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowHandle.ToInt32() != 0)
                {
                    re.Add(p);
                }
            }
            return re;
        }

        /// <summary>
        /// When the window be focused
        /// </summary>       
        private void ActivatedEvent(object sender, EventArgs e)
        {
            RefreshListApp();
        }
    }
}
