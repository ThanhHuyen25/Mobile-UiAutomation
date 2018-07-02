// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 11:08 AM 2018/2/26
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace GUI_Testing_Automation
{
    public class TestReportFileLoader
    {
        /// <summary>
        /// load model from xml file
        /// </summary>
        /// <param name="filePath">file's path, view TestingReport/sampleFile.guilog</param>
        /// <returns></returns>
        public static TestingReportModel LoadFile(string filePath)
        {
            var doc = new XmlDocument();

            //load content of the file.
            try
            {
                doc.Load(filePath);
            }
            catch (Exception ex)
            {
                return null;
            }

            //validate content of the file.
            XmlNode root = null;
            string rootTag = "Report";
            root = (doc.FirstChild != null) && (doc.FirstChild.Name != rootTag) ? doc.FirstChild.NextSibling : doc.FirstChild;
            if (root == null || root.FirstChild == null) return null;

            List<TestingModuleReport> listTestingModule = new List<TestingModuleReport>();
            XmlNodeList modules = root.FirstChild.ChildNodes;
            foreach (XmlNode module in modules)
            {
                string moduleName = module.Attributes["Name"].Value;
                string relativeClass = module.Attributes["RelativeFilePath"].Value;

                if (module.FirstChild != null
                    && module.FirstChild.ChildNodes.Count > 0)
                {
                    List<IActionReport> listActionReport = new List<IActionReport>();
                    XmlNodeList actions = module.FirstChild.ChildNodes;
                    foreach (XmlNode action in actions)
                    {
                        string message = action.Attributes["Message"].Value;
                        string status = action.Attributes["Status"].Value;
                        string category = action.Attributes["Category"].Value;
                        string imageCapturePath = action.Attributes["ImageCapturePath"] != null ? action.Attributes["ImageCapturePath"].Value : null;
                        listActionReport.Add(new ActionReport(message, status, category, imageCapturePath));
                    }

                    listTestingModule.Add(new TestingModuleReport(moduleName, listActionReport, relativeClass));
                }
            }

            string modelName = root.Attributes["Name"].Value;
            return new TestingReportModel(modelName, listTestingModule);
        }

        /// <summary>
        /// write model to .guilog file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="testingReportModel"></param>
        /// <returns></returns>
        public static bool WriteFile(string filePath, TestingReportModel testingReportModel)
        {
            XmlDocument doc = new XmlDocument();

            XmlElement model = doc.CreateElement("Report");
            XmlAttribute modelName = doc.CreateAttribute("Name");
            modelName.Value = testingReportModel.Name;
            model.Attributes.Append(modelName);
            doc.AppendChild(model);

            XmlElement moduleCollection = doc.CreateElement("ModuleCollection");
            model.AppendChild(moduleCollection);

            List<TestingModuleReport> listTestingModule = testingReportModel.ListTestModules;
            if (listTestingModule != null)
            {
                foreach (TestingModuleReport testingModule in listTestingModule)
                {
                    XmlElement module = doc.CreateElement("Module");
                    moduleCollection.AppendChild(module);

                    XmlAttribute moduleName = doc.CreateAttribute("Name");
                    moduleName.Value = testingModule.Name;

                    XmlAttribute relativeFilePath = doc.CreateAttribute("RelativeFilePath");
                    relativeFilePath.Value = testingModule.RelativeFilePath;

                    module.Attributes.Append(moduleName);
                    module.Attributes.Append(relativeFilePath);

                    XmlElement actionCollection = doc.CreateElement("ActionCollection");
                    module.AppendChild(actionCollection);

                    List<IActionReport> listActionReport = testingModule.ListActionReport;
                    if (listActionReport != null)
                    {
                        foreach (IActionReport actionReport in listActionReport)
                        {
                            XmlElement action = doc.CreateElement("Action");
                            actionCollection.AppendChild(action);

                            XmlAttribute category = doc.CreateAttribute("Category");
                            category.Value = actionReport.Category;

                            XmlAttribute status = doc.CreateAttribute("Status");
                            status.Value = actionReport.Status;

                            XmlAttribute message = doc.CreateAttribute("Message");
                            message.Value = actionReport.Message;

                            action.Attributes.Append(category);
                            action.Attributes.Append(status);
                            action.Attributes.Append(message);

                            if (actionReport.ImageCapPath != null)
                            {
                                XmlAttribute imageCapturePath = doc.CreateAttribute("ImageCapturePath");
                                imageCapturePath.Value = actionReport.ImageCapPath;
                                action.Attributes.Append(imageCapturePath);
                            }
                        }
                    }
                }
            }

            XElement xdoc = XElement.Parse(doc.OuterXml);
            xdoc.Save(filePath);

            return true;
        }
    }
}
