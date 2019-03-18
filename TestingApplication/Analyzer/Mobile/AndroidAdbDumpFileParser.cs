// Copyright (c) 2018 fit.uet.vnu.edu.vn
// author @duongtd
// created on 10:09 AM 2018/7/3
using GUI_Testing_Automation;
using GUI_Testing_Automation.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Xml;

namespace TestingApplication
{
    public class AndroidAdbDumpFileParser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>only root elements</returns>
        public List<IElement> Parse(string filePath)
        {
            // implement here
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            IElement root = Convert(doc.DocumentElement.ChildNodes[0], null);
            return new List<IElement> { root };
        }

        private IElement Convert(XmlNode node, IElement parent)
        {
            // handle type of element
            IElement re = new FrameLayout();
            re.Parent = parent;
            
            // get element name
            getElementName(node, re);

            // set attributes
            setProperties(re, node);

            if (node.HasChildNodes)
            {
                for (int i= 0; i < node.ChildNodes.Count; i++)
                {
                    IElement children = Convert(node.ChildNodes[i], re);
                    re.Children.Add(children);
                }
                // reset type
                
            }
            setTypeforRE(re);
            return re;
        }
        //
        // set re type
        //
        private void setTypeforRE(IElement re)
        {
            if (re.Attributes.Name == "FrameLayout") {
                re = new FrameLayout();
            }
            else if (re.Attributes.Name == "Button"){
                re = new ButtonAndroidElement();
            }
            else if (re.Attributes.Name == "RelativeLayout"){
                re = new LayoutAndroidElement();
            }
            else if (re.Attributes.Name == "View"){
                re = new ViewAndroidElement();
            }
            else if (re.Attributes.Name == "TextView") {
                re = new TextViewAndroidElement();
            }
            else if (re.Attributes.Name == "AppWidgetHostView") { 
           
            }
            else if (re.Attributes.Name == "ImageView") {
                re = new ImageViewAndroidElement();
            }
            else if (re.Attributes.Name == "ListView")
            {
                re = new ListViewAndroidElement();
            }
        }
        //
        // get element.name
        //
        private void getElementName(XmlNode node, IElement element)
        {
            if (node.Attributes["class"].Value.EndsWith("LinearLayout"))
            {
                element.Attributes.Name = "LinearLayout";
            }
            else if (node.Attributes["class"].Value.EndsWith("FrameLayout"))
            {
                element.Attributes.Name = "FrameLayout";
            }
            else if (node.Attributes["class"].Value.EndsWith("RelativeLayout"))
            {
                element.Attributes.Name = "RelativeLayout";
            }
            else if (node.Attributes["class"].Value.EndsWith("ListView"))
            {
                element.Attributes.Name = "ListView";
            }
            else if (node.Attributes["class"].Value.EndsWith("ScrollView"))
            {
                element.Attributes.Name = "ScrollView";
            }
            else if (node.Attributes["class"].Value.EndsWith("View"))
            {
                element.Attributes.Name = "View";
            }
            else if (node.Attributes["class"].Value.EndsWith("ViewPager"))
            {
                element.Attributes.Name = "ViewPager";
            }
            else if (node.Attributes["class"].Value.EndsWith("ViewSwitcher"))
            {
                element.Attributes.Name = "ViewSwitcher";
            }
            else if (node.Attributes["class"].Value.EndsWith("TextView"))
            {
                element.Attributes.Name = "TextView";
            }
            else if (node.Attributes["class"].Value.EndsWith("AppWidgetHostView"))
            {
                element.Attributes.Name = "AppWidgetHostView";
            }
            else if (node.Attributes["class"].Value.EndsWith("ImageView"))
            {
                element.Attributes.Name = "ImageView";
            }
            else if (node.Attributes["class"].Value.EndsWith("Image"))
            {
                element.Attributes.Name = "Image";
            }
            else if (node.Attributes["class"].Value.EndsWith("ImageButton"))
            {
                element.Attributes.Name = "ImageButton";
            }
            else if (node.Attributes["class"].Value.EndsWith("Button"))
            {
                element.Attributes.Name = "Button";
            }
            else if (node.Attributes["class"].Value.EndsWith("EditText"))
            {
                element.Attributes.Name = "EditText";
            }
        }

        //**
        // set properties
        //
        private void setProperties(IElement element, XmlNode node)
        {
            if (node.Attributes != null)
            {
                element.Attributes.Index = Int32.Parse(node.Attributes["index"].Value);
                element.Attributes.Text = node.Attributes["text"].Value;
                element.Attributes.ClassName = node.Attributes["class"].Value;
                element.Attributes.Package = node.Attributes["package"].Value;
                element.Attributes.ContentDesc = node.Attributes["content-desc"].Value;
                element.Attributes.Checkable = node.Attributes["checkable"].Value.Equals(true);
                element.Attributes.IsChecked = node.Attributes["checked"].Value.Equals(true);
                element.Attributes.Clickable = node.Attributes["clickable"].Value.Equals(true);
                element.Attributes.Enabled = node.Attributes["enabled"].Value.Equals(true);
                element.Attributes.Focusable = node.Attributes["focusable"].Value.Equals(true);
                element.Attributes.Focused = node.Attributes["focused"].Value.Equals(true);
                element.Attributes.Scrollable = node.Attributes["scrollable"].Value.Equals(true);
                element.Attributes.LongClickable = node.Attributes["long-clickable"].Value.Equals(true);
                element.Attributes.Password = node.Attributes["password"].Value.Equals(true);
                element.Attributes.Selected = node.Attributes["selected"].Value.Equals(true);
                element.Attributes.ResourceId = node.Attributes["resource-id"].Value;
                element.Attributes.Xpath = getElementXpath(element);
                ///string temp = node.Attributes["bounds"].Value;
                element.Attributes.RectBounding = HandleNodeBound(node.Attributes["bounds"].Value);
                var rectBound = element.Attributes.RectBounding;
                Bitmap source = new Bitmap(@"C:/ProgramData/screen.png");
                string strEncoded = CaptureAndroidElement.CaptureElement(source,rectBound);
                element.Attributes.ImageCaptureEncoded = strEncoded;
            }
            else element.Attributes = null;
        }

        //
        // get xpath
        //
        private String getElementXpath(IElement element)
        {
            string xpath;
            IElement temp;
            xpath = "/" + element.Attributes.ClassName + "[@index='" + element.Attributes.Index.ToString() + "']";
            temp = element.Parent;
            while (temp != null)
            {
                xpath = "/" + temp.Attributes.ClassName + "[@index='" + temp.Attributes.Index.ToString() + "']" + xpath;
                temp = temp.Parent;
            }
            xpath = "/"+ xpath;
            return xpath;
        }

        private Rect HandleNodeBound(string boundToString)
        {
            char[] array = boundToString.ToCharArray();
            for (int i=0; i<boundToString.Length; i++)
            {
                if (array[i] == ']' && array[i+1] == '[')
                {
                    array[i] = ',';
                    array[i + 1] = ' ';
                }
                array[0] = ' ';
                array[boundToString.Length - 1] = ' ';
            }
            string bound = new string(array);
            Rect rect = Rect.Parse(bound);
            return rect;
        }
    }
}
