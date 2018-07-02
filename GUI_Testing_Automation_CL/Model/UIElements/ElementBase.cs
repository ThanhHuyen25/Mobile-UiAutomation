// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:39 AM 2017/09/xx
using Microsoft.Test.Input;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;

namespace GUI_Testing_Automation
{
    public abstract class ElementBase : IElement
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string FOLDER_STORE_CAPTURE_IMAGE = @"..\..\Reports\ImagesCaptured";

        #region define const
        /// <summary>
        /// define all const item's type
        /// </summary>
        public const string WINDOW = "window";
        public const string BUTTON = "button";
        public const string COMBO_BOX = "combo box";
        public const string TEXT = "text";
        public const string PANE = "pane";
        public const string TITLE_BAR = "title bar";
        public const string MENU_BAR = "menu bar";
        public const string DOCUMENT = "document";
        public const string TAB = "tab";
        public const string TAB_ITEM = "tab item";
        public const string SCROLL_BAR = "scroll bar";
        public const string THUMB = "thumb";
        public const string TREE = "tree";
        public const string TREE_VIEW = "tree view";
        public const string TREE_ITEM = "tree item";
        public const string TABLE = "table";
        public const string HEADER = "header";
        public const string ITEM = "item";
        public const string LIST = "list";
        public const string LIST_ITEM = "list item";
        public const string EDIT = "edit";
        public const string CHECK_BOX = "check box";
        public const string RADIO_BUTTON = "radio button";
        public const string MENU_ITEM = "menu item";
        public const string CALENDAR = "calendar";
        public const string CUSTOM = "custom";
        public const string DATAGRID = "datagrid";
        public const string DATAGRID2 = "data grid";
        public const string DATAITEM = "dataitem";
        public const string GROUP = "group";
        public const string GROUP2 = "Group";
        public const string HEADER_ITEM = "header item";
        public const string HYPERLINK = "hyperlink";
        public const string IMAGE = "image";
        public const string MENU = "menu";
        public const string PROGRESS_BAR = "progress bar";
        public const string SEPARATOR = "separator";
        public const string SLIDER = "slider";
        public const string SPINNER = "spinner";
        public const string SPLIT_BUTTON = "split button";
        public const string STATUS_BAR = "status bar";
        public const string TOOL_BAR = "tool bar";
        public const string TOOL_TIP = "tool tip";
        public const string LIST_VIEW = "list view";
        public const string DIALOG = "dialog";
        public const string LINK = "link";

        /// <summary>
        /// define all element's type (ElementBase.ELementType)
        /// </summary>
        public const string WINDOW_TYPE = "Window";
        public const string BUTTON_TYPE = "Button";
        public const string COMBOBOX_TYPE = "ComboBox";
        public const string TEXT_TYPE = "Text";
        public const string CONTAINER_TYPE = "Container";
        public const string TITLEBAR_TYPE = "TitleBar";
        public const string MENUBAR_TYPE = "MenuBar";
        public const string TABPAGE_TYPE = "TabPage";
        public const string TABITEM_TYPE = "TabItem";
        public const string SCROLLBAR_TYPE = "ScrollBar";
        public const string TREEVIEW_TYPE = "TreeView";
        public const string TREEITEM_TYPE = "TreeItem";
        public const string TABLE_TYPE = "Table";
        public const string LIST_TYPE = "List";
        public const string LISTITEM_TYPE = "ListItem";
        public const string EDITABLETEXT_TYPE = "EditableText";
        public const string CHECKBOX_TYPE = "CheckBox";
        public const string RADIOBUTTON_TYPE = "RadioButton";
        public const string MENUITEM_TYPE = "MenuItem";
        public const string ROW_TYPE = "Row";
        public const string CELL_TYPE = "Cell";
        public const string UNKNOWN_TYPE = "Unknown";
        public const string HEADER_TYPE = "Header";
        public const string LIST_VIEW_TYPE = "ListView";
        public const string RADIO_BUTTON_TYPE = "RadioButton";
        public const string DIALOG_TYPE = "Window";
        public const string GROUP_TYPE = "Group";
        public const string LINK_TYPE = "Link";
        #endregion end define const

