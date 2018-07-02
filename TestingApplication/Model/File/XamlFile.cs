// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 5:38 PM 2017/11/22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class XamlFile : AbstractFile
    {
        public XamlFile(string path) : base(path) { }
        public XamlFile(string path, IFile parent) : base(path, parent) { }

        /// <summary>
        /// full path to start up file
        /// </summary>
        private string windowStartup;

        public string WindowStartup
        {
            get { return windowStartup; }
            set { windowStartup = value; }
        }
    }
}