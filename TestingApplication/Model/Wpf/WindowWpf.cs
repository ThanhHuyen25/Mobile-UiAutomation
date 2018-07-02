// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 9:23 AM 2017/12/1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    /// <summary>
    /// represent for a window
    /// consist of path to .cs file and its name
    /// </summary>
    public class WindowWpf
    {
        /// <summary>
        /// MainWindow.xaml
        /// </summary>
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// MainWindow has a .cs file code (MainWindow.cs or MainWindow.xaml.cs)
        /// </summary>
        private CsFile csFileCode;
        public CsFile CsFileCode
        {
            get { return csFileCode; }
            set { csFileCode = value; }
        }

        /// <summary>
        /// FirstFolder\MainWindow.xaml
        /// </summary>
        private string fullName;
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public WindowWpf()
        {
        }

        public WindowWpf(string name, CsFile csFileCode, string fullName)
        {
            this.name = name;
            this.csFileCode = csFileCode;
            this.fullName = fullName;
        }
    }
}
