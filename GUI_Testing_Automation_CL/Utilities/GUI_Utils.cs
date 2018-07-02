// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:18 PM 2018/1/10
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Media.Imaging;

namespace GUI_Testing_Automation
{
    public class GUI_Utils
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IElement SearchElementById(List<IElement> listRootElement, string id)
        {
            foreach (IElement root in listRootElement)
            {
                IElement re = SearchElementById(root, id);
                if (re != null)
                    return re;
            }
            return null;
        }

        /// <summary>
        /// search IElement by id
        /// </summary>
        /// <param name="element"></param>
        /// <param name="id">UUID string, like this: "82138d04-6c3a-4a4b-b840-e79baf09b47d"</param>
        /// <returns></returns>
        public static IElement SearchElementById(IElement rootElement, string id)
        {
            IElement re = null;
            SearchElementById(rootElement, id, ref re);
            return re;
        }

        private static void SearchElementById(IElement element, string id, ref IElement re)
        {
            if (element.Id.Equals(id))
            {
                re = element;
                return;
            }
            if (element.Children != null)
            {
                foreach (IElement child in element.Children)
                {
                    SearchElementById(child, id, ref re);
                }
            }
        }

        public static List<IElement> SearchByName(IElement rootElement, string name)
        {
            List<IElement> re = new List<IElement>();
            SearchByName(rootElement, name, re);
            return re;
        }

        private static void SearchByName(IElement element, string name, List<IElement> re)
        {
            if (element.Attributes.Name.Equals(name))
                re.Add(element);
            if (element.Children != null)
            {
                foreach (IElement child in element.Children)
                {
                    SearchByName(child, name, re);
                }
            }
        }