        #region constructor
        public ElementBase(string id) : this()
        {
            this.id = id;
        }
        public ElementBase()
        {
            Init();
        }
        #endregion end constructor

        /// <summary>
        /// all init function define here
        /// </summary>
        public void Init()
        {
            this.children = new List<IElement>();
            this.Attributes = new ElementAttributes();
        }

        #region utilities common functions
        public AutomationElement GetCurrentAutoElement()
        {
            return GetCurrentAutoElement(this);
        }

        public static AutomationElement GetCurrentAutoElement(IElement element)
        {
            try
            {
                //if (RuntimeInstance.mapIdElements == null)
                //{
                //    string msg = "Not load elements from .xml file before!";
                //    logger.Error(msg);
                //    throw new NullReferenceException(msg);
                //}
                //IElement otherEle = RuntimeInstance.mapIdElements[this.id];
                //CopyAttributes(otherEle);

                List<AutomationElement> listAutoElements = Search.SearchListAutomationElementsWithCheckPath(element);
                if (listAutoElements.Count == 0)
                {
                    logger.Error("Not Found automation element(s) matching with IElement: " + GetExpression(element));
                    logger.Info("Continue try to search with automationId = " + element.Attributes.DesignedId);
                    listAutoElements = Search.SearchListAutomationElementsOnlyMatchId(element);
                    if (listAutoElements.Count == 0)
                    {
                        logger.Error("Not Found automation element(s) matching with IElement has automationId = " + element.Attributes.DesignedId);
                        return null;
                    }
                }
                if (listAutoElements.Count > 1)
                    logger.Error("Found " + listAutoElements.Count + " automation element(s) matching with IElement: " + GetExpression(element));
                return listAutoElements[0];
            }
            catch (KeyNotFoundException e)
            {
                string msg = "Not know element with id = " + element.Id + ", maybe it wasnot not from .xml file";
                logger.Error(msg);
                return null;
            }
        }

        public string GetExpression()
        {
            return GetExpression(this);
        }

        public static string GetExpression(IElement element)
        {
            return element.Attributes.Name + "(type: " + element.GetType().Name +
                    "; designId: " + element.Attributes.DesignedId +
                    "; designName: " + element.Attributes.DesignedName;
        }

