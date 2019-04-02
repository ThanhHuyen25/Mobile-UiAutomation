// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 10:49 AM 2018/1/12
using GUI_Testing_Automation;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TestingApplication
{
    /// <summary>
    /// represent for displaying item in tree view
    /// </summary>
    public class ElementItemVisual
    {
        public ElementItemVisual(IElement element)
        {
            this.element = element;
            if (this.children == null)
                this.children = new ObservableCollection<ElementItemVisual>();
        }
        public ElementItemVisual(IElement element, ElementItemVisual parent) : this (element)
        {
            this.parent = parent;
        }
        private IElement element;
        public IElement Element
        {
            get
            {
                return element;
            }
            set
            {
                this.element = value;
            }
        }

        private bool isExpanded = false;
        private bool isSelected = false;
        private ObservableCollection<ElementItemVisual> children = null;

        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged(string propertyName)
        //{
            //if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}

        public ObservableCollection<ElementItemVisual> Children
        {
            get { return children; }
            set {
                this.children = value;
                //OnPropertyChanged("Children");
            }
        }

        private ElementItemVisual parent = null;

        public string ImageUrl
        {
            get
            {
                if (element == null)
                    return null;
                string icon = "unknown_icon";
                if (element is ButtonElement)
                    icon = "Button_16x";
                else if (element is EditableTextElement)
                    icon = "TextBox_16x";
                else if (element is TextElement)
                    icon = "Text_16x";
                else if (element is WindowElement || element is ContainerElementBase)
                    icon = "WindowsForm_16x";
                else if (element is CheckBoxElement)
                    icon = "CheckBox_16x";
                else if (element is RadioButtonElement)
                    icon = "RadioButton_16x";
                else if (element is MenuItemElement || element is MenuElement)
                    icon = "MenuItem_16x";
                else if (element is TabPageElement)
                    icon = "TabPage_16x";
                else if (element is TableElement)
                    icon = "Table_16x";
                else if (element is ImageElement)
                    icon = "Image_16x";
                else if (element is TreeViewElement || element is TreeItemElement)
                    icon = "TreeView_16x";
                else if (element is ComboBoxElement)
                    icon = "ComboBox_16x";
                else if (element is ListElement)
                    icon = "ListBox_16x";
                else if (element is ListItemElement)
                    icon = "Item_16x";
                else if (element is ScrollBarElement)
                    icon = "Scrollbar_24x";
                else if (element is TabItemElement)
                    icon = "Tab_24x";
                else if (element is MenuBarElement || element is TitleBarElement)
                    icon = "BarChart_24x";
                else if (element is DatagridElement)
                    icon = "DataGrid_24x";
                else if (element is HeaderElement)
                    icon = "HeaderFile_24x";
                else if (element is HeaderItemElement || element is CellElement)
                    icon = "Item_16x";
                else if (element is RowElement)
                    icon = "Row_24x";
                else if (element is GroupELement)
                    icon = "Group_16x";
                else if (element is LinkElement)    
                    icon = "Link_16x";
                else if (element is SeparatorElement)
                    icon = "Separator_16x";
                else if (element is ToolBarElement)
                    icon = "BarSeries_16x";
                else if (element.Attributes.Name.StartsWith("FrameLayout") || element.Attributes.Name.StartsWith("RelativeLayout") || element.Attributes.Name.StartsWith("LinearLayout"))
                    icon = "WindowsForm_16x";
                else if (element.Attributes.Name.StartsWith("TextView"))
                    icon = "Text_16x";
                else if (element.Attributes.Name.StartsWith("EditText"))
                    icon = "TextBox_16x";
                else if (element.Attributes.Name.StartsWith("Button"))
                    icon = "Button_16x";

                return "Resources/treeview_icons/" + icon.ToLower() + ".png";
            }
        }

        public string Name
        {
            get
            {
                return element.Attributes.Name;
            }
        }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set { isExpanded = value; }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
            }
        }

        public ElementItemVisual Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public bool IsExportToSpec
        {
            get { return isExportToSpec; }
            set
            {
                isExportToSpec = value;
            }
        }

        private bool isExportToSpec = false;

        public static ElementItemVisual FromElement(IElement element, ObservableCollection<ElementItemVisual> listRoots)
        {
            foreach (ElementItemVisual root in listRoots)
            {
                ElementItemVisual temp = FromElement(element, root);
                if (temp != null)
                    return temp;
            }
            return null;
        }

        public static ElementItemVisual FromElement(IElement element, ElementItemVisual elementItemVisual)
        {
            if (elementItemVisual.Element.Equals(element))
                return elementItemVisual;
            if (elementItemVisual.Children != null)
                foreach (ElementItemVisual child in elementItemVisual.Children)
                {
                    ElementItemVisual temp = FromElement(element, child);
                    if (temp != null)
                        return temp;
                }
            return null;
        }

        public static void SelectItemAndExpandParent(ElementItemVisual elementVisual)
        {
            ElementItemVisual temp = elementVisual.Parent;
            while (temp != null)
            {
                temp.IsExpanded = true;
                temp = temp.Parent;
            }
            elementVisual.isSelected = true;
        }

        public void Remove()
        {
            this.Parent.Children.Remove(this);
            this.Parent = null;
        }
    }
}