        public static Bitmap DecodeBase64ToBitmap(string strEncoded)
        {
            Byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(strEncoded));
            System.IO.MemoryStream streamBitmap = new System.IO.MemoryStream(bitmapData);
            return new Bitmap((Bitmap)Image.FromStream(streamBitmap));
        }

        private static string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", string.Empty); sbText.Replace(" ", string.Empty);
            return sbText.ToString();
        }

        public static BitmapImage decodeBase64ToImage(string base64String)
        {
            byte[] byteBuffer = Convert.FromBase64String(base64String);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new System.IO.MemoryStream(byteBuffer);
            image.EndInit();
            return image;
        }

        /// <summary>
        /// convert encoded string to BitmapImage object
        /// and resize bitmap if it overs max size
        /// </summary>
        /// <param name="base64String"></param>
        /// <param name="maxImageWidth"></param>
        /// <param name="maxImageHeight"></param>
        /// <returns></returns>
        public static BitmapImage DecodeBase64ToImageWithResizeImg(string base64String,
            double maxImageWidth, double maxImageHeight)
        {
            Bitmap bitmap = DecodeBase64ToBitmap(base64String);
            bool isNeedResize = bitmap.Width > maxImageWidth || bitmap.Height > maxImageHeight;
            if (isNeedResize)
            {
                double ratioResizeWidth = maxImageWidth / (double)bitmap.Width;
                double ratioResizeHeight = maxImageHeight / (double)bitmap.Height;
                double targetRatioResize = ratioResizeWidth < ratioResizeHeight ?
                    ratioResizeWidth : ratioResizeHeight;
                double newWidth = targetRatioResize * (double)bitmap.Width;
                double newHeight = targetRatioResize * (double)bitmap.Height;
                return ConvertBitmap2BitmapImage(
                    ResizeBitmap(bitmap, new Size((int)newWidth, (int)newHeight)));
            }
            else
                return ConvertBitmap2BitmapImage(bitmap);
        }

        public static Bitmap ResizeBitmap(Bitmap imgToResize, Size size)
        {
            Bitmap b = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage((Image)b))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
            }
            return b;
        }

        public static BitmapImage ConvertBitmap2BitmapImage(Bitmap bitmap)
        {
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Jpeg);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        /// <summary>
        /// open .exe program
        /// </summary>
        /// <param name="pathToProgram"></param>
        /// <param name="scope"></param>
        /// <param name="removeOldInRuntime"> remove old instance if exist in RuntimeInstance </param>
        /// <returns></returns>
        public static List<AutomationElement> OpenApp(string pathToProgram, TreeScope scope, bool removeOldInRuntime)
        {
            return DoOpenApp(pathToProgram, scope, removeOldInRuntime).Item2;
        }

        /// <summary>
        /// open .exe program
        /// </summary>
        /// <param name="pathToProgram"></param>
        /// <param name="scope"></param>
        /// <param name="removeOldInRuntime"> remove old instance if exist in RuntimeInstance </param>
        /// <returns></returns>
        public static AutomationElementCollection OpenApp1(string pathToProgram, TreeScope scope, bool removeOldInRuntime)
        {
            return DoOpenApp(pathToProgram, scope, removeOldInRuntime).Item1;
        }

        /// <summary>
        /// open .exe program
        /// </summary>
        /// <param name="pathToProgram"></param>
        /// <param name="scope"></param>
        /// <param name="removeOldInRuntime"> remove old instance if exist in RuntimeInstance </param>
        /// <returns></returns>
        public static Tuple<AutomationElementCollection, List<AutomationElement>> DoOpenApp(
            string pathToProgram, TreeScope scope, bool removeOldInRuntime)
        {
            //run program to test
            Process process = Process.Start(pathToProgram);
            return DoOpenApp(process, scope, removeOldInRuntime);
        }

        public static Tuple<AutomationElementCollection, List<AutomationElement>> DoOpenApp(
            Process process, TreeScope scope, bool removeOldInRuntime)
        {
            //wait for app runing.
            while (process.MainWindowHandle == IntPtr.Zero) { }
            Thread.Sleep(500);
            if (removeOldInRuntime)
            {
                RuntimeInstance.processTesting = process;
            }
            return DoOpenApp(process.Id, scope, removeOldInRuntime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="scope"></param>
        /// <param name="removeOldInRuntime"></param>
        /// <returns></returns>
        public static Tuple<AutomationElementCollection, List<AutomationElement>> DoOpenApp(
            int processId, TreeScope scope, bool removeOldInRuntime)
        {
            Condition mainForm = new PropertyCondition(AutomationElement.ProcessIdProperty, processId);
            AutomationElementCollection listWindows = AutomationElement.RootElement.FindAll(scope, mainForm);
            List<AutomationElement> re = ConvertCollection2List(listWindows);
            RuntimeInstance.isAppOpened = true;
            if (removeOldInRuntime)
            {
                //RuntimeInstance.processTesting = process;
                RuntimeInstance.listRootAutoElement = re;
            }
            return new Tuple<AutomationElementCollection, List<AutomationElement>>(
                listWindows, re);
        }

        public static List<AutomationElement> UpdateElement()
        {
            Condition mainForm = new PropertyCondition(AutomationElement.ProcessIdProperty, RuntimeInstance.processTesting.Id);
            AutomationElementCollection listWindows = AutomationElement.RootElement.FindAll(TreeScope.Subtree, mainForm);

            List<AutomationElement> newWindows = new List<AutomationElement>();
            foreach(AutomationElement window in listWindows)
            {
                //add new window
                if (!RuntimeInstance.listRootAutoElement.Contains(window))
                {
                    RuntimeInstance.listRootAutoElement.Add(window);
                    newWindows.Add(window);
                }
            }
            return newWindows;
        }

        public static string GenerateUUID()
        {
            return System.Guid.NewGuid().ToString();
        }

        public static string NormalizeString(string str)
        {
            string pattern = "[^a-z|A-Z|0-9|_]+";
            Regex regex = new Regex(pattern);
            str = regex.Replace(str, "_");

            Regex regex2 = new Regex("_+");
            str = regex2.Replace(str, "_");
            return str;
        }

        /// <summary>
        /// search automation rootElement
        /// </summary>
        /// <param name="idAndNameCondition"></param>
        /// <returns></returns>
        public static AutomationElement SearchAutomationElement(IdAndNameDesignCondition idAndNameCondition)
        {
            List<AutomationElement> listAutoRootElement = RuntimeInstance.listRootAutoElement;
            if (listAutoRootElement == null || listAutoRootElement.Count == 0)
                listAutoRootElement.Add(AutomationElement.RootElement);
            return SearchAutomationElement(idAndNameCondition, listAutoRootElement);
        }

        /// <summary>
        /// after not found any element, retry search, because maybe another windows appear need to update to RuntimeInstance
        /// </summary>
        /// <param name="idAndNameCondition"></param>
        public static AutomationElement RetrySearchAutomationElementAndUpdate(IdAndNameDesignCondition idAndNameCondition)
        {
            PropertyCondition processCondition = new PropertyCondition(AutomationElement.ProcessIdProperty, RuntimeInstance.processTesting.Id);
            AutomationElementCollection listWindows = AutomationElement.RootElement.FindAll(TreeScope.Children, processCondition);
            RuntimeInstance.listRootAutoElement = ConvertCollection2List(listWindows);
            return DoSearchAutomationElement(idAndNameCondition, RuntimeInstance.listRootAutoElement);
        }

        public static AutomationElement SearchAutomationElement(
            IdAndNameDesignCondition idAndNameCondition, List<AutomationElement> listAutoRootElement)
        {
            var re = DoSearchAutomationElement(idAndNameCondition, listAutoRootElement);
            if (re == null)
                return RetrySearchAutomationElementAndUpdate(idAndNameCondition);
            return re;
        }

        public static AutomationElement DoSearchAutomationElement(
            IdAndNameDesignCondition idAndNameCondition, List<AutomationElement> listAutoRootElement)
        {
            foreach (AutomationElement autoRootElement in listAutoRootElement)
            {
                AutomationElement re = SearchAutomationElement(idAndNameCondition, autoRootElement);
                if (re != null)
                    return re;
            }
            return null;
        }

        public static AutomationElement SearchAutomationElement(IdAndNameDesignCondition idAndNameCondition, AutomationElement rootAutoElement)
        {
            Condition idCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, idAndNameCondition.DesignId);
            Condition nameCondition = new PropertyCondition(AutomationElement.NameProperty, idAndNameCondition.DesignName);
            AndCondition condition = new AndCondition(idCondition, nameCondition);
            AutomationElementCollection listElements = rootAutoElement.FindAll(TreeScope.Subtree, condition);
            if (listElements == null || listElements.Count == 0)
                return null;
            if (listElements.Count > 1)
            {
                logger.Warn("More than one automation rootElement were found with Id: " + idAndNameCondition.DesignId +
                    " and Name: " + idAndNameCondition.DesignName);
            }
            // return first rootElement
            return listElements[0];
        }

        public static List<AutomationElement> ConvertCollection2List(AutomationElementCollection collection)
        {
            List<AutomationElement> re = new List<AutomationElement>();
            foreach (AutomationElement element in collection)
                re.Add(element);
            return re;
        }

        public static string AutoElementToString(AutomationElement element)
        {
            return "Element: " + element.Current.Name + ", id: " + element.Current.AutomationId +
                ", type: " + element.Current.LocalizedControlType;
        }

        public static string TryAutoElementToString(AutomationElement element)
        {
            try
            {
                return AutoElementToString(element);
            } catch (ElementNotAvailableException ex)
            {
                logger.Error("Element not available. " + "Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
                return "";
            }
        }

        public static string ElementToString(IElement element)
        {
            return "Element: " + element.Attributes.Name;
        }

        public static void CheckRuntimeInstance()
        {
            if (RuntimeInstance.testingReportModel == null)
                RuntimeInstance.testingReportModel = new TestingReportModel();
            if (RuntimeInstance.testingReportModel.ListTestModules == null)
                RuntimeInstance.testingReportModel.ListTestModules = new List<TestingModuleReport>();
        }

        public static void AddNewActionReport(IActionReport actionReport)
        {
            if (RuntimeInstance.currentTestingModule == null)
                RuntimeInstance.currentTestingModule = new TestingModuleReport(
                    RuntimeInstance.currentRunningFileTest.Item1, RuntimeInstance.currentRunningFileTest.Item2);
            if (RuntimeInstance.currentTestingModule.ListActionReport == null)
                RuntimeInstance.currentTestingModule.ListActionReport = new List<IActionReport>();
            RuntimeInstance.currentTestingModule.ListActionReport.Add(actionReport);
        }

        public static DirectoryInfo CreateDirectoryForFilePath(string filePath, bool removeIfExisted = false)
        {
            string dirPath = Path.GetDirectoryName(filePath);
            if (removeIfExisted)
                Directory.Delete(dirPath);
            return Directory.CreateDirectory(dirPath);
        }

        public static AutomationElementCollection SearchRootAutoElements(int processId)
        {
            Condition mainForm = new PropertyCondition(AutomationElement.ProcessIdProperty, processId);
            AutomationElementCollection listWindows = AutomationElement.RootElement.FindAll(TreeScope.Children, mainForm);
            return listWindows;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public static void DoMouseClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        public static void MoveCursorToPoint(int x, int y)
        {
            SetCursorPos(x, y);
        }

    }
}
