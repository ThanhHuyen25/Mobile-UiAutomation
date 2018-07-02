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
    /// <summary>
    /// condition for search @AutomationElement
    /// current use 2 params: AutomationId and Name, but not unique (more than one element can have the same AutomationId and Name)
    /// </summary>
    public class IdAndNameDesignCondition
    {
        private string designId;

        public string DesignId
        {
            get { return designId; }
            set { designId = value; }
        }
        private string designName;

        public string DesignName
        {
            get { return designName; }
            set { designName = value; }
        }

        public IdAndNameDesignCondition(string id, string name)
        {
            this.designId = id;
            this.designName = name;
        }
    }
}
