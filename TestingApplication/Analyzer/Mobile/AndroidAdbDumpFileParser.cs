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
using System.Threading;
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

            // set attributes
            setProperties(re, node);

            // get element name
            getElementName(node, re);
            setTypeforRE(re);

            re.Parent = parent;
            setProperties(re, node);
            getElementName(node, re);

            if (node.HasChildNodes)
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    IElement children = Convert(node.ChildNodes[i], re);
                    re.Children.Add(children);
                }
                // reset type

            }
            // setTypeforRE(re);
            return re;
        }
        //
        // set re type
        //
        private void setTypeforRE(IElement re)
        {
            if (re.Attributes.Name.StartsWith("FrameLayout"))
            {
                re = new FrameLayout();
            }
            else if (re.Attributes.Name.StartsWith("Button"))
            {
                re = new ButtonAndroidElement();
            }
            else if (re.Attributes.Name.StartsWith("RelativeLayout"))
            {
                re = new LayoutAndroidElement();
            }
            else if (re.Attributes.Name.StartsWith("View"))
            {
                re = new ViewAndroidElement();
            }
            else if (re.Attributes.Name.StartsWith("TextView"))
            {
                re = new TextViewAndroidElement();
            }
            else if (re.Attributes.Name.StartsWith("AppWidgetHostView"))
            {
                re = new TextViewAndroidElement();
            }
            else if (re.Attributes.Name.StartsWith("ImageView"))
            {
                re = new ImageViewAndroidElement();
            }
            else if (re.Attributes.Name.StartsWith("ListView"))
            {
                re = new ListViewAndroidElement();
            }
            else if (re.Attributes.Name.StartsWith("LinearLayout"))
            {
                re = new ListViewAndroidElement();
            }
            else if (re.Attributes.Name.StartsWith("ScrollView"))
            {
                re = new ListViewAndroidElement();
            }
            else if (re.Attributes.Name.StartsWith("ViewPager"))
            {
                re = new ListViewAndroidElement();
            }
            else if (re.Attributes.Name.StartsWith("ViewSwitcher"))
            {
                re = new ListViewAndroidElement();
            }
            else if (re.Attributes.Name.StartsWith("Image"))
            {
                re = new ListViewAndroidElement();
            }
            else if (re.Attributes.Name.StartsWith("ImageButton"))
            {
                re = new ListViewAndroidElement();
            }
            else if (re.Attributes.Name.StartsWith("EditText"))
            {
                re = new ListViewAndroidElement();
            }
            else if (re.Attributes.Name.StartsWith("MultiAutoCompleteTextView"))
            {
                re = new ListViewAndroidElement();
            }
            else if (re.Attributes.Name.StartsWith("Spinner"))
            {
                re = new ListViewAndroidElement();
            }
            else if (re.Attributes.Name.StartsWith("QuickContactBadge"))
            {
                re = new ListViewAndroidElement();
            }
        }
        //
        // get element.name
        //
        static int i = 0;
        private void getElementName(XmlNode node, IElement element)
        {
            string tmp;
            tmp = element.Attributes.ContentDesc;
            if (tmp == "")
            {
                string str = element.Attributes.ResourceId;
                if (str == "")
                {
                    str = element.Attributes.Text;
                    if(str == "")
                    {
                        tmp = "layout" + i;
                        i++;
                    } 
                    else
                    {
                        tmp = str;
                    }
                }
                else
                {
                    string[] arr = str.Split('/');
                    tmp = arr[1];
                }
            }
            if (node.Attributes["class"].Value.EndsWith("LinearLayout"))
            {
                element.Attributes.Name = "LinearLayout " + tmp;
            }
            else if (node.Attributes["class"].Value.EndsWith("FrameLayout"))
            {
                element.Attributes.Name = "FrameLayout " + tmp;
            }
            else if (node.Attributes["class"].Value.EndsWith("RelativeLayout"))
            {
                element.Attributes.Name = "RelativeLayout " + tmp;
            }
            else if (node.Attributes["class"].Value.EndsWith("ListView"))
            {
                element.Attributes.Name = "ListView " + tmp;
            }
            else if (node.Attributes["class"].Value.EndsWith("ScrollView"))
            {
                element.Attributes.Name = "ScrollView " + tmp;
            }
            else if (node.Attributes["class"].Value.EndsWith("View"))
            {
                element.Attributes.Name = "View " + tmp;
            }
            else if (node.Attributes["class"].Value.EndsWith("ViewPager"))
            {
                element.Attributes.Name = "ViewPager " + tmp;
            }
            else if (node.Attributes["class"].Value.EndsWith("ViewSwitcher"))
            {
                element.Attributes.Name = "ViewSwitcher " + tmp;
            }
            else if (node.Attributes["class"].Value.EndsWith("TextView"))
            {
                element.Attributes.Name = "TextView " + tmp;
            }
            else if (node.Attributes["class"].Value.EndsWith("AppWidgetHostView"))
            {
                element.Attributes.Name = "AppWidgetHostView " + tmp;
            }
            else if (node.Attributes["class"].Value.EndsWith("ImageView"))
            {
                element.Attributes.Name = "ImageView " + tmp;
            }
            else if (node.Attributes["class"].Value.EndsWith("Image"))
            {
                element.Attributes.Name = "Image " + tmp;
            }
            else if (node.Attributes["class"].Value.EndsWith("ImageButton"))
            {
                element.Attributes.Name = "ImageButton " + tmp;
            }
            else if (node.Attributes["class"].Value.EndsWith("Button"))
            {
                element.Attributes.Name = "Button " + tmp;
            }
            else if (node.Attributes["class"].Value.EndsWith("EditText"))
            {
                element.Attributes.Name = "EditText " + tmp;
            } 
            else if (node.Attributes["class"].Value.EndsWith("MultiAutoCompleteTextView"))
            {
                element.Attributes.Name = "MultiAutoCompleteTextView " + tmp;
            }
            else if (node.Attributes["class"].Value.EndsWith("Spinner"))
            {
                element.Attributes.Name = "Spinner " + tmp;
            }
            else if (node.Attributes["class"].Value.EndsWith("QuickContactBadge"))
            {
                element.Attributes.Name = "QuickContactBadge " + tmp;
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
                element.Attributes.Checkable = bool.Parse(node.Attributes["checkable"].Value);
                element.Attributes.IsChecked = bool.Parse(node.Attributes["checked"].Value);
                element.Attributes.Clickable = bool.Parse(node.Attributes["clickable"].Value);
                element.Attributes.Enabled = bool.Parse(node.Attributes["enabled"].Value);
                element.Attributes.Focusable = bool.Parse(node.Attributes["focusable"].Value);
                element.Attributes.Focused = bool.Parse(node.Attributes["focused"].Value);
                element.Attributes.Scrollable = bool.Parse(node.Attributes["scrollable"].Value);
                element.Attributes.LongClickable = bool.Parse(node.Attributes["long-clickable"].Value);
                element.Attributes.Password = bool.Parse(node.Attributes["password"].Value);
                element.Attributes.Selected = bool.Parse(node.Attributes["selected"].Value);
                element.Attributes.ResourceId = node.Attributes["resource-id"].Value;
                element.Attributes.Xpath = getElementXpath(element);
                ///string temp = node.Attributes["bounds"].Value;
                element.Attributes.RectBounding = HandleNodeBound(node.Attributes["bounds"].Value);
                var rectBound = element.Attributes.RectBounding;
                Thread.Sleep(100);
                Bitmap source = new Bitmap(@"C:/ProgramData/screen.png");
                //Thread.Sleep(100);
                string strEncoded = CaptureAndroidElement.CaptureElement(source, rectBound);
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
            xpath = "/" + xpath;
            return xpath;
        }

        private Rect HandleNodeBound(string boundToString)
        {
            char[] array = boundToString.ToCharArray();
            // change string -> bound
            for (int i = 0; i < boundToString.Length; i++)
            {
                if (array[i] == ']' && array[i + 1] == '[')
                {
                    array[i] = ',';
                    array[i + 1] = ' ';
                }
                array[0] = ' ';
                array[boundToString.Length - 1] = ' ';
            }
            string bound = new string(array);
            Rect tmp = Rect.Parse(bound);
            Rect rect = new Rect();
            rect.Width = tmp.Width - tmp.X;
            rect.Height = tmp.Height - tmp.Y;
            rect.X = tmp.X;
            rect.Y = tmp.Y;
            return rect;
        }
    }
}
