using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TestingApplication
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public static IDictionary<string, string> dicTempPath = new Dictionary<string, string>();

        public const string COMMON_BUILDING_TOOL = "common_building_tool";
        public const string PATH_GUI_TESTING_AUTOMATION_REF = "path_GUI_Testing_Automation_ref";
        public const string PATH_TO_VSTEMPLATE_OUTPUT_PROJ = "path_to_vstemplate_output_proj";
        public const string TIME_MOUSE_HOVER = "time_mouse_hover";
        public const string NUGET_FILE_PATH = "nuget_file_path";

        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<IComponentState> listComponentState = new List<IComponentState>();
        public OptionsWindow()
        {
            InitializeComponent();
            InitalizeDicTempPath();
        }

        private void InitalizeDicTempPath()
        {
            dicTempPath[COMMON_BUILDING_TOOL] = null;
            dicTempPath[PATH_GUI_TESTING_AUTOMATION_REF] = null;
            dicTempPath[PATH_TO_VSTEMPLATE_OUTPUT_PROJ] = null;
            dicTempPath[TIME_MOUSE_HOVER] = null;
            dicTempPath[NUGET_FILE_PATH] = null;
        }

        #region controls's event
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
        }
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            //save
            bool check = true;
            foreach (IComponentState state in listComponentState)
                check &= state.Save();
            if (check)
                this.Close();
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //discard

            this.Close();
        }

        private void Elements_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem item = (TreeViewItem)e.NewValue;
            object child = null;
            switch (item.Name)
            {
                case COMMON_BUILDING_TOOL:
                    child = Common_Building_Tool();
                    break;
                case PATH_GUI_TESTING_AUTOMATION_REF:
                    child = GUI_Testing_Automation_Ref();
                    break;
                case PATH_TO_VSTEMPLATE_OUTPUT_PROJ:
                    child = Vstemplate_Output_Proj();
                    break;
                case TIME_MOUSE_HOVER:
                    child = Time_Mouse_Hover();
                    break;
                case NUGET_FILE_PATH:
                    child = Nuget_File_Path();
                    break;
                default:
                    logger.Error("Not handled setting for item: " + item.Header);
                    break;
            }
            if (child != null)
            {
                SetContainerContent((UserControl)child);
                listComponentState.Add((IComponentState)child);
            }
        }
        #endregion end controls's event

        #region change menu setting
        private UserControl Common_Building_Tool()
        {
            string oldPath = dicTempPath[COMMON_BUILDING_TOOL] != null ?
                dicTempPath[COMMON_BUILDING_TOOL] 
                : (Properties.Settings.Default.vs_msbuild_path != null ?
                Properties.Settings.Default.vs_msbuild_path : String.Empty);

            UC_Common_BuildingTool item = new UC_Common_BuildingTool(oldPath);            

            return item;
        }

        private UserControl GUI_Testing_Automation_Ref()
        {
            string oldPath = dicTempPath[PATH_GUI_TESTING_AUTOMATION_REF] != null ?
                dicTempPath[PATH_GUI_TESTING_AUTOMATION_REF]
                : (Properties.Settings.Default.path_GUI_Testing_Automation_ref != null ?
                Properties.Settings.Default.path_GUI_Testing_Automation_ref : String.Empty);

            UC_GUI_Testing_Automation_Ref item = new UC_GUI_Testing_Automation_Ref(oldPath);            

            return item;
        }

        private UserControl Vstemplate_Output_Proj()
        {
            string oldPath = dicTempPath[PATH_TO_VSTEMPLATE_OUTPUT_PROJ] != null ?
                dicTempPath[PATH_TO_VSTEMPLATE_OUTPUT_PROJ]
                : (Properties.Settings.Default.path_to_vstemplate_output_proj != null ?
                Properties.Settings.Default.path_to_vstemplate_output_proj : String.Empty);

            UC_Vstemplate_Output_Proj item = new UC_Vstemplate_Output_Proj(oldPath);            

            return item;
        }

        private UserControl Time_Mouse_Hover()
        {
            string oldValue = dicTempPath[TIME_MOUSE_HOVER] != null ?
                dicTempPath[TIME_MOUSE_HOVER]
                : (Properties.Settings.Default.time_mouse_hover != null ?
                Properties.Settings.Default.time_mouse_hover : String.Empty);

            UC_Time_Mouse_Hover item = new UC_Time_Mouse_Hover(oldValue);           

            return item;
        }

        private UserControl Nuget_File_Path()
        {
            string oldValue = dicTempPath[NUGET_FILE_PATH] != null ?
                dicTempPath[NUGET_FILE_PATH]
                : (Properties.Settings.Default.nuget_path != null ?
                Properties.Settings.Default.nuget_path : String.Empty);

            UC_Nuget item = new UC_Nuget(oldValue);

            return item;
        }
        #endregion end change menu setting


        private void SetContainerContent(UserControl child)
        {
            gvSettingContainer.Children.Clear();
            gvSettingContainer.Children.Add(child);
        }
    }
}