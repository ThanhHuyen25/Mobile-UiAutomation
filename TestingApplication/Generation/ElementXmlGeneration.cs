// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @quypk
// created on 7:41 PM 2017/12/12
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using GUI_Testing_Automation;

namespace TestingApplication
{
    /// <summary>
    /// export inspected elements to .xml file
    /// there are 2 file output, one declares elements, the remain stores ImageCaptureEncoded
    /// </summary>
    public class ElementXmlGeneration
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        // recommend to use relative path instead of absolute path
        public const string DEFAULT_FILE = @"..\..\ElementsRepo.xml";
        public const string DEFAULT_FILE2 = @"..\..\..\ProjectGenTemplate\ElementsRepo.xml";        
        public const string DEFAULT_FILE_IMAGE = @"..\..\..\ProjectGenTemplate\ImageCapture.xml";

        /// <summary>
        /// store window and its content in a specific file
        /// </summary>
        /// <param name="listRootElements"></param>
        /// <param name="repoFilePath"></param>
        /// <param name="imageCaptureFilePath"></param>
        public List<FileInfo> Store(List<IElement> listRootElements, string repoFilePath, string imageCaptureFilePath)
        {
            // remove old file, create new parent directory
            Utils.RemoveOldFile(repoFilePath, false);
            Utils.RemoveOldFile(imageCaptureFilePath, false);

            XmlDocument xmlDocMain = new XmlDocument();
            xmlDocMain.AppendChild(xmlDocMain.CreateXmlDeclaration("1.0", "UTF-8\" standalone=\"yes", ""));

            XmlDocument xmlDocImageCap = new XmlDocument();
            xmlDocImageCap.AppendChild(xmlDocImageCap.CreateXmlDeclaration("1.0", "UTF-8\" standalone=\"yes", ""));

            XmlElement bodyMain = xmlDocMain.CreateElement(XmlFilesLoader.ELEMENTS_TAG);
            XmlElement bodyImageCap = xmlDocImageCap.CreateElement(XmlFilesLoader.IMAGES_TAG);

            foreach (IElement root in listRootElements)
            {
                XmlElement window = ConvertToXML(root, xmlDocMain, xmlDocImageCap, ref bodyImageCap);
                bodyMain.AppendChild(window);
            }
            xmlDocMain.AppendChild(bodyMain);
            xmlDocImageCap.AppendChild(bodyImageCap);

            using (XmlTextWriter writer = new XmlTextWriter(repoFilePath, null))
            {
                writer.Formatting = Formatting.Indented;
                xmlDocMain.Save(writer);
            }
            using (XmlTextWriter writer2 = new XmlTextWriter(imageCaptureFilePath, null))
            {
                writer2.Formatting = Formatting.Indented;
                xmlDocImageCap.Save(writer2);
            }
            List<FileInfo> re = new List<FileInfo>();
            FileInfo repoFile = new FileInfo(repoFilePath);
            FileInfo imgCapFile = new FileInfo(imageCaptureFilePath);
            if (repoFile != null)
                re.Add(repoFile);
            if (imgCapFile != null)
                re.Add(imgCapFile);
            return re;
        }

        /// <summary>
        /// store window and its content in a specific file
        /// </summary>
        /// <param name="root"></param>
        private void Store(List<IElement> listRoot)
        {
            Store(listRoot, DEFAULT_FILE2, DEFAULT_FILE_IMAGE);       
        }

        /// <summary>
        /// Convert element to XML to store window and its content to file.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="xmlDocument"></param>
        /// <param name="xmlDocImageCap"></param>
        /// <param name="bodyImageCap"></param>
        /// <returns></returns>
        private XmlElement ConvertToXML(IElement root, XmlDocument xmlDocument, XmlDocument xmlDocImageCap, ref XmlElement bodyImageCap)
        {
            if (root == null)
            {
                return null;
            }

            XmlElement parent = CreateElement(root, xmlDocument, xmlDocImageCap, ref bodyImageCap);
            if (root.Children.Count != 0)
            {
                foreach (IElement node in root.Children)
                {
                    XmlElement child = ConvertToXML(node, xmlDocument, xmlDocImageCap, ref bodyImageCap);
                    parent.AppendChild(child);
                }
            }
            return parent;
        }

