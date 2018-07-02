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
    public class CsprojWfaFile : CsprojFile
    {
        public CsprojWfaFile(string path) : base(path) { }

        /// <summary>
        /// full path to .cs file
        /// .cs file contain Window, Page, Form,
        /// </summary>
        private List<string> listCsFilesScreen;
        public List<string> ListCsFilesScreen
        {
            get { return listCsFilesScreen; }
            set { listCsFilesScreen = value; }
        }
    }
}
