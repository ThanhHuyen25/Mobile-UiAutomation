// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:10 PM 2018/3/6
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI_Testing_Automation;

namespace TestingReport
{
    public class ViewerPane : WindowPane
    {
        #region Fields
        private ReportViewerPackage _thisPackage;
        private string _fileName = string.Empty;
        private UCReportViewer ucViewer;
        #endregion Fields

        #region "Window.Pane Overrides"
        /// <summary>
        /// Constructor that calls the Microsoft.VisualStudio.Shell.WindowPane constructor then
        /// our initialization functions.
        /// </summary>
        /// <param name="package">Our Package instance.</param>
        public ViewerPane(ReportViewerPackage package, string fileName)
            : base(null)
        {
            _thisPackage = package;
            _fileName = fileName;
        }

        protected override void OnClose()
        {
            Dispose(true);
            base.OnClose();
        }
        #endregion

        /// <summary>
        /// parse file content in order to pass to model
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            TestingReportModel.file_path = _fileName;
            //new ActionReport().
            ucViewer = new UCReportViewer(new ViewModel(TestReportFileLoader.LoadFile(_fileName)));
            Content = ucViewer;
        }

        /// <summary>
        /// returns the name of the file currently loaded
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }
    }
}
