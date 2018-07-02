// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 10:42 AM 2017/12/13
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Automation;
using GUI_Testing_Automation;

namespace TestingApplication
{
    public class ElementsAnalyzer : IElementsAnalyzer
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string ROOT_PATH = "Root";
        public const string SLASH = ElementPath.SLASH;

        private const int DELAY_INSPECTING_TIME = 100;

        //private CaptureElement captureElement = new CaptureElement();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootAutomationElement"></param>
        /// <returns></returns>
        public List<IElement> Analyzing(
            AutomationElementCollection listRootAutomationElement,
            ElementDiscoverManual elementDiscorerManual)
        {
            //logger.Debug("Start Analyze.....");
            List<IElement> re = new List<IElement>();
            Thread.Sleep(DELAY_INSPECTING_TIME);
            foreach (AutomationElement rootAutoElement in listRootAutomationElement)
            {
                IElement newElement = Analyzing(rootAutoElement, elementDiscorerManual);
                if (newElement != null)
                    re.Add(newElement);
            }
            //logger.Debug("Finish Analyze.....");
            return re;
        }

        public IElement Analyzing(AutomationElement rootAutoElement, ElementDiscoverManual elementDiscorerManual)
        {
            //TODO: fix bound
            //captureElement.WindowBound = rootAutoElement.Current.BoundingRectangle;
            //logger.Debug("Start Window.....");
            try
            {
                rootAutoElement.SetFocus();
            }
            catch (InvalidOperationException)
            {
                logger.Error("Element: " + rootAutoElement.Current.Name + ", id: " +
                    rootAutoElement.Current.AutomationId + ", type: " + rootAutoElement.Current.LocalizedControlType +
                    "cannot receive focus");
            }
            IElement rootEle = Convert(rootAutoElement, null, new Dictionary<string, int>());
            //logger.Debug("Start add struct change event.....");
            elementDiscorerManual.AddEvent(rootAutoElement);
            //logger.Debug("Finish Window.....");
            return rootEle;
        }

        /// <summary>
        /// resurcive to transfer all automation element in order to buiding a tree.
        /// </summary>
        /// <param name="currentAutoEle"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IElement Convert(AutomationElement currentAutoEle, IElement parent, Dictionary<string, int> mapUnknowCounter, bool add2Runtime = true)
        {
            //logger.Debug("Start convert...");
            IElement re = DoConvert(currentAutoEle, parent, mapUnknowCounter, add2Runtime);
            //logger.Debug("Finish convert...");
            ShowHiddenElements(currentAutoEle);
            AutomationElementCollection children = currentAutoEle.FindAll(TreeScope.Children, Condition.TrueCondition);
            //logger.Debug("Finish find children...");
            foreach (AutomationElement autoChild in children)
            {
                IElement child = Convert(autoChild, re, mapUnknowCounter, add2Runtime);
                //if (child != null)
                //re.Children.Add(child);
            }
            //logger.Debug("Finish...");
            return re;
        }

        private IElement DoConvert(AutomationElement autoElement, IElement parent, Dictionary<string, int> mapUnknowCounter, bool add2Runtime = true)
        {
            IElement re = ConvertElement(autoElement, parent, mapUnknowCounter);
            if (add2Runtime)
            {
                RuntimeInstance.listAutoElement.Add(autoElement);
                try
                {
                    RuntimeInstance.mappingUIElement.Add(autoElement, re);
                }
                catch (ArgumentException)
                {
                    logger.Error("An element with Key = " + GUI_Utils.TryAutoElementToString(autoElement) +
                        " already exists.");
                }
            }
            if (re == null)
                return null;
            if (parent == null)
                re.Attributes.ElementPath = new ElementPath(re.Attributes.Name);
            else
            {
                re.Attributes.ElementPath = parent.Attributes.ElementPath.AppendWith(re.Attributes.Name);
                if (parent.Children == null)
                    parent.Children = new List<IElement>();
                parent.Children.Add(re);
            }
            re.Parent = parent;
            /**
             * when 2 or more elements have the same name
             **/
            NormalizeChildrenName(re.Children);
            return re;
        }

