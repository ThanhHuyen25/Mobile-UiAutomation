// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 10:53 AM 2017/11/23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public interface ISearchCondition
    {

        bool IsSatisfiable(IFile file);
    }
}
