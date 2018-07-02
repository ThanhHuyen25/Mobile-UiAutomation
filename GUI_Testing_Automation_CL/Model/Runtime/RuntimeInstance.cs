// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:18 PM 2018/1/10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace GUI_Testing_Automation
{
    // store all runtime vars
    public class RuntimeInstance
    {        
        public static List<IElement> listRootElement = new List<IElement>();

        /// <summary>
        /// map element's id and itself
        /// </summary>
        public static Dictionary<string, IElement> mapIdElements = new Dictionary<string, IElement>();

        public static List<AutomationElement> listRootAutoElement = new List<AutomationElement>();

        public static List<AutomationElement> listAutoElement = new List<AutomationElement>();

        public static Dictionary<AutomationElement, IElement> mappingUIElement = new Dictionary<AutomationElement, IElement>();

        /// <summary>
        /// path to screen testing
        /// </summary>
        public static string pathToApp;

        public static bool isAppOpened = false;

        /// <summary>
        /// process screen testing run
        /// </summary>
        public static Process processTesting;

        public static string testReportFilePath;

        public static TestingReportModel testingReportModel;

        public static IEnumerable<string> listRunningFilesTest;

        //<name, relativepath>
        public static Tuple<string,string> currentRunningFileTest;

        public static TestingModuleReport currentTestingModule;

        public static List<string> additionFiles = new List<string>();
    }
}
