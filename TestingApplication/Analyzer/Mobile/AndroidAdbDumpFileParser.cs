// Copyright (c) 2018 fit.uet.vnu.edu.vn
// author @duongtd
// created on 10:09 AM 2018/7/3
using GUI_Testing_Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            }   
            return re;
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
            else if (node.Attributes["class"].Value.EndsWith("View"))
            {
                element.Attributes.Name = "View";
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
            else if (node.Attributes["class"].Value.EndsWith("Button"))
            {
                element.Attributes.Name = "Button";
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
                //element.Attributes.ClassName = node.Attributes["class"].Value;
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
                //element.Attributes.RectBounding = node.Attributes["content-desc"].Value;
            }
            else element.Attributes = null;

        }

    }
}
