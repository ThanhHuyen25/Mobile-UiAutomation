// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:40 PM 2018/1/19
using GUI_Testing_Automation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;

namespace TestingApplication
{
    public class ElementHoverHelper
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int processId;

        public int ProcessId
        {
            get { return processId; }
            set { processId = value; }
        }

        public static ElementItemVisual FindElement(Point point, int processId, ObservableCollection<ElementItemVisual> rootVisualElements)
        {
            try
            {
                AutomationElement element = AutomationElement.FromPoint(point);
                if (element.Current.ProcessId != processId)
                {
                    logger.Info("AutomationElement's process id: " + element.Current.ProcessId +
                        " not equal with desire process id: " + processId + " when finding element");
                    return null;
                }
                logger.Debug("Element: " + element.Current.Name + ", id: " + element.Current.AutomationId +
                ", type: " + element.Current.LocalizedControlType);
                foreach (ElementItemVisual rootVisualElement in rootVisualElements)
                {
                    ElementItemVisual temp = FindElement(element, rootVisualElement);
                    if (temp != null)
                        return temp;
                }
            }
            catch (Exception e)
            {
                logger.Error("----EXCEPTION: " + Utils.ExceptionToString(e));
            }
            return null;
        }
        public static ElementItemVisual FindElement(AutomationElement autoElement, ElementItemVisual visualElement)
        {
            if (Comparison.ElementEqual(visualElement.Element, autoElement, true))
                return visualElement;
            if (visualElement.Children != null)
                foreach (ElementItemVisual child in visualElement.Children)
                {
                    ElementItemVisual temp = FindElement(autoElement, child);
                    if (temp != null)
                        return temp;
                }
            return null;
        }
    }
}
