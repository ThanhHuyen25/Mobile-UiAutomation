// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 10:56 AM 2017/11/23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class CsprojSearchCondition : SearchCondition
    {
        public override bool IsSatisfiable(IFile file)
        {
            return file is CsprojFile;
        }
    }
}
