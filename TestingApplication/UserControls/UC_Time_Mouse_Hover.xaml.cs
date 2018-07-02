using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace TestingApplication
{
    /// <summary>
    /// Interaction logic for UC_TimeMouseHover.xaml
    /// </summary>
    public partial class UC_Time_Mouse_Hover : System.Windows.Controls.UserControl, IComponentState
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string refValue;
        protected bool isChanged;

        public UC_Time_Mouse_Hover(string path) : base()
        {
            InitializeComponent();
            this.refValue = path;
        }

        public string RefValue
        {
            get { return refValue; }
            set { refValue = value; }
        }

        #region control's event
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txt_time_mouse_hover_value.Text = refValue;            
        }      
        #endregion end control's event

        #region state
        public bool Save()
        {
            isChanged = false;
            if (txt_time_mouse_hover_value.Text != null)
            {
                int value;
                if (!int.TryParse(txt_time_mouse_hover_value.Text, out value))
                {
                    logger.Error("Invalid path: " + txt_time_mouse_hover_value.Text);
                    System.Windows.MessageBox.Show(
                        "The value: " + txt_time_mouse_hover_value.Text + " is invalid!",
                        "Invalid value",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    Properties.Settings.Default.time_mouse_hover = txt_time_mouse_hover_value.Text;
                    Properties.Settings.Default.Save();
                    return true;
                }
            }
            else
            {
                logger.Error("Current value is null");
                return false;
            }
        }

        public bool Discard()
        {
            isChanged = false;
            refValue = Properties.Settings.Default.time_mouse_hover;
            txt_time_mouse_hover_value.Text = refValue;
            return true;
        }

        public bool IsChanged()
        {
            return isChanged;
        }
        #endregion end state

        private void TextChangedEvent(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //store as an running value.
            refValue = txt_time_mouse_hover_value.Text;
            OptionsWindow.dicTempPath[OptionsWindow.TIME_MOUSE_HOVER] = txt_time_mouse_hover_value.Text;
        }
    }
}
