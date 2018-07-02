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
    public class ConfigFile : AbstractFile
    {
        public ConfigFile(string path) : base(path) { }
        public ConfigFile(string path, IFile parent) : base(path, parent) { }
    }
}
