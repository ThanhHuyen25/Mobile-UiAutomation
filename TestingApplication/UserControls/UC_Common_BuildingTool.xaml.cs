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
using System.Windows.Shapes;

namespace TestingApplication
{
    /// <summary>
    /// Interaction logic for UC_Common_BuildingTool.xaml
    /// </summary>
    public partial class UC_Common_BuildingTool : System.Windows.Controls.UserControl, IComponentState
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string buildingToolPath;
        protected bool isChanged;

        public UC_Common_BuildingTool(string path) : base()
        {
            InitializeComponent();
            this.buildingToolPath = path;
        }

        public string BuildingToolPath
        {
            get { return buildingToolPath; }
            set { buildingToolPath = value; }
        }

        #region control's event
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txt_building_tool_path.Text = buildingToolPath;
        }
        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.Title = "Select building tool path";
            fileBrowser.Filter = "Execute file|*.exe";
            fileBrowser.RestoreDirectory = true;
            if (fileBrowser.ShowDialog() == DialogResult.OK)
            {
                txt_building_tool_path.Text = fileBrowser.FileName;
                //store as an running path.
                OptionsWindow.dicTempPath[OptionsWindow.COMMON_BUILDING_TOOL] = fileBrowser.FileName;
                isChanged = true;
                //buildingToolPath = folderBrowser.SelectedPath;
            }
        }
        #endregion end control's event

        #region state
        public bool Save()
        {
            isChanged = false;
            if (txt_building_tool_path.Text != null)
            {
                if (!File.Exists(txt_building_tool_path.Text))
                {
                    logger.Error("Invalid path: " + txt_building_tool_path.Text);
                    System.Windows.MessageBox.Show(
                        "The path: " + txt_building_tool_path.Text + " isn't exist!",
                        "Invalid path",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    Properties.Settings.Default.vs_msbuild_path = txt_building_tool_path.Text;
                    Properties.Settings.Default.Save();
                    return true;
                }
            }
            else
            {
                logger.Error("Current buiding path is null");
                return false;
            }
        }

        public bool Discard()
        {
            isChanged = false;
            buildingToolPath = Properties.Settings.Default.vs_msbuild_path;
            txt_building_tool_path.Text = Properties.Settings.Default.vs_msbuild_path;
            return true;
        }

        public bool IsChanged()
        {
            return isChanged;
        }
        #endregion end state
    }
}
