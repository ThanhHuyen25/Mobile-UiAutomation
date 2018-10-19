// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:37 PM 2018/7/1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Testing_Automation
{
    /// <summary>
    /// represent for android-based element
    /// </summary>
    public abstract class AbstractAndroidElement : ElementBase
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string LAYOUT = "layout";
        public const string VIEW = "view";
        public const string IMAGE_VIEW = "image view";
        public const string TEXT_VIEW = "text view";
        public const string VIEW_SWITCHER = "view switcher";
        public const string BUTTON = "button";
        public const string VIEW_PAGER = "view pager";
        public const string EDIT_TEXT = "edit text";
        public const string IMAGE_BUTTON = "image button";
        public const string SPINNER = "spinner";
        public const string LIST_VIEW = "list view";
        public const string SCROLL_VIEW = "scroll view";


        public AbstractAndroidElement(string id) : base(id)
        {
        }
        public AbstractAndroidElement() : base()
        {
        }


    }
}
