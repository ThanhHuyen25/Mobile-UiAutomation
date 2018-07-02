// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:18 PM 2018/1/10
using EnvDTE;
using EnvDTE80;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;

namespace GUI_Testing_Automation
{
    public class Handler
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Open application from path input
        /// </summary>
        /// <param name="pathToProgram"></param>
        /// <returns></returns>
        public static bool OpenApp(string pathToProgram)
        {
            //modify by duongtd 180226
            GUI_Utils.OpenApp(pathToProgram, TreeScope.Children, true);
            return true;
        }

        public static bool CloseApp()
        {
            if(RuntimeInstance.processTesting == null)
            {
                logger.Warn("App not running to close!");
                return false;
            }
            RuntimeInstance.processTesting.Kill();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathToElementRepo">path to .xml file store list of elements</param>
        /// <param name="pathToImageCaptureRepo"></param>
        /// <returns></returns>
        public static bool Init(string projectName, string runningTestFilePath)
        {
            RuntimeInstance.testReportFilePath = @"..\...\Reports\" + projectName + "_" + DateTime.Now.ToString("yyMMdd_hhmmss") + ".guilog";
            if (!Directory.Exists(@"..\...\Reports\"))
                Directory.CreateDirectory(@"..\...\Reports\");
            RuntimeInstance.testingReportModel = new TestingReportModel();

            RuntimeInstance.listRunningFilesTest = File.ReadLines(runningTestFilePath);
            RuntimeInstance.additionFiles = new List<string>();
            return true;
        }

        public static void InitNewModule(string moduleName)
        {
            string filePath = GetFilePath(moduleName, RuntimeInstance.listRunningFilesTest);
            if (filePath.EndsWith(".cs"))
                filePath = filePath.Substring(0, filePath.Length - ".cs".Length);
            RuntimeInstance.currentRunningFileTest = new Tuple<string,string>(moduleName, filePath);
            if (RuntimeInstance.currentTestingModule != null)
            {
                GUI_Utils.CheckRuntimeInstance();
                RuntimeInstance.testingReportModel.ListTestModules.Add(RuntimeInstance.currentTestingModule);
            }
            RuntimeInstance.currentTestingModule = null;
        }

        private static string GetFilePath(string moduleName, IEnumerable<string> listRunningFilesTest)
        {
            foreach(string runningFileTest in listRunningFilesTest)
            {
                if (Regex.IsMatch(runningFileTest, @"\\" + moduleName + @"\.cs$"))
                    return runningFileTest;
            }
            return null;
        }

        public static bool Finish()
        {
            // write RuntimeInstance.testingReportModel into RuntimeInstance.testReportFilePath file
            if (RuntimeInstance.currentTestingModule != null)
            {
                GUI_Utils.CheckRuntimeInstance();
                if (!RuntimeInstance.testingReportModel.ListTestModules.Contains(RuntimeInstance.currentTestingModule))
                    RuntimeInstance.testingReportModel.ListTestModules.Add(RuntimeInstance.currentTestingModule);
            }
            bool need2WriteLog = RuntimeInstance.testingReportModel != null &&
                RuntimeInstance.testingReportModel.ListTestModules != null &&
                RuntimeInstance.testingReportModel.ListTestModules.Count > 0;
            if (need2WriteLog)
                TestReportFileLoader.WriteFile(RuntimeInstance.testReportFilePath, RuntimeInstance.testingReportModel);
            if (need2WriteLog ||
                (RuntimeInstance.additionFiles != null &&
                 RuntimeInstance.additionFiles.Count > 0))
            {
                // add generated file to current testing project here
                AddReportFiles2Proj(need2WriteLog);
            }
            return true;
        }

        private static bool AddReportFiles2Proj(bool need2WriteLog)
        {
            MessageFilter.Register();
            DTE2 dte2 = GetCurrent();
            //dte2.Solution.Projects.Item(2).ProjectItems.AddFromFile(@"D:\Research\projects\UtilitiesSln\ConsoleApp1\Reports\abc.txt");
            var prj = dte2.Solution.Projects.Item(1);
            if (need2WriteLog)
                prj.ProjectItems.AddFromFile(Path.GetFullPath(RuntimeInstance.testReportFilePath));
            if (RuntimeInstance.additionFiles != null)
                foreach (string additionFile in RuntimeInstance.additionFiles)
                    prj.ProjectItems.AddFromFile(Path.GetFullPath(additionFile));
            if (need2WriteLog)
                dte2.ItemOperations.OpenFile(Path.GetFullPath(RuntimeInstance.testReportFilePath));
            MessageFilter.Revoke();
            return true;
        }

        [DllImport("ole32.dll")]
        private static extern void CreateBindCtx(int reserved, out IBindCtx ppbc);
        [DllImport("ole32.dll")]
        private static extern void GetRunningObjectTable(int reserved, out IRunningObjectTable prot);

        /// <summary>
        /// Gets the current visual studio's solution DTE2
        /// </summary>
        private static DTE2 GetCurrent()
        {
            List<DTE2> dte2s = new List<DTE2>();

            IRunningObjectTable rot;
            GetRunningObjectTable(0, out rot);
            IEnumMoniker enumMoniker;
            rot.EnumRunning(out enumMoniker);
            enumMoniker.Reset();
            IntPtr fetched = IntPtr.Zero;
            IMoniker[] moniker = new IMoniker[1];
            while (enumMoniker.Next(1, moniker, fetched) == 0)
            {
                IBindCtx bindCtx;
                CreateBindCtx(0, out bindCtx);
                string displayName;
                moniker[0].GetDisplayName(bindCtx, null, out displayName);
                // add all VisualStudio ROT entries to list
                if (displayName.StartsWith("!VisualStudio"))
                {
                    object comObject;
                    rot.GetObject(moniker[0], out comObject);
                    dte2s.Add((DTE2)comObject);
                }
            }

            // get path of the executing assembly (assembly that holds this code) - you may need to adapt that to your setup
            string thisPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            // compare dte solution paths to find best match
            KeyValuePair<DTE2, int> maxMatch = new KeyValuePair<DTE2, int>(null, 0);
            foreach (DTE2 dte2 in dte2s)
            {
                int matching = GetMatchingCharsFromStart(thisPath, dte2.Solution.FullName);
                if (matching > maxMatch.Value)
                    maxMatch = new KeyValuePair<DTE2, int>(dte2, matching);
            }

            return (DTE2)maxMatch.Key;
        }

        /// <summary>
        /// Gets index of first non-equal char for two strings
        /// Not case sensitive.
        /// </summary>
        private static int GetMatchingCharsFromStart(string a, string b)
        {
            a = (a ?? string.Empty).ToLower();
            b = (b ?? string.Empty).ToLower();
            int matching = 0;
            for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
            {
                if (!char.Equals(a[i], b[i]))
                    break;

                matching++;
            }
            return matching;
        }
    }

    /// <summary>
    /// Class containing the IOleMessageFilter
    /// thread error-handling functions.
    /// </summary>
    internal class MessageFilter : IOleMessageFilter
    {
        // Start the filter.
        public static void Register()
        {
            IOleMessageFilter newFilter = new MessageFilter();
            IOleMessageFilter oldFilter = null;
            CoRegisterMessageFilter(newFilter, out oldFilter);
        }

        // Done with the filter, close it.
        public static void Revoke()
        {
            IOleMessageFilter oldFilter = null;
            CoRegisterMessageFilter(null, out oldFilter);
        }

        //
        // IOleMessageFilter functions.
        // Handle incoming thread requests.
        int IOleMessageFilter.HandleInComingCall(int dwCallType, System.IntPtr hTaskCaller, int dwTickCount, System.IntPtr lpInterfaceInfo)
        {
            return 0; //Return the flag SERVERCALL_ISHANDLED.
        }

        // Thread call was rejected, so try again.
        int IOleMessageFilter.RetryRejectedCall(System.IntPtr hTaskCallee, int dwTickCount, int dwRejectType)
        {
            if (dwRejectType == 2)
            // flag = SERVERCALL_RETRYLATER.
            {
                return 99; // Retry the thread call immediately if return >=0 & <100.
            }
            return -1; // Too busy; cancel call.
        }

        int IOleMessageFilter.MessagePending(System.IntPtr hTaskCallee, int dwTickCount, int dwPendingType)
        {
            //Return the flag PENDINGMSG_WAITDEFPROCESS.
            return 2;
        }

        // Implement the IOleMessageFilter interface.
        [DllImport("Ole32.dll")]
        private static extern int CoRegisterMessageFilter(IOleMessageFilter newFilter, out IOleMessageFilter oldFilter);
    }

    [ComImport(), Guid("00000016-0000-0000-C000-000000000046"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    interface IOleMessageFilter
    {
        [PreserveSig]
        int HandleInComingCall(int dwCallType, IntPtr hTaskCaller, int dwTickCount, IntPtr lpInterfaceInfo);
        [PreserveSig]
        int RetryRejectedCall(IntPtr hTaskCallee, int dwTickCount, int dwRejectType);
        [PreserveSig]
        int MessagePending(IntPtr hTaskCallee, int dwTickCount, int dwPendingType);
    }
}