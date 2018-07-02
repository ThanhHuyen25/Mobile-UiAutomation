// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 9:54 AM 2017/12/1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class CsprojWpfFile : CsprojFile
    {
        public CsprojWpfFile(string path) : base(path) { }

        //private List<WindowWpf> listWindows;

        //public List<WindowWpf> ListWindows
        //{
        //    get { return listWindows; }
        //    set { this.listWindows = value; }
        //}

        /// <summary>
        /// xaml file define start up window (path, not file name)
        /// </summary>
        private string applicationDefinion;
        public string ApplicationDefinion
        {
            get { return applicationDefinion; }
            set { applicationDefinion = value; }
        }

        public Dictionary<string, string> MapXamlCsFilesPath
        {
            get { return mapXamlCsFilesPath; }
            set { mapXamlCsFilesPath = value; }
        }

        private Dictionary<string, string> mapXamlCsFilesPath;
    }
}
