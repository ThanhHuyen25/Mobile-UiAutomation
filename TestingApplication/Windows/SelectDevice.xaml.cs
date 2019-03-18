using GUI_Testing_Automation;
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

namespace TestingApplication
{
    /// <summary>
    /// Interaction logic for SelectDevice.xaml
    /// </summary>
    public partial class SelectDevice : Window
    {
        private ISelectedDeviceNotify notify;
        // close
        private ICloseWindowNotify closeWindow;
        private SelectDevice()
        {
            InitializeComponent();
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "adb";

            AndroidAdbCommand AdbCommand = new AndroidAdbCommand();
            List<AndroidDevice> Devices = new List<AndroidDevice>();
            Devices = AdbCommand.GetAllDevices();
            for(int i=0; i < Devices.Count; i++)
            {
                listView.Items.Add(Devices[i].Name);
            }
            
        }

        //public SelectDevice(ICloseWindowNotify closeWindow) : this()
        //{
        //    this.closeWindow = closeWindow;
        //}

            // close
        public SelectDevice(ISelectedDeviceNotify notify, ICloseWindowNotify closeWindow) : this()
        {
            this.notify = notify;
            this.closeWindow = closeWindow;
        }
        public ISelectedDeviceNotify Notify
        {
            get { return notify; }
            set { notify = value; }
        }

        // close
        public ICloseWindowNotify CloseWindow
        {
            get { return closeWindow; }
            set { closeWindow = value; }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            AndroidAdbCommand AdbCommand = new AndroidAdbCommand();
            List<AndroidDevice> Devices = new List<AndroidDevice>();
            Devices = AdbCommand.GetAllDevices();

            for (int i=0; i<listView.Items.Count;i++)
            {
                string name = listView.SelectedItems[i].ToString();
                name = name.Replace("\r", "");
                name = name.Replace("\n", "");
                if (name != null)
                {
                    //MessageBox.Show(name);
                    //break;
                    for (int j = 0; j < Devices.Count; j++)
                    {
                        if (Devices[j].Name == name)
                        {
                            string test = AdbCommand.DumpGUI(Devices[j]);
                            List<IElement> elements = new AndroidAdbDumpFileParser().Parse(test);
                            //MessageBox.Show(test);
                            if (elements != null)
                            {
                                // close
                                this.Close();
                                closeWindow.CloseWindow();
                                notify.SelectedDeviceCallBack(elements);
                                
                            }
                            break;
                            }
                    }
                    
                    break;
                }
                else { MessageBox.Show("Please Choice Device!"); }
            }
        }
    }
}
