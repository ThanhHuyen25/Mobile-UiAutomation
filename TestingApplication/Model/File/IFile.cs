// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:19 PM 2017/11/10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    /// <summary>
    /// abstract file
    /// </summary>
    public interface IFile
    {
        string Path { get; set; }
        IFile Parent { get; set; }
        //maybe null
        List<IFile> Children { get; set; }
    }
}
