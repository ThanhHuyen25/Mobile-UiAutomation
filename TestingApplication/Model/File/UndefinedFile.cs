// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:22 PM 2017/11/22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    /// <summary>
    /// not-interested file
    /// </summary>
    public class UndefinedFile : AbstractFile
    {
        public UndefinedFile(string path) : base(path) { }
        public UndefinedFile(string path, IFile parent) : base(path, parent) { }
    }
}
