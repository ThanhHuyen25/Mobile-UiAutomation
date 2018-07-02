// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:10 PM 2018/3/6
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GUI_Testing_Automation;

namespace TestingReport
{
    /// <summary>
    /// Interaction logic for UCReportViewer.xaml
    /// </summary>
    public partial class UCReportViewer : UserControl
    {
        public UCReportViewer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// binding data to view
        /// </summary>
        /// <param name="viewModel"></param>
        public UCReportViewer(ViewModel viewModel)
        {
            //DataContext = viewModel;
            InitializeComponent();
            var report = viewModel.TestingReportModel;
            report.Convert();
            tvMain.ItemsSource = report.ListRoots;
            // wait until we're initialized to handle events
            viewModel.ViewModelChanged += new EventHandler(ViewModelChanged);
        }

        private void ViewModelChanged(object sender, EventArgs e)
        {
            // this gets called when the view model is updated because the Xml Document was updated
            // since we don't get individual PropertyChanged events, just re-set the DataContext
            ViewModel viewModel = DataContext as ViewModel;
            DataContext = null; // first, set to null so that we see the change and rebind
            DataContext = viewModel;
        }

        //testing
        public void Load()
        {
            var report = TestReportFileLoader.LoadFile(@"..\..\sample.guilog");
            report.Convert();
            tvMain.ItemsSource = report.ListRoots;
        }

        private void expander_Loaded(object sender, RoutedEventArgs e)
        {
            var tmp = VTHelper.FindChild<ContentPresenter>(sender as Expander);
            if (tmp != null)
            {
                tmp.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            }
        }

        private void btnViewFullSizeImg_Click(object sender, RoutedEventArgs e)
        {
            var image = (sender as Button).Content as Image;
            var source = image.Source as BitmapFrame;
            string filePath = source.Decoder.ToString();
            if (filePath.StartsWith(@"file:///"))
                filePath = filePath.Substring(@"file:///".Length);
            System.Diagnostics.Process.Start(filePath);
        }
    }

    public static class VTHelper
    {
        public static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            T childElement = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;
                if (childType == null)
                {
                    childElement = FindChild<T>(child);
                    if (childElement != null)
                        break;
                }
                else
                {
                    childElement = (T)child;
                    break;
                }
            }
            return childElement;
        }
    }
}
