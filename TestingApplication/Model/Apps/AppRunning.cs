// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:46 PM 2018/3/3
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TestingApplication
{
    public class AppRunning
    {
        private ImageSource icon; 
        private int processId;
        private string name;

        private Process process;

        public int ProcessId
        {
            get { return processId; }
            set
            {
                processId = value;
            }
        }
        public ImageSource Icon
        {
            get { return icon; }
            set
            {
                icon = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
        public Process Process
        {
            get { return process; }
            set
            {
                process = value;
            }
        }

        public AppRunning(ImageSource _icon, int _processId, string _name)
        {
            this.icon = _icon;
            this.processId = _processId;
            this.name = _name;
        }
    }
}