        public static string GetNameExp(IElement element)
        {
            string elementType = element.GetType().ToString().
                Replace("GUI_Testing_Automation.", "").
                Replace("Element", "");

            //Debug.WriteLine(elementType);

            return element.Attributes.DesignedName != "" ?
                (elementType + " " + GUI_Utils.NormalizeString(element.Attributes.DesignedName)) :
                (elementType + " " + GUI_Utils.NormalizeString(element.Attributes.DesignedId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> text of elements</returns>
        public string GetText()
        {
            AutomationElement automationElement = GetCurrentAutoElement();
            if (automationElement == null)
                return null;
            return GetText(automationElement);
        }

        public string GetText(AutomationElement automationElement)
        {
            object objPattern;
            TextPattern textPattern = null;
            if (true == automationElement.TryGetCurrentPattern(TextPattern.Pattern, out objPattern))
                textPattern = objPattern as TextPattern;
            if (textPattern == null)
            {
                // Target control doesn't support TextPattern.
                //logger.Info("Not support TextPattern");
                return automationElement.Current.Name;
            }
            return GetTextPattern(automationElement, textPattern);
        }

        public string GetTextPattern()
        {
            AutomationElement automationElement = GetCurrentAutoElement();
            if (automationElement == null)
                return null;
            //AutomationPattern[] patterns = automationElement.GetSupportedPatterns();
            //logger.Debug("Element " + this.Attributes.Name + "'s patterns: " + 
            //        string.Join<object>(",", patterns));
            object objPattern;
            TextPattern textPattern = null;
            if (true == automationElement.TryGetCurrentPattern(TextPattern.Pattern, out objPattern))
                textPattern = objPattern as TextPattern;
            if (textPattern == null)
            {
                // Target control doesn't support TextPattern.
                //logger.Info("Not support TextPattern");
                return null;
            }
            return GetTextPattern(automationElement, textPattern);
        }

        public static string GetTextPattern(AutomationElement targetTextElement, TextPattern textPattern)
        {
            return textPattern.DocumentRange.GetText(-1);
        }

        public double GetHeight()
        {
            AutomationElement autoElement = GetCurrentAutoElement();
            if (autoElement == null)
                return -1;
            Rect bound = autoElement.Current.BoundingRectangle;
            if (bound == null)
                return -1;
            return bound.Height;
        }

        public double GetWidth()
        {
            AutomationElement autoElement = GetCurrentAutoElement();
            if (autoElement == null)
                return -1;
            Rect bound = autoElement.Current.BoundingRectangle;
            if (bound == null)
                return -1;
            return bound.Width;
        }
        public Rect GetBoundingRect()
        {
            AutomationElement autoElement = GetCurrentAutoElement();
            if (autoElement == null)
                return Rect.Empty;
            Rect bound = autoElement.Current.BoundingRectangle;
            return bound;
        }

        public bool SetHeight(double height)
        {
            AutomationElement autoElement = GetCurrentAutoElement();
            if (autoElement == null)
                return false;
            TransformPattern transformPattern = null;
            object objPattern;
            if (true == autoElement.TryGetCurrentPattern(TransformPattern.Pattern, out objPattern))
                transformPattern = objPattern as TransformPattern;
            if (transformPattern != null)
            {
                Rect bound = autoElement.Current.BoundingRectangle;
                if (bound == null)
                    return false;
                transformPattern.Resize(bound.Width, height);
                return true;
            }
            // if not supported TransformPattern
            else
            {
            }
            return false;
        }

        public bool SetWidth(double width)
        {
            AutomationElement autoElement = GetCurrentAutoElement();
            if (autoElement == null)
                return false;
            TransformPattern transformPattern = null;
            object objPattern;
            if (true == autoElement.TryGetCurrentPattern(TransformPattern.Pattern, out objPattern))
                transformPattern = objPattern as TransformPattern;
            if (transformPattern != null)
            {
                Rect bound = autoElement.Current.BoundingRectangle;
                if (bound == null)
                    return false;
                transformPattern.Resize(width, bound.Height);
                return true;
            }
            // if not supported TransformPattern
            else
            {
            }
            return false;
        }

        public bool SetSize(double width, double height)
        {
            AutomationElement autoElement = GetCurrentAutoElement();
            if (autoElement == null)
                return false;
            TransformPattern transformPattern = null;
            object objPattern;
            if (true == autoElement.TryGetCurrentPattern(TransformPattern.Pattern, out objPattern))
                transformPattern = objPattern as TransformPattern;
            if (transformPattern != null)
            {
                Rect bound = autoElement.Current.BoundingRectangle;
                if (bound == null)
                    return false;
                transformPattern.Resize(width, height);
                return true;
            }
            // if not supported TransformPattern
            else
            {
            }
            return false;
        }

        public bool SetLocation(double x, double y)
        {
            AutomationElement autoElement = GetCurrentAutoElement();
            if (autoElement == null)
                return false;
            TransformPattern transformPattern = null;
            object objPattern;
            if (true == autoElement.TryGetCurrentPattern(TransformPattern.Pattern, out objPattern))
                transformPattern = objPattern as TransformPattern;
            if (transformPattern != null)
            {
                Rect bound = autoElement.Current.BoundingRectangle;
                if (bound == null)
                    return false;
                transformPattern.Move(x, y);
                return true;
            }
            // if not supported TransformPattern
            else
            {
            }
            return false;
        }

        public void ListSupportedPatterns()
        {
            AutomationElement automationElement = GetCurrentAutoElement();
            if (automationElement == null)
                return;
            object objPattern;
            WindowPattern windowPattern = null;
            if (true == automationElement.TryGetCurrentPattern(WindowPattern.Pattern, out objPattern))
                windowPattern = objPattern as WindowPattern;
            logger.Info(this.Attributes.Name + " support windowPattern: " + (windowPattern != null).ToString());

            TransformPattern transformPattern = null;
            if (true == automationElement.TryGetCurrentPattern(TransformPattern.Pattern, out objPattern))
                transformPattern = objPattern as TransformPattern;
            logger.Info(this.Attributes.Name + " support TransformPattern: " + (transformPattern != null).ToString());

            InvokePattern invokePattern = null;
            if (true == automationElement.TryGetCurrentPattern(InvokePattern.Pattern, out objPattern))
                invokePattern = objPattern as InvokePattern;
            logger.Info(this.Attributes.Name + " support InvokePattern: " + (invokePattern != null).ToString());
        }

        #endregion end utilities common functions

        #region common actions
        /// <summary>
        /// click element
        /// </summary>
        /// <returns></returns>
        public bool Click(ClickOptions option = ClickOptions.CenterElement)
        {
            AutomationElement element = GetCurrentAutoElement();
            return Click(element, option);
        }

        public static bool Click(AutomationElement autoElement, ClickOptions option = ClickOptions.CenterElement)
        {
            if (autoElement == null)
                return false;
            Rect bounds = autoElement.Current.BoundingRectangle;
            if (bounds == null)
                return false;
            double x = (bounds.Left + bounds.Right) / 2;
            if (option == ClickOptions.LeftElement)
                x = bounds.Left + 10;
            else if (option == ClickOptions.RightElement)
                x = bounds.Right - 10;
            System.Windows.Point p =
                new System.Windows.Point(x, (bounds.Top + bounds.Bottom) / 2);
            Mouse.MoveTo(new System.Drawing.Point((int)p.X, (int)p.Y));
            Mouse.Click(MouseButton.Left);
            return true;
        }

        public bool DoubleClick()
        {
            AutomationElement element = GetCurrentAutoElement();
            Rect bounds = element.Current.BoundingRectangle;
            System.Windows.Point p = new System.Windows.Point((bounds.Left + bounds.Right) / 2,
                       (bounds.Top + bounds.Bottom) / 2);
            Mouse.MoveTo(new System.Drawing.Point((int)p.X, (int)p.Y));
            Mouse.DoubleClick(MouseButton.Left);
            return true;
        }

        public bool RightClick()
        {
            AutomationElement element = GetCurrentAutoElement();
            Rect bounds = element.Current.BoundingRectangle;
            System.Windows.Point p = new System.Windows.Point((bounds.Left + bounds.Right) / 2,
                       (bounds.Top + bounds.Bottom) / 2);
            Mouse.MoveTo(new System.Drawing.Point((int)p.X, (int)p.Y));
            Mouse.Click(MouseButton.Right);
            return true;
        }

        public void Focus()
        {
            AutomationElement autoElement = GetCurrentAutoElement();
            //try
            //{
                autoElement.SetFocus();
            //} catch (Exception e)
            //{
                //logger.Error("Exception: " + e.Message + ", stacktrace: " + e.StackTrace);
            //}
        }

        public void Touch()
        {
        }
        /// <summary>
        /// store capture image to folder @path2FolderStore
        /// @fileName = @ElementType_@ElementName_@ddMMyy_HHmmss
        /// </summary>
        /// <param name="path2FolderStore"></param>
        /// <returns></returns>
        public bool Capture(string path2FolderStore)
        {
            string fileName = this.Attributes.ElementType + "_";
            fileName +=
                this.Attributes.DesignedName != "" ?
                 GUI_Utils.NormalizeString(this.Attributes.DesignedName) :
                 GUI_Utils.NormalizeString(this.Attributes.DesignedId);
            fileName += "_" + System.DateTime.Now.ToString("ddMMyy_HHmmss") + ".png";
            string filePath = path2FolderStore + @"\" + fileName;

            //modified by @duongtd - 09/11
            //hard code
            string encoded = CaptureElement.CaptureScreen(
                GetCurrentAutoElement().Current.BoundingRectangle, filePath);
            return encoded != null;
        }

        protected void CopyAttributes(IElement otherElement)
        {
            this.Attributes = otherElement.Attributes;
        }
        #endregion end common actions

        #region attributes
        protected string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        protected List<IElement> children;
        public List<IElement> Children
        {
            get { return children; }
            set { children = value; }
        }

        protected IElement parent;
        public IElement Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        protected ElementAttributes attributes;
        public ElementAttributes Attributes
        {
            get { return attributes; }
            set { this.attributes = value; }
        }

        protected string alias;
        public string Alias
        {
            get { return alias; }
            set { this.alias = value; }
        }
        #endregion end attributes
    }

    public enum ClickOptions
    {
        RightElement = 3,
        LeftElement = 2,
        CenterElement = 1,
    }
}