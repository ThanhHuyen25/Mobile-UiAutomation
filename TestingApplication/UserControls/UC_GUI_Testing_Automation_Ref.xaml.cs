using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace TestingApplication
{
    /// <summary>
    /// Interaction logic for UC_GUI_Testing_Automation_Ref.xaml
    /// </summary>
    public partial class UC_GUI_Testing_Automation_Ref : System.Windows.Controls.UserControl, IComponentState
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string refPath;
        protected bool isChanged;

        public UC_GUI_Testing_Automation_Ref(string path) : base()
        {
            InitializeComponent();
            this.refPath = path;
        }

        public string RefPath
        {
            get { return refPath; }
            set { refPath = value; }
        }

        #region control's event
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txt_GUI_Testing_Automation_ref_path.Text = refPath;
        }
        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.Title = "Select GUI_Testing_Automation reference path";
            fileBrowser.Filter = "|GUI_Testing_Automation.dll|All files|*.*";
            fileBrowser.RestoreDirectory = true;
            if (fileBrowser.ShowDialog() == DialogResult.OK)
            {
                txt_GUI_Testing_Automation_ref_path.Text = fileBrowser.FileName;
                //store as an running path.
                OptionsWindow.dicTempPath[OptionsWindow.PATH_GUI_TESTING_AUTOMATION_REF] = fileBrowser.FileName;
                isChanged = true;
                //referencePath = folderBrowser.SelectedPath;
            }
        }
        #endregion end control's event

        #region state
        public bool Save()
        {
            isChanged = false;
            if (txt_GUI_Testing_Automation_ref_path.Text != null)
            {
                if (!File.Exists(txt_GUI_Testing_Automation_ref_path.Text))
                {
                    logger.Error("Invalid path: " + txt_GUI_Testing_Automation_ref_path.Text);
                    System.Windows.MessageBox.Show(
                        "The path: " + txt_GUI_Testing_Automation_ref_path.Text + " isn't exist!",
                        "Invalid path",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    Properties.Settings.Default.path_GUI_Testing_Automation_ref = txt_GUI_Testing_Automation_ref_path.Text;
                    Properties.Settings.Default.Save();
                    return true;
                }
            }
            else
            {
                logger.Error("Current Gui_Testing_Automation path is null");
                return false;
            }
        }

        public bool Discard()
        {
            isChanged = false;
            refPath = Properties.Settings.Default.path_GUI_Testing_Automation_ref;
            txt_GUI_Testing_Automation_ref_path.Text = Properties.Settings.Default.path_GUI_Testing_Automation_ref;
            return true;
        }

        public bool IsChanged()
        {
            return isChanged;
        }
        #endregion end state
    }
}
