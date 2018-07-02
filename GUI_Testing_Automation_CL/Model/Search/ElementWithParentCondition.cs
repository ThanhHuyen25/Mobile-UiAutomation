// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:18 PM 2018/1/10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Testing_Automation
{
    public class ElementWithParentCondition : AbstractSearchCondition
    {
        public override bool IsSatisfiable(IElement element)
        {
            return true;
        }
    }
}
