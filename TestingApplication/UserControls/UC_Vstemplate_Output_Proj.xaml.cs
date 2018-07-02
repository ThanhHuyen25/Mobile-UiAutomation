using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace TestingApplication
{
    /// <summary>
    /// Interaction logic for UC_vstemplate_output_proj.xaml
    /// </summary>
    public partial class UC_Vstemplate_Output_Proj : System.Windows.Controls.UserControl, IComponentState
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string vstemplatePath;
        protected bool isChanged;

        public UC_Vstemplate_Output_Proj(string path) : base()
        {
            InitializeComponent();
            this.vstemplatePath = path;
        }

        public string RefPath
        {
            get { return vstemplatePath; }
            set { vstemplatePath = value; }
        }

        #region control's event
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txt_vstemplate_proj.Text = vstemplatePath;
        }
        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.Title = "Select Vstemplate Output Project Path";
            fileBrowser.Filter = "proj template|*.vstemplate|All files|*.*";
            fileBrowser.RestoreDirectory = true;
            if (fileBrowser.ShowDialog() == DialogResult.OK)
            {
                txt_vstemplate_proj.Text = fileBrowser.FileName;
                //store as an running path.
                OptionsWindow.dicTempPath[OptionsWindow.PATH_TO_VSTEMPLATE_OUTPUT_PROJ] = fileBrowser.FileName;
                isChanged = true;
                //referencePath = folderBrowser.SelectedPath;
            }
        }
        #endregion end control's event

        #region state
        public bool Save()
        {
            isChanged = false;
            if (txt_vstemplate_proj.Text != null)
            {
                if (!File.Exists(txt_vstemplate_proj.Text))
                {
                    logger.Error("Invalid path: " + txt_vstemplate_proj.Text);
                    System.Windows.MessageBox.Show(
                        "The path: " + txt_vstemplate_proj.Text + " isn't exist!",
                        "Invalid path",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    Properties.Settings.Default.path_to_vstemplate_output_proj = txt_vstemplate_proj.Text;
                    Properties.Settings.Default.Save();
                    return true;
                }
            }
            else
            {
                logger.Error("Current path is null");
                return false;
            }
        }

        public bool Discard()
        {
            isChanged = false;
            vstemplatePath = Properties.Settings.Default.path_to_vstemplate_output_proj;
            txt_vstemplate_proj.Text = Properties.Settings.Default.path_to_vstemplate_output_proj;
            return true;
        }

        public bool IsChanged()
        {
            return isChanged;
        }
        #endregion end state
    }
}