        /// <summary>
        /// create element to store element to file
        /// </summary>
        /// <param name="root"></param>
        /// <param name="xmlDocument"></param>
        /// <param name="xmlDocImageCap"></param>
        /// <param name="bodyImageCap"></param>
        /// <returns></returns>
        private XmlElement CreateElement(IElement root, XmlDocument xmlDocument, XmlDocument xmlDocImageCap, ref XmlElement bodyImageCap)
        {
            string tagName = root.GetType().Name;
            XmlElement element = xmlDocument.CreateElement(tagName);

            XmlAttribute idAttr = xmlDocument.CreateAttribute(IElementProperties.ID);
            idAttr.Value = root.Id;
            element.Attributes.Append(idAttr);

            XmlAttribute nameAttr = xmlDocument.CreateAttribute(IElementProperties.NAME);
            nameAttr.Value = root.Attributes.Name;
            element.Attributes.Append(nameAttr);

            XmlAttribute acceleratorKeyAttr = xmlDocument.CreateAttribute(IElementProperties.ACCELERATOR_KEY);
            acceleratorKeyAttr.Value = root.Attributes.AcceleratorKey;
            element.Attributes.Append(acceleratorKeyAttr);

            XmlAttribute accessKeyAttr = xmlDocument.CreateAttribute(IElementProperties.ACCESS_KEY);
            accessKeyAttr.Value = root.Attributes.AccessKey;
            element.Attributes.Append(accessKeyAttr);

            XmlAttribute classNameAttr = xmlDocument.CreateAttribute(IElementProperties.CLASSNAME);
            classNameAttr.Value = root.Attributes.ClassName;
            element.Attributes.Append(classNameAttr);

            XmlAttribute frameworkIdAttr = xmlDocument.CreateAttribute(IElementProperties.FRAMEWORK_ID);
            frameworkIdAttr.Value = root.Attributes.FrameworkId;
            element.Attributes.Append(frameworkIdAttr);

            XmlAttribute hasKeyboardFocusAttr = xmlDocument.CreateAttribute(IElementProperties.HAS_KEYBOARD_FOCUS);
            hasKeyboardFocusAttr.Value = root.Attributes.HasKeyboardFocus.ToString();
            element.Attributes.Append(hasKeyboardFocusAttr);

            XmlAttribute helpTextAttr = xmlDocument.CreateAttribute(IElementProperties.HELP_TEXT);
            helpTextAttr.Value = root.Attributes.HelpText;
            element.Attributes.Append(helpTextAttr);

            XmlAttribute isContentElementAttr = xmlDocument.CreateAttribute(IElementProperties.IS_CONTENT_ELEMENT);
            isContentElementAttr.Value = root.Attributes.IsContentElement.ToString();
            element.Attributes.Append(isContentElementAttr);

            XmlAttribute isControlElementAttr = xmlDocument.CreateAttribute(IElementProperties.IS_CONTROL_ELEMENT);
            isControlElementAttr.Value = root.Attributes.IsControlElement.ToString();
            element.Attributes.Append(isControlElementAttr);

            XmlAttribute isEnabledAttr = xmlDocument.CreateAttribute(IElementProperties.IS_ENABLED);
            isEnabledAttr.Value = root.Attributes.IsEnabled.ToString();
            element.Attributes.Append(isEnabledAttr);

            XmlAttribute isKeyboardFocusableAttr = xmlDocument.CreateAttribute(IElementProperties.IS_KEYBOARD_FOCUSABLE);
            isKeyboardFocusableAttr.Value = root.Attributes.IsKeyboardFocusable.ToString();
            element.Attributes.Append(isKeyboardFocusableAttr);

            XmlAttribute isOffscreenAttr = xmlDocument.CreateAttribute(IElementProperties.IS_OFFSCREEN);
            isOffscreenAttr.Value = root.Attributes.IsOffscreen.ToString();
            element.Attributes.Append(isOffscreenAttr);

            XmlAttribute isPasswordAttr = xmlDocument.CreateAttribute(IElementProperties.IS_PASSWORD);
            isPasswordAttr.Value = root.Attributes.IsPassword.ToString();
            element.Attributes.Append(isPasswordAttr);

            XmlAttribute isRequireForFormAttr = xmlDocument.CreateAttribute(IElementProperties.IS_REQUIRED_FOR_FORM);
            isRequireForFormAttr.Value = root.Attributes.IsRequiredForForm.ToString();
            element.Attributes.Append(isRequireForFormAttr);

            XmlAttribute itemStatusAttr = xmlDocument.CreateAttribute(IElementProperties.ITEM_STATUS);
            itemStatusAttr.Value = root.Attributes.ItemStatus;
            element.Attributes.Append(itemStatusAttr);

            XmlAttribute itemTypeAttr = xmlDocument.CreateAttribute(IElementProperties.ITEM_TYPE);
            itemTypeAttr.Value = root.Attributes.ItemType;
            element.Attributes.Append(itemTypeAttr);

            XmlAttribute localizedControlTypeChild = xmlDocument.CreateAttribute(IElementProperties.LOCALIZED_CONTROL_TYPE);
            localizedControlTypeChild.Value = root.Attributes.LocalizedControlType;
            //localizedControlTypeChild.InnerText = root.LocalizedControlType;
            element.Attributes.Append(localizedControlTypeChild);

            XmlAttribute nativeWindowHandleAttr = xmlDocument.CreateAttribute(IElementProperties.NATIVE_WINDOW_HANDLE);
            nativeWindowHandleAttr.Value = root.Attributes.NativeWindowHandle.ToString();
            element.Attributes.Append(nativeWindowHandleAttr);

            XmlAttribute processIdAttr = xmlDocument.CreateAttribute(IElementProperties.PROCESS_ID);
            processIdAttr.Value = root.Attributes.ProcessId.ToString();
            element.Attributes.Append(processIdAttr);

            XmlAttribute designIdAttr = xmlDocument.CreateAttribute(IElementProperties.DESIGN_ID);
            designIdAttr.Value = root.Attributes.DesignedId;
            element.Attributes.Append(designIdAttr);

            XmlAttribute designNameAttr = xmlDocument.CreateAttribute(IElementProperties.DESIGN_NAME);
            designNameAttr.Value = root.Attributes.DesignedName;
            element.Attributes.Append(designNameAttr);

            XmlElement imageCaptureEncodedChild = xmlDocImageCap.CreateElement(IElementProperties.IMAGE_CAPTURE_ENCODED);
            imageCaptureEncodedChild.InnerText = root.Attributes.ImageCaptureEncoded;
            XmlAttribute idAttr1 = xmlDocImageCap.CreateAttribute(IElementProperties.ID);
            idAttr1.Value = root.Id;
            imageCaptureEncodedChild.Attributes.Append(idAttr1);
            bodyImageCap.AppendChild(imageCaptureEncodedChild);

            return element;
        }
    }
}