        private void ShowHiddenElements(AutomationElement hiddenElem)
        {
            if (hiddenElem.Current.LocalizedControlType.ToLower() == ElementBase.TREE_ITEM)
            {
                Object tree;
                hiddenElem.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out tree);
                if (tree != null)
                {
                    ExpandCollapsePattern treeConvert = (ExpandCollapsePattern)tree;
                    if (treeConvert.Current.ExpandCollapseState == ExpandCollapseState.Collapsed)
                    {
                        treeConvert.Expand();
                    }
                }
                hiddenElem.SetFocus();
            }
            else if (hiddenElem.Current.LocalizedControlType.ToLower() == ElementBase.TAB_ITEM)
            {
                Object tabSelection;
                hiddenElem.TryGetCurrentPattern(SelectionItemPattern.Pattern, out tabSelection);
                if (tabSelection != null)
                {
                    ((SelectionItemPattern)tabSelection).Select();
                }
            }
            else if (hiddenElem.Current.LocalizedControlType.ToLower() == ElementBase.COMBO_BOX)
            {               
                //try
                //{
                //    ElementBase.Click(hiddenElem, ClickOptions.RightElement);
                //    ElementBase.Click(hiddenElem, ClickOptions.RightElement);
                //}
                //catch (Exception) { }

                Object combo_box_Pattern;
                hiddenElem.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out combo_box_Pattern);
                if (combo_box_Pattern != null)
                {
                    ((ExpandCollapsePattern)combo_box_Pattern).Expand();
                }
            }
        }

        /// <summary>
        /// convert automation element to a special IElement type
        /// </summary>
        /// <param name="automationElement"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private IElement ConvertElement(AutomationElement automationElement, IElement parent, Dictionary<string, int> mapUnknowCounter)
        {
            try
            {
                string localizedType = automationElement.Current.LocalizedControlType;
                string designedId = automationElement.Current.AutomationId;
                string designedName = automationElement.Current.Name;
                //if ((designedId == null || designedId.Equals("")) &&
                //    (designedName == null || designedName.Equals("")))
                //    return null;
                IElement element = ParseElement(localizedType, designedId, designedName, parent);
                if (element == null)
                    return null;
                SetElementProperties(element, automationElement, mapUnknowCounter);
                return element;
            }
            catch (ElementNotAvailableException e)
            {
                logger.Error(e.StackTrace);
                return null;
            }
            //catch (Exception ex)
            //{
            //string msg = "Exception: " + ex.StackTrace;
            //logger.Error(msg);
            //}
            //return null;
        }

        public static IElement ParseElement(string localizedType, string designedId, string designedName, IElement parent)
        {
            IElement element = null;
            switch (localizedType.ToLower())
            {
                case ElementBase.WINDOW:
                    element = new WindowElement();
                    element.Attributes.ElementType = ElementBase.WINDOW_TYPE;
                    break;
                case ElementBase.DIALOG:
                    element = new WindowElement();
                    element.Attributes.ElementType = ElementBase.DIALOG_TYPE;
                    break;
                case ElementBase.BUTTON:
                    element = new ButtonElement();
                    element.Attributes.ElementType = ElementBase.BUTTON_TYPE;
                    break;
                case ElementBase.COMBO_BOX:
                    element = new ComboBoxElement();
                    element.Attributes.ElementType = ElementBase.COMBOBOX_TYPE;
                    break;
                case ElementBase.TEXT:
                    element = new TextElement();
                    element.Attributes.ElementType = ElementBase.TEXT_TYPE;
                    break;
                case ElementBase.PANE:
                    element = new ContainerElement();
                    element.Attributes.ElementType = ElementBase.CONTAINER_TYPE;
                    break;
                case ElementBase.TITLE_BAR:
                    element = new TitleBarElement();
                    element.Attributes.ElementType = ElementBase.TITLEBAR_TYPE;
                    break;
                case ElementBase.MENU_BAR:
                    element = new MenuBarElement();
                    element.Attributes.ElementType = ElementBase.MENUBAR_TYPE;
                    break;
                case ElementBase.DOCUMENT:
                    element = new EditableTextElement();
                    element.Attributes.ElementType = ElementBase.EDITABLETEXT_TYPE;
                    break;
                case ElementBase.TAB:
                    element = new TabPageElement();
                    element.Attributes.ElementType = ElementBase.TABPAGE_TYPE;
                    break;
                case ElementBase.TAB_ITEM:
                    element = new TabItemElement();
                    element.Attributes.ElementType = ElementBase.TABITEM_TYPE;
                    break;
                case ElementBase.SCROLL_BAR:
                    element = new ScrollBarElement();
                    element.Attributes.ElementType = ElementBase.SCROLLBAR_TYPE;
                    break;
                //case THUMB:
                case ElementBase.TREE:
                    element = new TreeViewElement();
                    element.Attributes.ElementType = ElementBase.TREEVIEW_TYPE;
                    break;
                case ElementBase.TREE_VIEW:
                    element = new TreeViewElement();
                    element.Attributes.ElementType = ElementBase.TREEVIEW_TYPE;
                    break;
                case ElementBase.TREE_ITEM:
                    element = new TreeItemElement();
                    element.Attributes.ElementType = ElementBase.TREEITEM_TYPE;
                    break;
                case ElementBase.TABLE:
                    element = new TableElement();
                    element.Attributes.ElementType = ElementBase.TABLE_TYPE;
                    break;
                //?
                case ElementBase.HEADER:
                    element = new HeaderElement();
                    element.Attributes.ElementType = ElementBase.HEADER_TYPE;
                    break;
                case ElementBase.ITEM:
                    if (parent != null && (parent is TableElement || parent is DatagridElement))
                    {
                        element = new RowElement();
                        element.Attributes.ElementType = ElementBase.ROW_TYPE;
                    }
                    break;
                case ElementBase.LIST: //(listview or checkedlistbox)
                    element = new ListElement();
                    element.Attributes.ElementType = ElementBase.LIST_TYPE;
                    break;
                case ElementBase.LIST_VIEW: //(listview or checkedlistbox)
                    element = new ListElement();
                    element.Attributes.ElementType = ElementBase.LIST_TYPE;
                    break;
                case ElementBase.LIST_ITEM: //(table)
                    element = new ListItemElement();
                    element.Attributes.ElementType = ElementBase.LISTITEM_TYPE;
                    break;
                case ElementBase.EDIT: //textbox
                    element = new EditableTextElement();
                    element.Attributes.ElementType = ElementBase.EDITABLETEXT_TYPE;
                    break;
                case ElementBase.CHECK_BOX:
                    element = new CheckBoxElement();
                    element.Attributes.ElementType = ElementBase.CHECKBOX_TYPE;
                    break;
                case ElementBase.RADIO_BUTTON:
                    element = new RadioButtonElement();
                    element.Attributes.ElementType = ElementBase.RADIO_BUTTON_TYPE;
                    break;
                case ElementBase.CALENDAR:
                    element = new CalendarElement();
                    element.Attributes.ElementType = "Calendar";
                    break;
                case ElementBase.CUSTOM:
                    element = new CustomElement();
                    element.Attributes.ElementType = "Custom";
                    break;
                case ElementBase.DATAGRID:
                    element = new DatagridElement();
                    element.Attributes.ElementType = "DataGrid";
                    break;
                case ElementBase.DATAGRID2:
                    element = new DatagridElement();
                    element.Attributes.ElementType = "DataGrid";
                    break;
                case ElementBase.DATAITEM:
                    element = new DataitemElement();
                    element.Attributes.ElementType = "dataitem";
                    break;
                case ElementBase.GROUP:
                    element = new GroupELement();
                    element.Attributes.ElementType = ElementBase.GROUP_TYPE;
                    break;
                case ElementBase.HEADER_ITEM:
                    element = new HeaderItemElement();
                    element.Attributes.ElementType = "HeaderItem";
                    break;
                case ElementBase.HYPERLINK:
                    element = new LinkElement();
                    element.Attributes.ElementType = "Hyperlink";
                    break;
                case ElementBase.IMAGE:
                    element = new ImageElement();
                    element.Attributes.ElementType = "Image";
                    break;
                case ElementBase.MENU:
                    element = new MenuElement();
                    element.Attributes.ElementType = "Menu";
                    break;
                case ElementBase.PROGRESS_BAR:
                    element = new ProgressBarElement();
                    element.Attributes.ElementType = "ProgressBar";
                    break;
                case ElementBase.SEPARATOR:
                    element = new SeparatorElement();
                    element.Attributes.ElementType = "Separator";
                    break;
                case ElementBase.SLIDER:
                    element = new SliderElement();
                    element.Attributes.ElementType = "Slider";
                    break;
                case ElementBase.SPINNER:
                    element = new SpinnerElement();
                    element.Attributes.ElementType = "Spinner";
                    break;
                case ElementBase.SPLIT_BUTTON:
                    element = new SplitButtonElement();
                    element.Attributes.ElementType = "SplitButton";
                    break;
                case ElementBase.STATUS_BAR:
                    element = new StatusBarElement();
                    element.Attributes.ElementType = "StatusBar";
                    break;
                case ElementBase.TOOL_BAR:
                    element = new ToolBarElement();
                    element.Attributes.ElementType = "ToolBar";
                    break;
                case ElementBase.TOOL_TIP:
                    element = new ToolTipElement();
                    element.Attributes.ElementType = "ToolTip";
                    break;
                //?
                case ElementBase.MENU_ITEM:
                    element = new MenuItemElement();
                    element.Attributes.ElementType = "MenuItem";
                    break;
                case ElementBase.LINK:
                    element = new LinkElement();
                    element.Attributes.ElementType = ElementBase.LINK_TYPE;
                    break;
            }
            if (element == null)
            {
                if (parent is TableElement || parent is DatagridElement)
                {
                    element = new RowElement();
                    element.Attributes.ElementType = ElementBase.ROW_TYPE;
                }
                else if (parent is RowElement)
                {
                    element = new CellElement();
                    element.Attributes.ElementType = ElementBase.CELL_TYPE;
                }
                else
                {
                    element = new UnknownELement();
                    element.Attributes.ElementType = ElementBase.UNKNOWN_TYPE;
                }
            }
            element.Attributes.DesignedId = designedId;
            return element;
        }

        public const int MAX_LENGTH_DESIGN_NAME = 30;
        public const int LENGTH_TO_CUT_NAME = 20;
        private string GetNameElement(IElement element, Dictionary<string, int> mapUnknowCounter)
        {
            string elementType = element.Attributes.ElementType;
            string rawName = "";
            if (element.Attributes.DesignedName != null && element.Attributes.DesignedName != "")
            {
                if (element.Attributes.DesignedName.Length > MAX_LENGTH_DESIGN_NAME)
                    // TODO: implement better cut string here
                    rawName = element.Attributes.DesignedName.Substring(0, MAX_LENGTH_DESIGN_NAME);
                else
                    rawName = element.Attributes.DesignedName;
            }
            else if (element.Attributes.DesignedId != null && element.Attributes.DesignedId != "")
                rawName = element.Attributes.DesignedId;
            else
            {
                int counter = 0;
                mapUnknowCounter.TryGetValue(elementType, out counter);
                counter++;
                mapUnknowCounter[elementType] = counter;
                rawName = "Unknow_" + counter;
            }
            return
                (elementType + " " + GUI_Utils.NormalizeString(rawName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="autoElement"></param>
        /// <returns></returns>
        private bool SetElementProperties(IElement element, AutomationElement autoElement, Dictionary<string, int> mapUnknowCounter)
        {
            element.Attributes.DesignedId = autoElement.Current.AutomationId;
            element.Attributes.DesignedName = autoElement.Current.Name;
            element.Attributes.AcceleratorKey = autoElement.Current.AcceleratorKey;
            element.Attributes.AccessKey = autoElement.Current.AccessKey;
            element.Attributes.ClassName = autoElement.Current.ClassName;
            element.Attributes.FrameworkId = autoElement.Current.FrameworkId;
            element.Attributes.HasKeyboardFocus = autoElement.Current.HasKeyboardFocus;
            element.Attributes.HelpText = autoElement.Current.HelpText;
            element.Attributes.IsContentElement = autoElement.Current.IsContentElement;
            element.Attributes.IsControlElement = autoElement.Current.IsControlElement;
            element.Attributes.IsEnabled = autoElement.Current.IsEnabled;
            element.Attributes.IsKeyboardFocusable = autoElement.Current.IsKeyboardFocusable;
            element.Attributes.IsOffscreen = autoElement.Current.IsOffscreen;
            element.Attributes.IsPassword = autoElement.Current.IsPassword;
            element.Attributes.IsRequiredForForm = autoElement.Current.IsRequiredForForm;
            element.Attributes.ItemStatus = autoElement.Current.ItemStatus;
            element.Attributes.ItemType = autoElement.Current.ItemType;
            element.Attributes.LocalizedControlType = autoElement.Current.LocalizedControlType;
            element.Attributes.NativeWindowHandle = autoElement.Current.NativeWindowHandle;
            element.Attributes.ProcessId = autoElement.Current.ProcessId;
            element.Id = GUI_Utils.GenerateUUID();
            element.Attributes.Name = GetNameElement(element, mapUnknowCounter);

            //get element's screenshot, store as encoded base64
            //ignore path to save image
            var rectBound = autoElement.Current.BoundingRectangle;
            element.Attributes.RectBounding = rectBound;
            string strEncoded = CaptureElement.CaptureScreen(rectBound, null);
            element.Attributes.ImageCaptureEncoded = strEncoded;
            return true;
        }


        /// <summary>
        /// determine if duplicate name of sibling element
        /// </summary>
        /// <param name="listElement"></param>
        private void NormalizeChildrenName(List<IElement> listElement)
        {
            foreach (IElement element in listElement)
            {
                List<int> indexOccurrence = SearchOccurrence(listElement, element);
                if (indexOccurrence.Count > 1)
                {
                    logger.Warn("Found 2 or more element with the same name: " + listElement[indexOccurrence[0]].Attributes.Name);
                    for (int fi = 0; fi < indexOccurrence.Count; fi++)
                    {
                        listElement[indexOccurrence[fi]].Attributes.Name = listElement[indexOccurrence[fi]].Attributes.Name + (fi + 1).ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listElement"></param>
        /// <param name="elementCompare"></param>
        /// <returns> list of index occurrenced </returns>
        private List<int> SearchOccurrence(List<IElement> listElement, IElement elementCompare)
        {
            List<int> re = new List<int>();
            for (int fi = 0; fi < listElement.Count; fi++)
            {
                IElement element = listElement[fi];
                if (element.Attributes.Name.Equals(elementCompare.Attributes.Name) &&
                    element.GetType().Name.Equals(elementCompare.GetType().Name))
                    re.Add(fi);
            }
            return re;
        }
    }
}