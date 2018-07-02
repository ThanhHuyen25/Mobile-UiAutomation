// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 3:39 PM 2018/5/23
using GUI_Testing_Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GUI_Testing_Automation.Ranorex;

namespace TestingApplication
{
    public class RanorexRxrepAnalyzer
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string ROOT_FOLDER = "rootfolder";
	    private const string APP_FOLDER = "appfolder";
	    private const string FOLDER = "folder";
	    private const string ITEM = "item";
	    private const string NAME = "name";
	    private const string ID = "id";
	    private const string SEARCH_TIME_OUT = "searchtimeout";
	    private const string CAP_NAME = "capname";
	    private const string IS_ROOT = "isrooted";
	    private const string USE_CACHE = "usecache";
	    private const string REF_PATH = "refpath";
	    private const string ADD_CAPS = "addcaps";
	    private const string REF_IMG_ID = "refimgid";
	    private const string BASE_PATH = "basepath";
	    private const string ICON = "icon";
	    private const string CLASSNAME = "classname";
	    private const string CODEGEN = "codegen";
	    private const string NAMESPACE = "namespace";
	    private const string VARIABLES = "variables";

        private const string CONTAINER = "container";
        private const string MENUITEM = "menuitem";
        private const string TABPAGE = "tabpage";
        private const string TITLEBAR = "titlebar";
        private const string LISTITEM = "listitem";
        private const string COMBOBOX = "combobox";
        private const string CHECKBOX = "checkbox";
        private const string ACCESSIBLE = "accessible";
        private const string WPFGROUPELEMENT = "wpfgroupelement";

        // parse file to extract all elements
        public List<IElement> Analyze(string filePath)
        {
            var doc = new XmlDocument();
            doc.Load(filePath);
            var rootFolders = doc.GetElementsByTagName(ROOT_FOLDER);
            if (rootFolders.Count < 1)
            {
                logger.Error("Not found root folder in .rxrep file");
                throw new NullReferenceException();
            }
            if (rootFolders.Count > 1)
                logger.Warn("Found more than one root folder in .rxrep file");
            List<IElement> rootElements = new List<IElement>();
            foreach (XmlNode rootFolderNode in rootFolders)
            {
                if (rootFolderNode == null || rootFolderNode.ChildNodes == null ||
                    rootFolderNode.ChildNodes.Count < 1)
                    continue;
                foreach (XmlNode newNode in rootFolderNode.ChildNodes)
                {
                    var rootEle = Convert2Element(newNode, null);
                    if (rootEle != null)
                        rootElements.Add(rootEle);
                }
            }
            return rootElements;
        }

        public IElement Convert2Element(XmlNode node, IElement parent)
        {
            string tagName = node.Name;
            IElement element = null;
            switch (tagName)
            {
                case APP_FOLDER:
                    element = new AppFolderRanorexElement();
                    break;
                case FOLDER:
                    element = new FolderRanorexElement();
                    break;
                case ITEM:
                    string name = node.Attributes[NAME].Value;
                    string addcaps = node.Attributes[ADD_CAPS].Value;
                    string capname = node.Attributes[CAP_NAME].Value;
                    switch (capname)
                    {
                        case ElementBase.BUTTON:
                            element = new ButtonElement();
                            break;
                        case ElementBase.TEXT:
                            if (addcaps.Contains(ACCESSIBLE) ||
                                    addcaps.Contains(WPFGROUPELEMENT))
                            {
                                element = new EditableTextElement();
                            }
                            else
                            {
                                element = new TextElement();
                            }
                            break;
                        case CONTAINER:
                            element = new ContainerElement();
                            break;
                        case LISTITEM:
                            element = new ListItemElement();
                            break;
                        case MENUITEM:
                            element = new MenuItemElement();
                            break;
                        case TABPAGE:
                            element = new TabPageElement();
                            break;
                        case TITLEBAR:
                            element = new TitleBarElement();
                            break;
                        case CHECKBOX:
                            element = new CheckBoxElement();
                            break;
                        case COMBOBOX:
                            element = new ComboBoxElement();
                            break;
                        default:
                            element = new RanorexUnknowElement();
                            break;
                    }
                    break;
                case VARIABLES:
                    break;
            }
            if (element != null)
            {
                element.Attributes.Name = node.Attributes[NAME].Value;
                if (parent != null)
                {
                    element.Parent = parent;
                    if (parent.Children == null)
                        parent.Children = new List<IElement>();
                    parent.Children.Add(element);
                }
                if (node.HasChildNodes)
                    foreach (XmlNode child in node.ChildNodes)
                        Convert2Element(child, element);
            }
            return element;
        }
    }
}
