// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:04 PM 2017/11/30
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class CsFile : AbstractFile
    {
        public CsFile(string path) : base(path) { }
        public CsFile(string path, IFile parent) : base(path, parent) { }
        private List<CsClass> classes;

        public List<CsClass> Classes
        {
            get { return classes; }
            set { classes = value; }
        }

        public string GetName()
        {
            FileInfo fileInfo = new FileInfo(path);
            return fileInfo.Name;
        }

        public string GetNameWithoutExtension()
        {
            FileInfo fileInfo = new FileInfo(path);
            string re = fileInfo.Name.Replace(".xaml.cs", "");
            return re.Replace(fileInfo.Extension, "");
        }
    }

    public class CsClass
    {
        /// <summary>
        /// Wpf project type
        /// </summary>
        public const string WINDOW_CLASS = "Window";
        public const string PAGE_CLASS = "Page";

        /// <summary>
        /// Window form app
        /// </summary>
        public const string FORM_CLASS = "Form";

        private string name;
        /// <summary>
        /// name of parent, interface inheritance
        /// </summary>
        private List<string> baseOnClass;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<string> BaseOnClass
        {
            get { return baseOnClass; }
            set { baseOnClass = value; }
        }

        private bool isContainMain = false;

        public bool IsContainMain
        {
            get { return isContainMain; }
            set { isContainMain = value; }
        }
    }
}
