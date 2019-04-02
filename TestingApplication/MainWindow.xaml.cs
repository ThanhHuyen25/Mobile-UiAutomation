// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:18 AM 2017/10/5
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using GUI_Testing_Automation;
using log4net;
using System.Windows.Automation;
using System.Windows.Threading;
using System.Windows.Media;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Gma.System.MouseKeyHook;
using System.Threading;
using System.ComponentModel;
using System.Threading.Tasks;

namespace TestingApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// IMPORTANT NOTE: PUT FUNCTION INTO CORRECT REGION
    /// </summary>
    public partial class MainWindow : Window, INewElementAddedNotify, ISelectedAppNotify, IProgressResultNoti, ISelectedDeviceNotify
    {
        #region attributes
        //------------------------------------------------------
        //
        //  Shortcut InputBindings
        //
        //------------------------------------------------------
        public static RoutedCommand NewInspectCommand = new RoutedCommand("NewInspect", typeof(MainWindow), null);
        public static RoutedCommand SaveCommand = new RoutedCommand("NewSave", typeof(MainWindow), null);
        public static RoutedCommand ExportCodeCommand = new RoutedCommand("NewExportCode", typeof(MainWindow), null);
        public static RoutedCommand CloseCommand = new RoutedCommand("NewClose", typeof(MainWindow), null);
        public static RoutedCommand OpenAppSelection = new RoutedCommand("OpenAppSelection", typeof(MainWindow), null);

        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SOURCE_FOLDER_EXTRACT = "projects_extract";
        private const string IMAGE_NOT_AVAILABLE_MSG = "Image not available";

        // time hover to find element (mili-second)
        private const int TIME_MOUSE_HOVER = 1500;
        //private const int TIME_MOUSE_HOVER = 3000;

        private const bool ENABLE_HOVER_HELPER = true;

        private ElementDiscoverManual elementDiscorerManual;

        private DispatcherTimer timer;
        private string excel_spec_file_path;
        private string app_under_test_path;
        private List<SpecScreen> listScreens;
        private string ranorex_repo_file_path;
        private ScriptType scriptType = ScriptType.Normal;

        private AppSelection appSelection;
        //callback
        private SelectDevice selectDevice;
        private const string DEFAULT_NULL_ELEMENT_ATTRIBUTE = "";

        // store all log to notify to user
        private MyLog myLog;
        #endregion attributes

        public MainWindow()
        {
            // TODO: wait validate, UI thread
            log4net.Config.XmlConfigurator.Configure();
            InitializeComponent();
            // enable this when release product
            Release();
            MyInit();

            //List<IElement> elements = new AndroidAdbDumpFileParser().Parse("C:/ProgramData/window_dump.xml");
            //SelectedDeviceCallBack(elements);
            // for debugging
            // DefaultLoad();
            //Testing();
            //TestingMultipleScreens();
        }


        private void MyInit()
        {
            // set location for popup dialog
            popupDialog.VerticalOffset = SystemParameters.WorkArea.Height - popupDialog.Height - 3;
            popupDialog.HorizontalOffset = SystemParameters.WorkArea.Width - popupDialog.Width - 5;
            popupDialog.IsOpen = false;
            myLog = new MyLog();
            elementDiscorerManual = new ElementDiscoverManual(this);
            HandleHoverHelper();

            // restore settings
            int p1 = (int)Math.Round(Properties.Settings.Default.divide1_per * 100);
            ColDef1.Width = new GridLength(p1, GridUnitType.Star);
            ColDef2.Width = new GridLength(100 - p1, GridUnitType.Star);
        }

        #region inspect app
        /// <summary>
        /// select new file to inspect (.zip or .exe)
        /// </summary>
        private void ChooseNewFileUI()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open program";
            openFileDialog.Filter = "Execute file|*.exe|Zip files|*.zip|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // inspecting screen to get all elements
                string fileToOpen = openFileDialog.FileName;
                FileInfo file = new FileInfo(fileToOpen);
                logger.Debug("extension: " + file.Extension);
                if (file.Extension.Equals(".exe", StringComparison.OrdinalIgnoreCase))
                {
                    ResetAll();
                    try
                    {
                        ProcessWithExeFile(fileToOpen);
                    } catch (Exception)
                    {

                    }
                }
                else if (file.Extension.Equals(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    ResetAll();
                    ProcessWithZipFile(fileToOpen);
                }
                else
                    System.Windows.MessageBox.Show(
                        "Please choose a valid file with .zip or .exe extension",
                        "Invalid file type",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// after get exe file
        /// </summary>
        /// <param name="file2Open"></param>        
        private void ProcessWithExeFile(string file2Open)
        {
            //lastInspectType = 1;
            System.Windows.Application.Current.Dispatcher.BeginInvoke(
              //DispatcherPriority.Background,
              new Action(() =>
              {
                  popupTextNotify.Text = "Inspecting elements";
                  popupDialog.IsOpen = true;
              }));
            Task.Factory.StartNew(new Action(() =>
            {
                IInspecting inspecting = new InspectElement();
                AutomationElementCollection windows = inspecting.Inspect(file2Open);
                logger.Debug("Found " + windows.Count + " Screen(s)");
                DoAnalyze(windows);
                //Code to execute when Popup opens
                System.Windows.Application.Current.Dispatcher.BeginInvoke(
                    //DispatcherPriority.Background,
                    new Action(() =>
                    {
                        // analyzing
                        this.Activate();
                        popupDialog.IsOpen = false;
                    }));
            }));
        }

        /// <summary>
        /// when input is a zip project file
        /// </summary>
        /// <param name="zipFilePath"></param>
        private void ProcessWithZipFile(string zipFilePath)
        {
            //lastInspectType = 1;
            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() =>
                {
                    popupTextNotify.Text = "Extracting file!";
                    popupDialog.IsOpen = true;
                }));
            Task.Factory.StartNew(new Action(() =>
            {
                string projectExtractPath = Utils.CreateUniqueDirAndExtract(zipFilePath, Path.GetTempPath());
                AbstractProject abProject = ProjectLoader.Load(projectExtractPath);
                ProjectAnalyzer projectAnalyzer = new ProjectAnalyzer();
                System.Windows.Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() =>
                    {
                        popupTextNotify.Text = "Analyzing, modifying and rebuilding project";
                    }));
                Task.Factory.StartNew(new Action(() =>
                {
                    Tuple<string, LogProcess> pair = projectAnalyzer.Process(abProject);
                    string pathExeFile = pair.Item1;
                    if (pathExeFile == null)
                    {
                        // show error windows
                        ShowBuildingWindowsLog(pair.Item2);
                    }
                    else
                        ProcessWithExeFile(pathExeFile);
                }));
            }));
            
            
        }

        private void ProcessWithInstanceProcess(Process process)
        {
            //lastInspectType = 0;
            System.Windows.Application.Current.Dispatcher.BeginInvoke(
              //DispatcherPriority.Background,
              new Action(() =>
              {
                  popupTextNotify.Text = "Inspecting elements";
                  popupDialog.IsOpen = true;
              }));
            IInspecting inspecting = new InspectElement();
            AutomationElementCollection windows = inspecting.Inspect(process);
            logger.Debug("Found " + windows.Count + " Screen(s)");
            DoAnalyze(windows);
            //Code to execute when Popup opens
            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                //DispatcherPriority.Background,
                new Action(() =>
                {
                    this.Activate();
                    popupDialog.IsOpen = false;
                }));
        }

        /// <summary>
        /// after inspecting
        /// </summary>
        private void EnableUIElements()
        {
            imgSave.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/Save_40x.png"));
            imgClose.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/CloseDocument_32x.png"));
            imgExportCode.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/Export_30x.png"));
            menuItemSave.Icon = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/Save_40x.png")) };
            menuItemClose.Icon = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/CloseDocument_32x.png")) };
            menuItemExportCodes.Icon = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/Export_30x.png")) };
            menuItemImportSpec.Icon = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/Import_16x.png")) };
            imgImportCode.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/Import_16x.png"));
            imgExportSpec.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/Export_30x.png"));
            btnSave.IsEnabled = true;
            btnClose.IsEnabled = true;
            btnExportCode.IsEnabled = true;
            menuItemSave.IsEnabled = true;
            menuItemClose.IsEnabled = true;
            menuItemExportCodes.IsEnabled = true;
            menuItemImportSpec.IsEnabled = true;
            btnImportCode.IsEnabled = true;
            btnExportSpec.IsEnabled = true;
        }

        /// <summary>
        /// when close a project
        /// </summary>
        private void DisableUIElements()
        {
            imgSave.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/Save_Disable_40x.png"));
            imgClose.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/CloseDocument_Disable_32x.png"));
            imgExportCode.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/Export_Disable_30x.png"));
            menuItemSave.Icon = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/Save_Disable_40x.png")) };
            menuItemClose.Icon = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/CloseDocument_Disable_32x.png")) };
            menuItemExportCodes.Icon = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/Export_Disable_30x.png")) };
            menuItemImportSpec.Icon = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/Import_Disable_16x.png")) };
            imgImportCode.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/Import_Disable_16x.png"));
            imgExportSpec.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/Export_Disable_30x.png"));
            btnSave.IsEnabled = false;
            btnClose.IsEnabled = false;
            btnExportCode.IsEnabled = false;
            menuItemSave.IsEnabled = false;
            menuItemClose.IsEnabled = false;
            menuItemExportCodes.IsEnabled = false;
            menuItemImportSpec.IsEnabled = false;
            btnImportCode.IsEnabled = false;
            btnExportSpec.IsEnabled = false;
        }
        #endregion inspect app

        #region UI control events
        /// <summary>
        /// inspect app from an .exe file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //ISelectedDeviceNotify notify;
        private void NewInspection_Click(object sender, RoutedEventArgs e)
        {
            //ChooseNewFileUI();
            new ChoiceWindow(this).Show();
        }

        /// <summary>
        /// inspect instance app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInstanceInspect_Click(object sender, RoutedEventArgs e)
        {
            SelectRunningApps();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveAction();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            CloseAction();
        }

        private void ExportProject_Click(object sender, RoutedEventArgs e)
        {
            ExportProjectAction();
        }

        private void ImportRanorexRepoFile_Click(object sender, RoutedEventArgs e)
        {
            DoImportRanorexRepoFile();
        }

        private void ElementsTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!(e.NewValue is ElementItemVisual))
                return;
            //MakeContextMenu();
            // get current element clicked
            ElementItemVisual elementItemVisual = (ElementItemVisual)e.NewValue;
            IElement element = elementItemVisual.Element;

            if (element != null)
            {
                ShowInfo(element);
                //logger.Debug("Element " + element.Attributes.Name + "'s text: " + element.GetText());
                //(element as ElementBase).SetWidth(500);
            }
            else
            {
                logger.Error("Can not search element - " + e.NewValue.ToString());
            }
        }

        // custom event
        private void TreeViewSelectedItemChanged(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                item.BringIntoView();
                e.Handled = true;
            }
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            new OptionsWindow().Show();
        }

        private void TreeViewItem_Collapse_Click(object sender, RoutedEventArgs e)
        {
            //logger.Debug("Collapse");
            var selectedItem = elementsTreeView.SelectedItem as ElementItemVisual;
            selectedItem.IsExpanded = false;
            RefreshTreeView();
        }

        private void TreeViewItem_Expand_Click(object sender, RoutedEventArgs e)
        {
            //logger.Debug("Expand");
            var selectedItem = elementsTreeView.SelectedItem as ElementItemVisual;
            selectedItem.IsExpanded = true;
            RefreshTreeView();
        }

        private void TreeViewItem_Remove_Click(object sender, RoutedEventArgs e)
        {
            //logger.Debug("Remove");
            var selectedItem = elementsTreeView.SelectedItem as ElementItemVisual;
            selectedItem.Remove();
            RefreshTreeView();
        }

        private void TreeViewItem_CollapseAll_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = elementsTreeView.SelectedItem as ElementItemVisual;
            Collapse_All_Item(selectedItem);
            RefreshTreeView();
        }

        private void TreeViewItem_ExpandAll_Click(object sender, RoutedEventArgs e)
        {
            logger.Debug("Expand All");
            var selectedItem = elementsTreeView.SelectedItem as ElementItemVisual;
            Expand_All_Item(selectedItem);
            RefreshTreeView();
            //Properties.Settings.Default.vs_msbuild_path
        }

        /// <summary>
        /// import Excel specification file - scenarios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportSpecFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import excel specification file";
            openFileDialog.Filter = "Excel file|*.xlsx;*.xls";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            //logger.Info("mainWindows thread: " + Thread.CurrentThread.ManagedThreadId);
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                excel_spec_file_path = openFileDialog.FileName;
                ProgressDialog spec_analyzer_progress = new ProgressDialog("Analyzing spec", "Analyzing specification Excel file. Please wait!", this);
                spec_analyzer_progress.Show();

                Task.Factory.StartNew(new Action(() =>
                {
                    listScreens = HandleSpecExcelFile(
                        RuntimeInstance.listRootElement, excel_spec_file_path, app_under_test_path, myLog);
                    if (listScreens != null)
                        this.Dispatcher.Invoke((Action)delegate
                        {
                            spec_analyzer_progress.Close();
                            if (listScreens != null)
                                DisplayTestScenarios(listScreens);
                        });
                }));
            }
        }

        private void ExportSpec_Click(object sender, RoutedEventArgs e)
        {
            List<IElement> elementsToExport = new List<IElement>();
            var roots = elementsTreeView.ItemsSource as IEnumerable<ElementItemVisual>;
            if (roots != null)
                foreach (var root in roots)
                    RetrieveElements2ExportSpecRecursive(root, elementsToExport);
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "PictMaster-Template"; // Default file name
            dlg.DefaultExt = ".xls"; // Default file extension
            dlg.Filter = "Excel documents (.xls)|*.xls"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                PictMasterFileGeneration pictMasterFileGeneration = new PictMasterFileGeneration();

                ProgressDialog exportSpecProgress = new ProgressDialog("Exporting PictMaster Template", "Exporting Excel file, please wait!", this);
                exportSpecProgress.Show();
                System.Threading.Tasks.Task.Factory.StartNew(
                    new Action(() =>
                    {
                        pictMasterFileGeneration.Generate(filename,
                            new ListUIElements(ListElementsIndicator.AllElements, elementsToExport));
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(
                              DispatcherPriority.Background,
                           new Action(() =>
                           {
                               exportSpecProgress.Close();
                           }));
                        MessageBoxResult res = System.Windows.MessageBox.Show(
                                       "Exported successfully! Do you want to open this Excel file now?",
                                       "Export spec",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Information);
                        if (res == MessageBoxResult.Yes)
                        {
                            var psi = new System.Diagnostics.ProcessStartInfo(filename);
                            System.Diagnostics.Process.Start(psi);
                        }
                    }));

            }
        }

        /// <summary>
        /// add current element to export spec list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Add2Export_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = elementsTreeView.SelectedItem as ElementItemVisual;
            selectedItem.IsExportToSpec = true;
            RefreshTreeView();
        }

        /// <summary>
        /// add current element and all it's children to export spec list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_AddAll2Export_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = elementsTreeView.SelectedItem as ElementItemVisual;
            AddElementAndChildren2Spec(selectedItem);
            RefreshTreeView();
        }

        /// <summary>
        /// remove current element and all it's children to export spec list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_RemoveAll2Export_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = elementsTreeView.SelectedItem as ElementItemVisual;
            RemoveElementAndChildren2Spec(selectedItem);
            RefreshTreeView();
        }

        /// <summary>
        /// when move spliter -> store setting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spliter1_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            double p1 = ColDef1.ActualWidth;
            double p2 = ColDef2.ActualWidth;
            Properties.Settings.Default.divide1_per = p1 / (p1 + p2);
            Properties.Settings.Default.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // .dll path
            if (Properties.Settings.Default.path_GUI_Testing_Automation_ref == null)
            {
                string app_path = System.Reflection.Assembly.GetEntryAssembly().Location;
                string folder = new FileInfo(app_path).DirectoryName;
                string new_path = Path.Combine(folder, "GUI_Testing_Automation.dll");
                if (File.Exists(new_path))
                {
                    Properties.Settings.Default.path_GUI_Testing_Automation_ref = new_path;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    var result = System.Windows.MessageBox.Show(
                        "Not define library path yet! Please select path to GUI_Testing_Automation.dll file!",
                        "Choose library path",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Title = "Select GUI_Testing_Automation.dll file";
                        openFileDialog.Filter = "Dynamic library|*.dll";
                        openFileDialog.FilterIndex = 1;
                        openFileDialog.RestoreDirectory = true;
                        if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            Properties.Settings.Default.path_GUI_Testing_Automation_ref = openFileDialog.FileName;
                            Properties.Settings.Default.Save();
                        }
                    }
                }
            }
        }
        #endregion end UI control events

        #region common actions
        private void SaveAction()
        {
            // save project
        }

        private void CloseAction()
        {
            popupDialog.IsOpen = false;
        }

        private void ExportProjectAction()
        {
            //btnExportCode.IsEnabled = false;
            //imgExportCode.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/Export_Disable_30x.png"));            
            if (scriptType == ScriptType.Normal)
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "MyTestProject"; // Default file name

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    CheckParamsNull();
                    ProgressDialog exportCodesProgress = new ProgressDialog("Exporting project", "Exporting, please wait!", this);
                    exportCodesProgress.Show();
                    System.Threading.Tasks.Task.Factory.StartNew(
                        new Action(() =>
                        {
                            string selectedDir = dlg.FileName;
                            string projectName = System.IO.Path.GetFileName(dlg.FileName);
                            bool genResult = false, exception = false;
                            try
                            {
                                genResult = new ProjectGeneration().Generate(
                                    RuntimeInstance.listRootElement,
                                    listScreens,
                                    selectedDir,
                                    projectName,
                                    app_under_test_path,
                                    myLog);
                            }
                            catch (Exception e)
                            {
                                exception = true;
                                System.Windows.MessageBox.Show(
                                       e.Message,
                                       "Export project fail! ",
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Error);
                            }
                        //update in UI Thread
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(
                                  DispatcherPriority.Background,
                               new Action(() =>
                               {
                                   btnExportCode.IsEnabled = true;
                                   imgExportCode.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/icons/Export_30x.png"));
                                   exportCodesProgress.Close();
                                   if (genResult && !exception)
                                   {
                                       MessageBoxResult res = System.Windows.MessageBox.Show(
                                           "Exported successfully! Do you want to open project now?",
                                           "Export project",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Information);
                                       if (res == MessageBoxResult.Yes)
                                       {
                                           string newSolutionPath = Path.Combine(selectedDir, projectName + ".sln");
                                           var psi = new System.Diagnostics.ProcessStartInfo(newSolutionPath);
                                           System.Diagnostics.Process.Start(psi);
                                       }
                                   }
                                   else if (!genResult && !exception)
                                   {
                                       MessageBoxResult res = System.Windows.MessageBox.Show(
                                           "An error occured when exporting project",
                                           "Export project fail",
                                           MessageBoxButton.OK,
                                           MessageBoxImage.Error);
                                   }
                               }));
                        }));
                }
            }
            else
            {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string folderSelected = dialog.SelectedPath;
                    var genResult = new ProjectGeneration().GenerateRanorexProject(
                                    listRootElements : RuntimeInstance.listRootElement,
                                    screensExpanded : listScreens,
                                    repoFilePath : ranorex_repo_file_path,
                                    folderOutPath : folderSelected,
                                    appPath : app_under_test_path,
                                    myLog : myLog);
                }
            }
        }

        private void CheckParamsNull()
        {
            if (app_under_test_path == null)
            {
                MessageBoxResult res = System.Windows.MessageBox.Show(
                                       "System detected that you have not set app under test.\nDo you want to set right now?",
                                       "Set app test",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Information);
                if (res == MessageBoxResult.Yes)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Title = "Select app under test";
                    openFileDialog.Filter = "Execution file|*.exe";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;
                    //logger.Info("mainWindows thread: " + Thread.CurrentThread.ManagedThreadId);
                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        app_under_test_path = openFileDialog.FileName;
                    }
                }
            }
        }

        private void Expand_All_Item(ElementItemVisual element)
        {
            element.IsExpanded = true;
            if (element.Children == null)
            {
                return;
            }
            foreach (ElementItemVisual ele in element.Children)
            {
                Expand_All_Item(ele);
            }
        }

        private void ShowInfo(IElement element)
        {
            //logger.Debug("element " + element.Attributes.Name + "'s rect: (" +
            //element.Attributes.RectBounding.TopLeft + ", " +
            //element.Attributes.RectBounding.BottomRight + ")");
            ShowInfoPicture(element);
            ShowInfoProperties(element);
        }

        /// <summary>
        /// @author Nguyen
        /// </summary>
        /// <param name="element"></param>
        private void ShowInfoProperties(IElement element)
        {
            tb_value_Parent.Content = element.Parent == null ? "" : element.Parent.ToString();
            // tb_value_AcceleratorKey.Text = element.AcceleratorKey;
            //tb_value_DesignedId.Content = element.Attributes.DesignedId ?? DEFAULT_NULL_ELEMENT_ATTRIBUTE;
            tb_value_Children.Content = element.Children == null ? "" : element.Children.ToString();
            //tb_value_ElementPath.Content = element.Attributes.ElementPath == null ? "" : element.Attributes.ElementPath.ToString();
            //tb_value_ImageCaptureEncoded.Text = element.ImageCaptureEncoded;
            //tb_value_Acceleratorvalue.Content = element.Attributes.AcceleratorKey ?? DEFAULT_NULL_ELEMENT_ATTRIBUTE;
            //tb_value_Accessvalue.Content = element.Attributes.AccessKey ?? DEFAULT_NULL_ELEMENT_ATTRIBUTE;
            tb_value_ClassName.Content = element.Attributes.ClassName ?? DEFAULT_NULL_ELEMENT_ATTRIBUTE;
            tb_value_Index.Content = element.Attributes.Index;
            tb_value_Text.Content = element.Attributes.Text ?? DEFAULT_NULL_ELEMENT_ATTRIBUTE;
            tb_value_Package.Content = element.Attributes.Package ?? DEFAULT_NULL_ELEMENT_ATTRIBUTE;
            tb_value_ContentDesc.Content = element.Attributes.ContentDesc ?? DEFAULT_NULL_ELEMENT_ATTRIBUTE;
            tb_value_Clickable.Content = element.Attributes.Clickable;
            tb_value_Checkable.Content = element.Attributes.Checkable;
            tb_value_IsChecked.Content = element.Attributes.IsChecked;
            tb_value_Enabled.Content = element.Attributes.Enabled;
            tb_value_Focusable.Content = element.Attributes.Focusable;
            tb_value_Focused.Content = element.Attributes.Focused;
            tb_value_Scrollable.Content = element.Attributes.Scrollable;
            tb_value_LongClickable.Content = element.Attributes.LongClickable;
            tb_value_Password.Content = element.Attributes.Password == true ? "true" : "false";
            tb_value_Selected.Content = element.Attributes.Selected == true ? "true" : "false";
            tb_value_ResourceId.Content = element.Attributes.ResourceId ?? DEFAULT_NULL_ELEMENT_ATTRIBUTE;
            //tb_value_FrameworkId.Content = element.Attributes.FrameworkId ?? DEFAULT_NULL_ELEMENT_ATTRIBUTE;
            //tb_value_HasvalueboardFocus.Content = element.Attributes.HasKeyboardFocus == true ? "true" : "fasle";
            //tb_value_HelpText.Content = element.Attributes.HelpText ?? DEFAULT_NULL_ELEMENT_ATTRIBUTE;
            //tb_value_IsContentElement.Content = element.Attributes.IsContentElement == true ? "true" : "false";
            //tb_value_IsControlElement.Content = element.Attributes.IsControlElement == true ? "true" : "false";
            //tb_value_isenabled.content = element.attributes.isenabled == true ? "true" : "false";
            //tb_value_isvalueboardfocusable.content = element.attributes.iskeyboardfocusable == true ? "true" : "false";
            //tb_value_isoffscreen.content = element.attributes.isoffscreen == true ? "true" : "false";
            //tb_value_ispassword.content = element.attributes.ispassword == true ? "true" : "false";
            //tb_value_isrequiredforform.content = element.attributes.isrequiredforform == true ? "true" : "fasle";
            //tb_value_itemstatus.content = element.attributes.itemstatus ?? default_null_element_attribute;
            //tb_value_itemtype.content = element.attributes.itemtype ?? default_null_element_attribute;
            //tb_value_localizedcontroltype.content = element.attributes.localizedcontroltype;
            //tb_value_name.content = element.attributes.name ?? default_null_element_attribute;
            //tb_value_nativewindowhandle.content = element.attributes.nativewindowhandle.tostring();
            //tb_value_processid.content = element.attributes.processid.tostring();
            tb_value_Id.Content = element.Attributes.ResourceId ?? DEFAULT_NULL_ELEMENT_ATTRIBUTE;
            //tb_value_DesignedId.Content = element.Attributes.DesignedId ?? DEFAULT_NULL_ELEMENT_ATTRIBUTE;
            //tb_value_DesignedName.Content = element.Attributes.DesignedName ?? DEFAULT_NULL_ELEMENT_ATTRIBUTE;
            //tb_value_ElementType.Content = element.Attributes.ElementType ?? DEFAULT_NULL_ELEMENT_ATTRIBUTE;
            //tb_value_ResourceId.Content = element.Attributes.ResourceId;
            tb_value_Bound.Content = element.Attributes.RectBounding;
            tb_value_Xpath.Content = element.Attributes.Xpath;
            
        }

        /// <summary>
        /// Display image captured
        /// TODO: store windows bound, display "Image capture not available" when no img
        /// </summary>
        /// <param name="element"></param>
        private void ShowInfoPicture(IElement element)
        {
            string strEncoded = element.Attributes.ImageCaptureEncoded;
            System.Drawing.Rectangle screenSize = Screen.PrimaryScreen.Bounds;

            if (strEncoded != null && strEncoded != "")
            {
                noImage.Text = "";
                BitmapImage bitmap = GUI_Utils.DecodeBase64ToImageWithResizeImg(strEncoded,
                    screenSize.Width / 2 - 50,
                    screenSize.Height / 2 - 20);

                DrawingVisual dv = new DrawingVisual();
                int border_thickness = 4;
                using (DrawingContext dc = dv.RenderOpen())
                {
                    dc.DrawImage(bitmap, new Rect(border_thickness, border_thickness, bitmap.PixelWidth, bitmap.PixelHeight));
                    //dc.DrawRectangle(Brushes.Green, null, new Rect(20, 20, 150, 100));
                    dc.DrawRoundedRectangle(Brushes.Transparent, new Pen(Brushes.Red, 6),
                        new Rect(0, 0, bitmap.Width + 2 * border_thickness, bitmap.Height + 2 * border_thickness), 0, 0);
                }

                RenderTargetBitmap rtb = new RenderTargetBitmap(bitmap.PixelWidth + 2 * border_thickness, bitmap.PixelHeight + 2 * border_thickness, 96, 96, PixelFormats.Pbgra32);
                rtb.Render(dv);

                imgCapture.Source = rtb;
            }
            else
            {
                noImage.Text = IMAGE_NOT_AVAILABLE_MSG;
                imgCapture.Source = null;
            }
        }


        private void ShowBuildingWindowsLog(LogProcess logOut)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                elements_view_panel.Visibility = Visibility.Hidden;
                output_panel.Visibility = Visibility.Visible;
                output_text.Text = logOut.Output;
            }));
        }

        private void DoImportRanorexRepoFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import Ranorex repo file";
            openFileDialog.Filter = "Repo file|*.rxrep";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            //logger.Info("mainWindows thread: " + Thread.CurrentThread.ManagedThreadId);
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ranorex_repo_file_path = Utils.CreateTempFile(openFileDialog.FileName);
                scriptType = ScriptType.Ranorex;
                RuntimeInstance.listRootElement = new RanorexRxrepAnalyzer().Analyze(ranorex_repo_file_path);
            }
        }
        #endregion common actions

        #region commands
        //------------------------------------------------------
        //
        //  Shortcut CommandBindings
        //
        //------------------------------------------------------
        void DoInspectCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ChooseNewFileUI();
        }

        void DoSaveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SaveAction();
        }

        void DoExportCodeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ExportProjectAction();
        }

        void DoCloseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            CloseAction();
        }

        void DoOpenAppSelection(object sender, ExecutedRoutedEventArgs e)
        {
            SelectRunningApps();
        }
        #endregion commands

        #region callback functions
        /// <summary>
        /// callback function for event when new element added
        /// </summary>
        /// <param name="element"></param>
        public void NewElementAddedCallback(AutomationElement element)
        {
            object ob;
            // if not window
            if (element.TryGetCurrentPattern(WindowPattern.Pattern, out ob) == false)
            {
                AutomationElementCollection newAutoChildren = element.FindAll(TreeScope.Children,
                    System.Windows.Automation.Condition.TrueCondition);
                // must be clone, because mapping is modified in loop
                Dictionary<AutomationElement, IElement> tempDic =
                    new Dictionary<AutomationElement, IElement>(RuntimeInstance.mappingUIElement);

                // add by @duongtd 180303
                // modify stored map {old key, new key, value}
                List<Tuple<AutomationElement, AutomationElement, IElement>> pairNeedToUpdate =
                    new List<Tuple<AutomationElement, AutomationElement, IElement>>();
                foreach (KeyValuePair<AutomationElement, IElement> pair in tempDic)
                {
                    AutomationElement autoOldEle = pair.Key;
                    IElement ele = pair.Value;
                    AutomationElementCollection oldAutoChildrenUpdated =
                        autoOldEle.FindAll(TreeScope.Children, System.Windows.Automation.Condition.TrueCondition);
                    // if an element have changed in number of children
                    if (oldAutoChildrenUpdated != null && ele != null &&
                        ((ele.Children == null && oldAutoChildrenUpdated.Count > 0) ||
                            oldAutoChildrenUpdated.Count > ele.Children.Count) &&
                        (Comparison.Compare<string>(ele.Attributes.DesignedId, element.Current.AutomationId) ||
                            Comparison.Compare<string>(ele.Attributes.DesignedName, element.Current.Name)))
                    {
                        logger.Debug(GUI_Utils.TryAutoElementToString(autoOldEle));
                        // if new no. children of this element equals to the no. children of element catched by StructureChange event
                        if (newAutoChildren != null && oldAutoChildrenUpdated != null)
                        {
                            if (Comparison.ListElementsEqual(newAutoChildren, oldAutoChildrenUpdated))
                            {
                                logger.Info("[IM1] Handling new children for element: " + GUI_Utils.TryAutoElementToString(autoOldEle));
                                logger.Info("[IM1] newAutoCh: " + newAutoChildren.Count + ", oldAutoChildrenUpdated: " + oldAutoChildrenUpdated.Count);
                                List<AutomationElement> newChildren = Comparison.FindAdditionElements(
                                    ele.Children, newAutoChildren, tempDic);
                                if (newChildren != null && newChildren.Count > 0)
                                {
                                    AddNewElements(ele, newChildren);
                                }
                            }
                            // modify by @duongtd 180303
                            else if (newAutoChildren.Count >= oldAutoChildrenUpdated.Count)
                            {
                                logger.Info("[IM2] Handling new children for element: " + GUI_Utils.TryAutoElementToString(element));
                                logger.Info("[IM2] newAutoCh: " + newAutoChildren.Count + ", oldAutoChildrenUpdated: " + oldAutoChildrenUpdated.Count);
                                pairNeedToUpdate.Add(new Tuple<AutomationElement, AutomationElement, IElement>(
                                    element, autoOldEle, ele));
                                List<AutomationElement> newChildren = Comparison.FindAdditionElements(
                                    ele.Children, newAutoChildren, tempDic);
                                if (newChildren != null && newChildren.Count > 0)
                                {
                                    AddNewElements(ele, newChildren);
                                }
                            }
                            else
                                logger.Warn("(Not handled)Found new children for element: " + element.Current.Name +
                                    ", id: " + element.Current.AutomationId +
                                    ", type: " + element.Current.LocalizedControlType +
                                    ", old no. children: " + ele.Children.Count +
                                    ", new no. children: " + oldAutoChildrenUpdated.Count);
                        }
                    }
                }

                // add by @duongtd 180303
                foreach (var newPair in pairNeedToUpdate)
                {
                    RuntimeInstance.mappingUIElement.Remove(newPair.Item1);
                    RuntimeInstance.mappingUIElement[newPair.Item2] = newPair.Item3;
                }
            }
            // when new element is a window
            else
            {
                IElement newRootElement = new ElementsAnalyzer().Analyzing(element, elementDiscorerManual);
                elementDiscorerManual.ListElementAdded.Add(newRootElement);
                if (newRootElement != null)
                {
                    RuntimeInstance.listRootElement.Add(newRootElement);
                    BuildingTreeView buildingTreeView = new BuildingTreeView();
                    ObservableCollection<ElementItemVisual> old = (ObservableCollection<ElementItemVisual>)elementsTreeView.ItemsSource;
                    var newItems = buildingTreeView.PutAdapter(newRootElement);
                    foreach (ElementItemVisual elementItemVisual in newItems)
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            old.Add(elementItemVisual);
                        });
                    }
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        elementsTreeView.ItemsSource = old;
                    });
                }
                else
                {
                    //ignore
                }
            }
        }

        // when app selected
        public void AppSelectedCallBack(AppRunning app)
        {
            if (appSelection.IsActive)
                appSelection.Close();
            ResetAll();
            ProcessWithInstanceProcess(app.Process);
        }
        //callback
        public void SelectedDeviceCallBack(List<IElement> elements)
        {
            //elementsTreeView.ItemsSource = elements;
            RuntimeInstance.listRootElement = elements;
            BuildingTreeView buildingTreeView = new BuildingTreeView();
            ObservableCollection<ElementItemVisual> listRoot = buildingTreeView.PutAdapter(elements);
            if (listRoot != null && listRoot.Count > 0)
            {
                listRoot[0].IsExpanded = true;
                listRoot[0].IsSelected = true;
            }
            elementsTreeView.ItemsSource = listRoot;
            EnableUIElements();

        }

        #region progress dialog show callback
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
        #endregion progress dialog show callback
        #endregion callback functions

        #region hover helper
        private IKeyboardMouseEvents m_GlobalHook;
        public void HandleHoverHelper()
        {
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, TIME_MOUSE_HOVER)
            };
            timer.Tick += Timer_Tick;

            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.MouseMoveExt += GlobalHookMouseMoveExt;
        }

        /// <summary>
        /// callback function for hover element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Timer_Tick(object sender, EventArgs e)
        {
            //System.Windows.MessageBox.Show("Mouse stopped moving");
            // Convert mouse position from System.Drawing.Point to System.Windows.Point.
            // when mouse not moving for amount of time, timer will be stop (disable hover), it will not be restart until mouse move
            timer.Stop();
            if (RuntimeInstance.processTesting != null)
            {
                // get current point of mouse
                System.Windows.Point point = new System.Windows.Point(
                    System.Windows.Forms.Cursor.Position.X,
                    System.Windows.Forms.Cursor.Position.Y);
                ObservableCollection<ElementItemVisual> listRoot = (ObservableCollection<ElementItemVisual>)elementsTreeView.ItemsSource;
                ElementItemVisual visualElement = ElementHoverHelper.FindElement(point, RuntimeInstance.processTesting.Id, listRoot);
                if (visualElement != null)
                {
                    ChangeTreeViewSelection(visualElement);
                    //logger.Debug("Element's text: " + visualElement.Element.GetText());
                }
                else
                    logger.Info("ElementHoverHelper- cannot search element!");
            }
        }

        private void GlobalHookMouseMoveExt(object sender, MouseEventExtArgs e)
        {
            if (ENABLE_HOVER_HELPER)
            {
                // when mouse move, time count is restarted
                if (timer != null)
                {
                    timer.Stop();
                    timer.Interval = new TimeSpan(0, 0, 0, 0, TIME_MOUSE_HOVER);
                }
                timer.Start();
            }
            // uncommenting the following line will suppress the middle mouse button click
            // if (e.Buttons == MouseButtons.Middle) { e.Handled = true; }
        }
        #endregion hover helper

        #region util functions
        private void RetrieveElements2ExportSpecRecursive(ElementItemVisual current, List<IElement> listElements)
        {
            if (current != null && current.Element != null && current.IsExportToSpec)
                listElements.Add(current.Element);
            if (current.Children != null)
                foreach (var child in current.Children)
                    RetrieveElements2ExportSpecRecursive(child, listElements);
        }

        public void AddElementAndChildren2Spec(ElementItemVisual current)
        {
            current.IsExportToSpec = true;
            if (current.Children != null)
                foreach (var child in current.Children)
                    AddElementAndChildren2Spec(child);
        }

        public void RemoveElementAndChildren2Spec(ElementItemVisual current)
        {
            current.IsExportToSpec = false;
            if (current.Children != null)
                foreach (var child in current.Children)
                    RemoveElementAndChildren2Spec(child);
        }

        /// <summary>
        /// Process for new Elements
        /// </summary>
        /// <param name="parentElement"></param>
        /// <param name="parentAutoElement"></param>
        /// <param name="children"></param>
        private void AddNewElements(IElement parentElement, List<AutomationElement> children)
        {
            try
            {
                ObservableCollection<ElementItemVisual> listRootVisualElements =
                    (ObservableCollection<ElementItemVisual>)elementsTreeView.ItemsSource;
                ElementItemVisual parentVisual = ElementItemVisual.FromElement(parentElement, listRootVisualElements);
                foreach (AutomationElement autoChild in children)
                {
                    if (parentElement.Children == null)
                        parentElement.Children = new List<IElement>();
                    IElement newElement = new ElementsAnalyzer().Convert(autoChild, parentElement, new Dictionary<string, int>());
                    parentElement.Children.Add(newElement);
                    if (parentVisual != null)
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            var childVisual = new ElementItemVisual(newElement);
                            parentVisual.Children.Add(childVisual);
                            childVisual.Parent = parentVisual;
                        });
                }
            }
            catch (Exception)
            {
            }
        }

        //int lastInspectType = -1; // 0 instance, 1 others
        private void ResetAll()
        {
            RuntimeInstance.listAutoElement = new List<AutomationElement>();
            RuntimeInstance.mapIdElements = new Dictionary<string, IElement>();
            RuntimeInstance.mappingUIElement = new Dictionary<AutomationElement, IElement>();
            RuntimeInstance.listRootElement = new List<IElement>();
            RuntimeInstance.processTesting = null;
            RuntimeInstance.listRootAutoElement = new List<AutomationElement>();
            RuntimeInstance.additionFiles = new List<string>();
            RuntimeInstance.currentRunningFileTest = null;
            RuntimeInstance.currentTestingModule = null;
            RuntimeInstance.listRunningFilesTest = new List<string>();
            RuntimeInstance.pathToApp = null;
            RuntimeInstance.testingReportModel = null;
            RuntimeInstance.testReportFilePath = null;

            myLog = new MyLog();
            this.elementDiscorerManual = new ElementDiscoverManual(this);
            this.app_under_test_path = null;
            this.appSelection = new AppSelection(this);
            this.excel_spec_file_path = null;
            this.listScreens = new List<SpecScreen>();
            this.timer.Stop();
            //logger.Debug("start remove event registered...");
            //if (lastInspectType == 0)
                //Automation.RemoveAllEventHandlers();
            //logger.Debug("finish remove event registered!");
            ResetWindow();
        }

        public void ResetWindow()
        {
            elements_view_panel.Visibility = Visibility.Visible;
            output_panel.Visibility = Visibility.Hidden;
            output_text.Text = "";
        }

        /// <summary>
        /// update tree view display when data change
        /// </summary>
        private void RefreshTreeView()
        {
            elementsTreeView.Items.Refresh();
            elementsTreeView.UpdateLayout();
        }

        /// <summary>
        /// change tree view item selection
        /// </summary>
        /// <param name="visualElement"></param>
        private void ChangeTreeViewSelection(ElementItemVisual visualElement)
        {
            var ob = elementsTreeView.SelectedItem;
            if (ob != null)
                (ob as ElementItemVisual).IsSelected = false;
            ElementItemVisual.SelectItemAndExpandParent(visualElement);
            //logger.Debug("ElementHoverHelper- " + visualElement.Name);
            RefreshTreeView();
        }

        public void SelectRunningApps()
        {
            appSelection = new AppSelection(this);
            appSelection.RefreshListApp();
            appSelection.Show();
        }

        private void DoAnalyze(AutomationElementCollection windows)
        {
            IElementsAnalyzer elementsAnalyzer = new ElementsAnalyzer();
            List<IElement> listRootElement = elementsAnalyzer.Analyzing(windows, elementDiscorerManual);
            //logger.Debug("Finish analyzing");
            if (listRootElement != null && listRootElement.Count > 0)
            {
                RuntimeInstance.listRootElement.AddRange(listRootElement);
                BuildingTreeView buildingTreeView = new BuildingTreeView();
                ObservableCollection<ElementItemVisual> listRoot = buildingTreeView.PutAdapter(listRootElement);
                if (listRoot != null && listRoot.Count > 0)
                {
                    listRoot[0].IsExpanded = true;
                    listRoot[0].IsSelected = true;
                }
                System.Windows.Application.Current.Dispatcher.BeginInvoke(
                    //DispatcherPriority.Background,
                    new Action(() =>
                    {
                        elementsTreeView.ItemsSource = listRoot;
                        //logger.Debug("Finish building treeview");
                        EnableUIElements();
                    }));
            }
            else
            {
                //ignore
            }
            //this.Focus();
        }

        private void Collapse_All_Item(ElementItemVisual element)
        {
            element.IsExpanded = false;
            if (element.Children == null)
            {
                return;
            }
            foreach (ElementItemVisual ele in element.Children)
            {
                Collapse_All_Item(ele);
            }
        }

        private void DisplayTestScenarios(List<SpecScreen> screens)
        {
            // fake
            //var elements = XmlFilesLoader.Load(@"C:\Users\duongtd\Desktop\samples\exported\TestApplication\TestApplication\TestApplicationRepo.xml",
            //    @"C:\Users\duongtd\Desktop\samples\exported\TestApplication\TestApplication\TestApplicationImageCapture.xml");
            //ExcelSpecificationParser excelSpecificationParser = new ExcelSpecificationParser();
            //List<SpecScreen> screens = excelSpecificationParser.ParseWithRootElements(@"C:\Users\duongtd\Desktop\TSDV-Sample-UserCode-Spec.xlsx",
            //    elements, new MyLog());
            //UserActionSpecAnalyzer userActionSpecAnalyzer = new UserActionSpecAnalyzer();
            //userActionSpecAnalyzer.Expand(screens, "", new MyLog());

            //logger.Info("DisplayTestScenarios thread: " + Thread.CurrentThread.ManagedThreadId);

            if (grid_row_definition_scenarios.Height.Equals(new GridLength(0)));
                grid_row_definition_scenarios.Height = new GridLength(1, GridUnitType.Star);
            var ucTestScenarios = new UCTestScenarios(screens);
            grid_raw_scenarios.Children.Clear();
            grid_raw_scenarios.Children.Add(ucTestScenarios);
            //ucTestScenarios.LoadData(screens);
        }

        private List<SpecScreen> HandleSpecExcelFile(List<IElement> listRootElements, string specExcelFilePath, 
            string appPath, MyLog myLog)
        {
            //logger.Info("HandleSpecExcelFile thread: " + Thread.CurrentThread.ManagedThreadId);
            try
            {
                ExcelSpecificationParser excelSpecificationParser = new ExcelSpecificationParser();
                List<SpecScreen> specScreens = excelSpecificationParser.ParseWithRootElements(
                   specExcelFilePath, listRootElements, myLog);

                UserActionSpecAnalyzer specAnalyzer = new UserActionSpecAnalyzer();
                specAnalyzer.Expand(Utils.ConvertSpecToNormal(specScreens), appPath, myLog);
                return specScreens;
            }
            catch (Exception e)
            {
                logger.Error("Exception when analyzing spec. " + e.StackTrace);
                return null; 
            }
        }
        #endregion util functions

        #region Mobile
        private void AndroidInspect()
        {
            // show gui
            // send adb dump command
            // parse .xml file to get List<IElement>
            // visulize elements tree
        }
        #endregion

        /// <summary>
        /// for deploy
        /// </summary>
        private void Release()
        {
            Properties.Settings.Default.path_GUI_Testing_Automation_ref = null;
            Properties.Settings.Default.Save();
        }
    }

    /// <summary>
    /// notify when an app selected (for inspecting)
    /// </summary>
    public interface ISelectedAppNotify
    {
        void AppSelectedCallBack(AppRunning app);
    }

    //callback
    public interface ISelectedDeviceNotify
    {
        void SelectedDeviceCallBack(List<IElement> elements);
    }

    public enum ScriptType
    {
        Normal, Ranorex
    }

    
}