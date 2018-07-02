using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GUI_Testing_Automation
{
    public class XmlFilesLoader
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string TRUE = "True";

        public const string IMAGES_TAG = "Images";
        public const string ELEMENTS_TAG = "Elements";

        public static List<IElement> Load(string mainXamlFilePath, string imageCaptureFilePath) 
        {
            ValidateFileExist(mainXamlFilePath, imageCaptureFilePath);

            //Read all data in fileRepo.xml
            XmlDocument xmlDocMain = new XmlDocument();
            xmlDocMain.Load(mainXamlFilePath);

            XmlDocument xmlDocImgCap = new XmlDocument();
            xmlDocImgCap.Load(imageCaptureFilePath);

            Dictionary<string, string> mapIdImageEncoded = LoadImageCapEncoded(xmlDocImgCap);
            return LoadElements(xmlDocMain, mapIdImageEncoded);
        }

        public static Dictionary<string, IElement> Load2(string mainXmlFilePath, string imageCaptureFilePath)
        {
            ValidateFileExist(mainXmlFilePath, imageCaptureFilePath);

            //Read all data in fileRepo.xml
            XmlDocument xmlDocMain = new XmlDocument();
            xmlDocMain.Load(mainXmlFilePath);

            XmlDocument xmlDocImgCap = new XmlDocument();
            xmlDocImgCap.Load(imageCaptureFilePath);

            Dictionary<string, string> mapIdImageEncoded = LoadImageCapEncoded(xmlDocImgCap);
            return LoadElements2(xmlDocMain, mapIdImageEncoded);
        }

        public static void Load3(string mainXmlFilePath, string imageCaptureFilePath, params IElement[] listRootElements)
        {
            Dictionary<string, IElement> mapIdElements = Load2(mainXmlFilePath, imageCaptureFilePath);
            RuntimeInstance.mapIdElements = mapIdElements;
            foreach(IElement rootElement in listRootElements)
            {
                CopyAttributes(rootElement, mapIdElements);
            }
        }

        public static void CopyAttributes(IElement currentElement, Dictionary<string, IElement> mapIdElements)
        {
            currentElement.Attributes = mapIdElements[currentElement.Id].Attributes;
            if (currentElement.Children != null)
                foreach (IElement child in currentElement.Children)
                    CopyAttributes(child, mapIdElements);
        }

        public static void ValidateFileExist(string mainXamlFilePath, string imageCaptureFilePath)
        {
            //if file not exists then cannot extract
            if (!File.Exists(mainXamlFilePath))
            {
                string msg = "Not found file with path: " + mainXamlFilePath;
                logger.Error(msg);
                throw new FileNotFoundException(msg);
            }
            if (!File.Exists(imageCaptureFilePath))
            {
                string msg = "Not found file with path: " + imageCaptureFilePath;
                logger.Error(msg);
                throw new FileNotFoundException(msg);
            }
        }

        public static Dictionary<string, string> LoadImageCapEncoded(XmlDocument xmlDoc)
        {
            Dictionary<string, string> re = new Dictionary<string, string>();
            XmlNode xmlImgsCapNode = xmlDoc.GetElementsByTagName(IMAGES_TAG)[0];

            foreach (XmlNode xmlImgNode in xmlImgsCapNode.ChildNodes)
            {
                if (!xmlImgNode.Name.Equals(IElementProperties.IMAGE_CAPTURE_ENCODED))
                    continue;
                string id = xmlImgNode.Attributes[IElementProperties.ID].Value;
                string content = xmlImgNode.InnerText;
                try
                {
                    re.Add(id, content);
                }
                catch (ArgumentException)
                {
                    logger.Error("An element with Key = " + id + " already exists.");
                }
            }
            return re;
        }

        public static List<IElement> LoadElements(XmlDocument xmlDoc, Dictionary<string, string> mapIdImgCapEncoded)
        {
            XmlNode xmlElementsNode = xmlDoc.GetElementsByTagName(ELEMENTS_TAG)[0];
            //extract all windowRoot from listXmlNodeRoot
            List<IElement> listWindowRoot = new List<IElement>();
            foreach (XmlNode xmlEleNode in xmlElementsNode.ChildNodes)
            {
                IElement window = GetElementWithRelative(xmlEleNode, null, mapIdImgCapEncoded).Item2;
                listWindowRoot.Add(window);
            }
            return listWindowRoot;
        }

        public static Dictionary<string, IElement> LoadElements2(XmlDocument xmlDoc, Dictionary<string, string> mapIdImgCapEncoded)
        {
            XmlNode xmlElementsNode = xmlDoc.GetElementsByTagName(ELEMENTS_TAG)[0];
            //extract all windowRoot from listXmlNodeRoot
            Dictionary<string, IElement> mapIdElements = new Dictionary<string, IElement>();
            foreach (XmlNode xmlEleNode in xmlElementsNode.ChildNodes)
            {
                Tuple <string, IElement> window = GetElementWithRelative(xmlEleNode, null, mapIdImgCapEncoded, mapIdElements);
                //listWindowRoot.Add(window.Item1, window.Item2);
            }
            return mapIdElements;
        }

        /// <summary>
        /// resurcive to convert all XmlNode root and its childs to a window and window's contents.
        /// </summary>
        /// <param name="currentNode"></param>
        /// <returns></returns>
        private static Tuple<string, IElement> GetElementWithRelative(XmlNode currentNode, IElement parent, 
            Dictionary<string, string> mapIdImgCapEncoded, Dictionary<string, IElement> mapIdElements = null)
        {
            Tuple<string, IElement> currentElemTuple = CreateElement(currentNode, mapIdImgCapEncoded);
            IElement currentElem = currentElemTuple.Item2;
            if (parent != null)
                currentElem.Parent = parent;
            currentElem.Children = new List<IElement>();
            if (currentNode.ChildNodes != null)
                foreach (XmlNode node in currentNode.ChildNodes)
                {
                    Tuple<string, IElement> child = GetElementWithRelative(node, currentElem, mapIdImgCapEncoded, mapIdElements);
                    currentElem.Children.Add(child.Item2);
                }
            if (mapIdElements != null)
                mapIdElements.Add(currentElemTuple.Item1, currentElem);
            return currentElemTuple;
        }

        /// <summary>
        /// convert a node to an IElement:
        /// create an IElement and set properties for it
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static Tuple<string, IElement> CreateElement(XmlNode node, Dictionary<string, string> mapIdImgCapEncoded)
        {
            string elementType = node.Name;
            string id = node.Attributes[IElementProperties.ID].Value;
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Type type = assembly.GetType("GUI_Testing_Automation." + elementType);
                IElement element = (IElement)Activator.CreateInstance(type, id);
                SetIElementProperties(element, node);
                try
                {
                    if (mapIdImgCapEncoded[id] != null)
                        element.Attributes.ImageCaptureEncoded = mapIdImgCapEncoded[id];
                }
                catch (KeyNotFoundException)
                {
                    logger.Error("Key = " + id + " is not found.");
                }
                return Tuple.Create(id, element);
            }
            catch (Exception)
            {
                logger.Error("-----ERROR WHEN TRY TO CREATE INSTANCE: " + elementType);
                return null;
            }
        }

        /// <summary>ies
        /// set all propert from a node to element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="node"></param>
        private static void SetIElementProperties(IElement element, XmlNode node)
        {
            element.Attributes.Name = node.Attributes[IElementProperties.NAME].Value;
            element.Attributes.AcceleratorKey = node.Attributes[IElementProperties.ACCELERATOR_KEY].Value;
            element.Attributes.AccessKey = node.Attributes[IElementProperties.ACCESS_KEY].Value;
            element.Attributes.ClassName = node.Attributes[IElementProperties.CLASSNAME].Value;
            element.Attributes.FrameworkId = node.Attributes[IElementProperties.FRAMEWORK_ID].Value;
            element.Attributes.HasKeyboardFocus = node.Attributes[IElementProperties.HAS_KEYBOARD_FOCUS].Value.Equals(TRUE);
            element.Attributes.HelpText = node.Attributes[IElementProperties.HELP_TEXT].Value;
            element.Attributes.IsContentElement = node.Attributes[IElementProperties.IS_CONTENT_ELEMENT].Value.Equals(TRUE);
            element.Attributes.IsControlElement = node.Attributes[IElementProperties.IS_CONTROL_ELEMENT].Value.Equals(TRUE);
            element.Attributes.IsEnabled = node.Attributes[IElementProperties.IS_ENABLED].Value.Equals(TRUE);
            element.Attributes.IsKeyboardFocusable = node.Attributes[IElementProperties.IS_KEYBOARD_FOCUSABLE].Value.Equals(TRUE);
            element.Attributes.IsOffscreen = node.Attributes[IElementProperties.IS_OFFSCREEN].Value.Equals(TRUE);
            element.Attributes.IsPassword = node.Attributes[IElementProperties.IS_PASSWORD].Value.Equals(TRUE);
            element.Attributes.IsRequiredForForm = node.Attributes[IElementProperties.IS_REQUIRED_FOR_FORM].Value.Equals(TRUE);
            element.Attributes.ItemStatus = node.Attributes[IElementProperties.ITEM_STATUS].Value;
            element.Attributes.ItemType = node.Attributes[IElementProperties.ITEM_TYPE].Value;
            element.Attributes.LocalizedControlType = node.Attributes[IElementProperties.LOCALIZED_CONTROL_TYPE].Value;
            element.Attributes.NativeWindowHandle = Int32.Parse(node.Attributes[IElementProperties.NATIVE_WINDOW_HANDLE].Value);
            element.Attributes.ProcessId = Int32.Parse(node.Attributes[IElementProperties.PROCESS_ID].Value);
            //element.ImageCaptureEncoded = node.Attributes[IElementProperties.IMAGE_CAPTURE_ENCODED].Value;
            element.Attributes.DesignedName = node.Attributes[IElementProperties.DESIGN_NAME].Value;
            element.Attributes.DesignedId = node.Attributes[IElementProperties.DESIGN_ID].Value;
            //element.Id = node.Attributes[IElementProperties.ID].Value;
        }
    }
}
