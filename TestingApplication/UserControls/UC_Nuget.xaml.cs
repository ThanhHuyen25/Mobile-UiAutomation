// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 10:03 AM 2018/3/13
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestingApplication
{
    /// <summary>
    /// Interaction logic for UC_Nuget.xaml
    /// </summary>
    public partial class UC_Nuget : System.Windows.Controls.UserControl, IComponentState
    {
        private bool isChanged;
        private string nugetFilePath;

        public bool IsChanged() { return isChanged; }
        protected string NugetFilePath
        {
            get
            {
                return nugetFilePath;
            }
        }

        public UC_Nuget()
        {
            InitializeComponent();
        }

        public UC_Nuget(string path) : base()
        {
            InitializeComponent();
            this.nugetFilePath = path;
        }

        #region control's event
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txt_nuget_file_path.Text = nugetFilePath;
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.Title = "Select nuget.exe file";
            fileBrowser.Filter = "Execute file|*.exe";
            fileBrowser.RestoreDirectory = true;
            if (fileBrowser.ShowDialog() == DialogResult.OK)
            {
                txt_nuget_file_path.Text = fileBrowser.FileName;
                //store as an running path.
                OptionsWindow.dicTempPath[OptionsWindow.NUGET_FILE_PATH] = fileBrowser.FileName;
                isChanged = true;
                //buildingToolPath = folderBrowser.SelectedPath;
            }
        }
        #endregion end control's event

        #region state
        public bool Save()
        {
            isChanged = false;
            string path = txt_nuget_file_path.Text;
            if (path != null)
            {
                if (!File.Exists(path))
                {
                    System.Windows.MessageBox.Show(
                        "The path: " + path + " isn't exist!",
                        "Invalid path",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    Properties.Settings.Default.nuget_path = txt_nuget_file_path.Text;
                    Properties.Settings.Default.Save();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public bool Discard()
        {
            isChanged = false;
            nugetFilePath = Properties.Settings.Default.nuget_path;
            txt_nuget_file_path.Text = Properties.Settings.Default.nuget_path;
            return true;
        }
        #endregion
    }
}