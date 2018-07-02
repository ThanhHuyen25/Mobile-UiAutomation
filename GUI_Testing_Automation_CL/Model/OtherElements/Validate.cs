// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:18 PM 2018/1/10
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace GUI_Testing_Automation
{
    /// <summary>
    /// represent for validate action
    /// </summary>
    public class Validate
    {
        /// <summary>
        /// check if an element is existed
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool Exists(IElement element)
        {
            AutomationElement autoElement = GUI_Utils.SearchAutomationElement(
                new IdAndNameDesignCondition(element.Attributes.DesignedId, element.Attributes.DesignedName));
            GUI_Utils.CheckRuntimeInstance();
            string message = "Check Exist of " + element.Attributes.Name;
            string status = ActionReport.STATUS_FAILTURE;
            string imgPath = null;
            bool check = (autoElement != null && !autoElement.Current.IsOffscreen);
            if (check)
            {
                message += " successfully";
                status = ActionReport.STATUS_SUCCESS;
            } else
            {
                message += " not successfully";
                status = ActionReport.STATUS_FAILTURE;
                imgPath = CaptureScreen();
            }
            GUI_Utils.AddNewActionReport(new ActionReport(
                message, status, ActionReport.CATEGORY_VALIDATION, imgPath));
            return check;
        }

        public static bool NotExists(IElement element)
        {
            AutomationElement autoElement = GUI_Utils.SearchAutomationElement(
                new IdAndNameDesignCondition(element.Attributes.DesignedId, element.Attributes.DesignedName));
            GUI_Utils.CheckRuntimeInstance();
            string message = "Check NotExist of " + element.Attributes.Name;
            string status = ActionReport.STATUS_FAILTURE;
            string imgPath = null;
            bool check = (autoElement == null || autoElement.Current.IsOffscreen);
            if (check)
            {
                message += " successfully";
                status = ActionReport.STATUS_SUCCESS;
            }
            else
            {
                message += " not successfully";
                status = ActionReport.STATUS_FAILTURE;
                imgPath = CaptureScreen();
            }
            GUI_Utils.AddNewActionReport(new ActionReport(
                message, status, ActionReport.CATEGORY_VALIDATION, imgPath));
            return check;
        }

        public static bool ExistsWithIdOnly(IElement element)
        {
            List<AutomationElement> autoElement = Search.SearchListAutomationElementsOnlyMatchId(
                element);
            string message = "Check ExistWidthIdOnly of " + element.Attributes.Name;
            string status = ActionReport.STATUS_FAILTURE;
            string imgPath = null;
            bool check = autoElement != null && autoElement.Count > 0;
            if (check)
            {
                message += " successfully";
                status = ActionReport.STATUS_SUCCESS;
            }
            else
            {
                message += " not successfully";
                status = ActionReport.STATUS_FAILTURE;
                imgPath = CaptureScreen();
            }
            GUI_Utils.AddNewActionReport(new ActionReport(
                message, status, ActionReport.CATEGORY_VALIDATION, imgPath));
            return check;
        }

        public static bool TextEquals(IElement element, string value, StringComparison stringComparison = StringComparison.Ordinal)
        {
            // export report here
            string textAttribute = element.GetText();
            if (textAttribute == null)
                return false;
            string message = "Check " + element.Attributes.Name + "'s text equals with '" + value + "' is";
            string status = ActionReport.STATUS_FAILTURE;
            string imgPath = null;
            bool check = textAttribute.Equals(value, stringComparison);
            if (check)
            {
                message += " successful";
                status = ActionReport.STATUS_SUCCESS;
            }
            else
            {
                message += " not successful";
                status = ActionReport.STATUS_FAILTURE;
                imgPath = CaptureScreen();
            }
            GUI_Utils.AddNewActionReport(new ActionReport(
                message, status, ActionReport.CATEGORY_VALIDATION, imgPath));
            return check;
        }

        public static bool TextNotEquals(IElement element, string value, StringComparison stringComparison = StringComparison.Ordinal)
        {
            // export report here
            string textAttribute = element.GetText();
            if (textAttribute == null)
                return false;
            string message = "Check " + element.Attributes.Name + "'s text equals with '" + value + "' is";
            string status = ActionReport.STATUS_FAILTURE;
            string imgPath = null;
            bool check = !textAttribute.Equals(value, stringComparison);
            if (check)
            {
                message += " successful";
                status = ActionReport.STATUS_SUCCESS;
            }
            else
            {
                message += " not successful";
                status = ActionReport.STATUS_FAILTURE;
                imgPath = CaptureScreen();
            }
            GUI_Utils.AddNewActionReport(new ActionReport(
                message, status, ActionReport.CATEGORY_VALIDATION, imgPath));
            return check;
        }

        public static bool TextContains(IElement element, string value)
        {
            // export report here
            string textAttribute = element.GetText();
            if (textAttribute == null)
                return false;
            string message = "Check " + element.Attributes.Name + "'s text contains '" + value + "' is";
            string status = ActionReport.STATUS_FAILTURE;
            string imgPath = null;
            bool check = textAttribute.Contains(value);
            if (check)
            {
                message += " successful";
                status = ActionReport.STATUS_SUCCESS;
            }
            else
            {
                message += " not successful";
                status = ActionReport.STATUS_FAILTURE;
                imgPath = CaptureScreen();
            }
            GUI_Utils.AddNewActionReport(new ActionReport(
                message, status, ActionReport.CATEGORY_VALIDATION, imgPath));
            return check;
        }

        public static bool TextNotContains(IElement element, string value)
        {
            // export report here
            string textAttribute = element.GetText();
            if (textAttribute == null)
                return false;
            string message = "Check " + element.Attributes.Name + "'s text not contains '" + value + "' is";
            string status = ActionReport.STATUS_FAILTURE;
            string imgPath = null;
            bool check = !textAttribute.Contains(value);
            if (check)
            {
                message += " successful";
                status = ActionReport.STATUS_SUCCESS;
            }
            else
            {
                message += " not successful";
                status = ActionReport.STATUS_FAILTURE;
                imgPath = CaptureScreen();
            }
            GUI_Utils.AddNewActionReport(new ActionReport(
                message, status, ActionReport.CATEGORY_VALIDATION, imgPath));
            return check;
        }

        public static bool WidthEquals(IElement element, double value)
        {
            // export report here
            double realWidth = element.GetWidth();
            string message = "Check " + element.Attributes.Name + "'s width equals with " + value + " is";
            string status = ActionReport.STATUS_FAILTURE;
            string imgPath = null;
            bool check = realWidth == value;
            if (check)
            {
                message += " successful";
                status = ActionReport.STATUS_SUCCESS;
            }
            else
            {
                message += " not successful";
                status = ActionReport.STATUS_FAILTURE;
                imgPath = CaptureScreen();
            }
            GUI_Utils.AddNewActionReport(new ActionReport(
                message, status, ActionReport.CATEGORY_VALIDATION, imgPath));
            return check;
        }

        public static bool HeightEquals(IElement element, double value)
        {
            // export report here
            double realHeight = element.GetHeight();
            string message = "Check " + element.Attributes.Name + "'s height equals with " + value + " is";
            string status = ActionReport.STATUS_FAILTURE;
            string imgPath = null;
            bool check = realHeight == value;
            if (check)
            {
                message += " successful";
                status = ActionReport.STATUS_SUCCESS;
            }
            else
            {
                message += " not successful";
                status = ActionReport.STATUS_FAILTURE;
                imgPath = CaptureScreen();
            }
            GUI_Utils.AddNewActionReport(new ActionReport(
                message, status, ActionReport.CATEGORY_VALIDATION, imgPath));
            return check;
        }

        public static string CaptureScreen()
        {
            string relativePath = @"Images\Screen_" + DateTime.Now.ToString("ddMMyy_HHmmss") + ".png";
            string path = Path.Combine(@"..\..\Reports", relativePath);
            GUI_Utils.CreateDirectoryForFilePath(path, false);
            bool success = CaptureElement.CaptureAllScreen(path);
            if (!success)
                return null;
            return relativePath;
        }
    }
}
