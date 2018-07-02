// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:40 PM 2018/1/19
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace GUI_Testing_Automation
{
    public class Comparison
    {
        public static bool ElementEqual(AutomationElement element1, AutomationElement element2, bool checkProcessId = true)
        {
            try
            {
                if (element1 == null)
                    return element2 == null;
                if (element2 == null)
                    return element1 == null;
                return
                    Compare<string>(element1.Current.AcceleratorKey, element2.Current.AcceleratorKey) &&
                    Compare<string>(element1.Current.AccessKey, element2.Current.AccessKey) &&
                    Compare<string>(element1.Current.AutomationId, element2.Current.AutomationId) &&
                    Compare<string>(element1.Current.ClassName, element2.Current.ClassName) &&
                    //Compare<ControlType>(element1.Current.ControlType, element2.Current.ControlType) &&
                    Compare<string>(element1.Current.FrameworkId, element2.Current.FrameworkId) &&
                    Compare<string>(element1.Current.ItemType, element2.Current.ItemType) &&
                    Compare<string>(element1.Current.LocalizedControlType, element2.Current.LocalizedControlType) &&
                    Compare<string>(element1.Current.Name, element2.Current.Name) &&
                    (checkProcessId ? Compare<int>(element1.Current.ProcessId, element2.Current.ProcessId) : true) &&
                    Compare<bool>(element1.Current.HasKeyboardFocus, element2.Current.HasKeyboardFocus) &&
                    Compare<string>(element1.Current.HelpText, element2.Current.HelpText) &&
                    Compare<bool>(element1.Current.IsContentElement, element2.Current.IsContentElement) &&
                    Compare<bool>(element1.Current.IsControlElement, element2.Current.IsControlElement) &&
                    Compare<bool>(element1.Current.IsEnabled, element2.Current.IsEnabled) &&
                    Compare<bool>(element1.Current.IsKeyboardFocusable, element2.Current.IsKeyboardFocusable) &&
                    //Compare<bool>(element1.Current.IsOffscreen, element2.Current.IsOffscreen) &&
                    Compare<bool>(element1.Current.IsPassword, element2.Current.IsPassword) &&
                    Compare<bool>(element1.Current.IsRequiredForForm, element2.Current.IsRequiredForForm);
                //Compare<bool>(element1.Current., element2.Current.) 
            }
            catch (ElementNotAvailableException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool ElementEqual(IElement element, AutomationElement autoElement, bool checkProcessId = true)
        {
            try
            {
                if (element == null || element.Attributes == null)
                    return autoElement == null;
                if (autoElement == null)
                    return element == null || element.Attributes == null;
                return
                    Compare<string>(element.Attributes.AcceleratorKey, autoElement.Current.AcceleratorKey) &&
                    Compare<string>(element.Attributes.AccessKey, autoElement.Current.AccessKey) &&
                    Compare<string>(element.Attributes.DesignedId, autoElement.Current.AutomationId) &&
                    Compare<string>(element.Attributes.ClassName, autoElement.Current.ClassName) &&
                    Compare<string>(element.Attributes.FrameworkId, autoElement.Current.FrameworkId) &&
                    Compare<string>(element.Attributes.ItemType, autoElement.Current.ItemType) &&
                    Compare<string>(element.Attributes.LocalizedControlType, autoElement.Current.LocalizedControlType) &&
                    Compare<string>(element.Attributes.DesignedName, autoElement.Current.Name) &&
                    (checkProcessId ? Compare<int>(element.Attributes.ProcessId, autoElement.Current.ProcessId) : true) &&
                    //Compare<bool>(element.Attributes.HasKeyboardFocus, autoElement.Current.HasKeyboardFocus) &&
                    Compare<string>(element.Attributes.HelpText, autoElement.Current.HelpText);
                //Compare<bool>(element.Attributes.IsContentElement, autoElement.Current.IsContentElement) &&
                //Compare<bool>(element.Attributes.IsControlElement, autoElement.Current.IsControlElement) &&
                //Compare<bool>(element.Attributes.IsEnabled, autoElement.Current.IsEnabled) &&
                //Compare<bool>(element.Attributes.IsKeyboardFocusable, autoElement.Current.IsKeyboardFocusable) &&
                //Compare<bool>(element.Attributes.IsOffscreen, autoElement.Current.IsOffscreen) &&
                //Compare<bool>(element.Attributes.IsPassword, autoElement.Current.IsPassword) &&
                //Compare<bool>(element.Attributes.IsRequiredForForm, autoElement.Current.IsRequiredForForm);
            }
            catch (ElementNotAvailableException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CheckListElementContain(List<AutomationElement> listElements, AutomationElement element)
        {
            try
            {
                foreach (AutomationElement ele in listElements)
                {
                    if (ElementEqual(ele, element))
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CheckListElementContain(List<IElement> listElements, AutomationElement element)
        {
            try
            {
                foreach (IElement ele in listElements)
                {
                    if (ElementEqual(ele, element))
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ListElementsEqual(AutomationElementCollection list1, AutomationElementCollection list2)
        {
            try
            {
                if (list1 == null)
                    return list2 == null;
                if (list2 == null)
                    return list1 == null;
                if (list1.Count != list2.Count)
                    return false;
                foreach (AutomationElement ele1 in list1)
                {
                    if (!IsElementExisted(list2, ele1))
                        return false;
                }
                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        public static List<AutomationElement> FindAdditionElements(AutomationElementCollection _old, AutomationElementCollection _new)
        {
            if (_new == null)
                return null;
            if (_old == null)
            {
                AutomationElement[] temp = new AutomationElement[_new.Count];
                _new.CopyTo(temp, 0);
                return temp.ToList<AutomationElement>();
            }
            List<AutomationElement> re = new List<AutomationElement>();
            foreach (AutomationElement ele in _new)
            {
                if (!IsElementExisted(_old, ele))
                    re.Add(ele);
            }
            return re;
        }

        public static List<AutomationElement> FindAdditionElements(List<IElement> oldChildren, 
            AutomationElementCollection newChildren,
            Dictionary<AutomationElement, IElement> mappingElements)
        {
            if (oldChildren == null || oldChildren.Count == 0)
            {
                AutomationElement[] arr = new AutomationElement[newChildren.Count];
                newChildren.CopyTo(arr, 0);
                return arr.ToList();
            }
            List<AutomationElement> re = new List<AutomationElement>();
            foreach (IElement oldChild in oldChildren)
            {
                AutomationElement automationElement = null;
                foreach (KeyValuePair<AutomationElement, IElement> pair in mappingElements)
                {
                    if (pair.Value == null)
                        continue;
                    if (pair.Value.Equals(oldChild))
                        automationElement = pair.Key;
                }
                if (automationElement != null && !IsElementExisted(newChildren, automationElement))
                    re.Add(automationElement);
            }
            return re;
        }

        public static bool IsElementExisted(AutomationElementCollection collection, AutomationElement element)
        {
            if (collection == null)
                return false;
            foreach (AutomationElement child in collection)
            {
                if (ElementEqual(child, element))
                    return true;
            }
            return false;
        }

        public static bool Compare<T>(T v1, T v2)
        {
            if (v1 == null)
                return v2 == null;
            return v1.Equals(v2);
        }
    }
}
