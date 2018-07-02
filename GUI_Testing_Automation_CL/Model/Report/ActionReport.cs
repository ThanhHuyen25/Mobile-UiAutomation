// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:19 PM 2018/2/25
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GUI_Testing_Automation
{
    public class ActionReport : IActionReport
    {
        public const string STATUS_FAILTURE = "Failure";
        public const string STATUS_SUCCESS = "Success";
        public const string STATUS_ERROR = "Error";

        public const string CATEGORY_VALIDATION = "Validatation";
        public const string CATEGORY_PROCEDURE = "Procedure";

        string message;
        string status;
        string category;
        string imageCapPath;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
            }
        }
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
            }
        }
        public string Category
        {
            get { return category; }
            set
            {
                category = value;
            }
        }
        public string ImageCapPath
        {
            get { return imageCapPath; }
            set
            {
                imageCapPath = value;
            }
        }
        public Brush BgColor
        {
            get
            {
                return GetBgColor();
            }
        }
        public Brush FgColor
        {
            get
            {
                return GetFgColor();
            }
        }
        public string AbsoluteImgPath
        {
            get
            {
                return GetAbsoluteImgPath();
            }
        }

        public ActionReport(string message, string status, string category, string imageCapPath = null)
        {
            this.message = message;
            this.status = status;
            this.category = category;
            if (imageCapPath != null)
                this.imageCapPath = imageCapPath;
        }

        public string GetAbsoluteImgPath()
        {
            if (imageCapPath == null || imageCapPath == "")
                return null;
            string guilogFilePath = TestingReportModel.file_path;
            //System.Diagnostics.Debug.WriteLine("a----------: " + guilogFilePath);
            string reportsFolderPath = Path.GetDirectoryName(guilogFilePath);
            return Path.Combine(reportsFolderPath, imageCapPath);
        }

        public Brush GetBgColor()
        {
            if (status.Equals(STATUS_ERROR) || status.Equals(STATUS_FAILTURE))
                return new SolidColorBrush(Colors.Red);
            else
                return new SolidColorBrush(Colors.White);
        }
        public Brush GetFgColor()
        {
            if (status.Equals(STATUS_ERROR) || status.Equals(STATUS_FAILTURE))
                return new SolidColorBrush(Colors.White);
            else
                return new SolidColorBrush(Colors.Green);
        }
        public bool IsPass()
        {
            return status.Equals(STATUS_SUCCESS);
        }
    }
}
