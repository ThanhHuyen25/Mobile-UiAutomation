// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:57 AM 2017/11/23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class Folder : AbstractFile
    {
        public Folder(string path) : base(path) { }
        public Folder(string path, IFile parent) : base(path, parent) { }
    }
}
