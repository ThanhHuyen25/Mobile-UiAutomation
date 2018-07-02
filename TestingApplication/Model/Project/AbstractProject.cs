// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 9:08 PM 2017/11/10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public abstract class AbstractProject : IProject
    {
        protected Folder rootFolder;

        public AbstractProject() { }

        public AbstractProject(Folder rootFolder)
        {
            this.rootFolder = rootFolder;
        }

        public Folder RootFolder
        {
            get { return rootFolder; }
            set { rootFolder = value; }
        }

        public CsFile CsStartupFile
        {
            get { return csStartupFile; }
            set { csStartupFile = value; }
        }
        protected CsFile csStartupFile;
    }
}
