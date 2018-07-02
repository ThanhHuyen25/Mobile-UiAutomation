// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 9:08 PM 2017/11/10
using GUI_Testing_Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public interface IProject
    {
        Folder RootFolder { get; set; }
        CsFile CsStartupFile { get; set; }
    }
}
