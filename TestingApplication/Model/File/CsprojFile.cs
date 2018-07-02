// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:04 PM 2017/11/30
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class CsprojFile : AbstractFile
    {
        public const string C_SHARP_PROJECT_TYPE_GUID = "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";
        public const string WPF_PROJECT_TYPE_GUID = "60dc8134-eba5-43b8-bcc9-bb4bc16c2548";

        public List<string> ProjectTypeGuids
        {
            get { return projectTypeGuids; }
            set { projectTypeGuids = value; }
        }
        
        public CsprojFile(string path) : base(path) { }
        public CsprojFile(string path, IFile parent) : base(path, parent) { }

        private List<string> projectTypeGuids;
    }
}
