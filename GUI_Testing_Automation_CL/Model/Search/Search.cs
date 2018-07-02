// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:18 PM 2018/1/10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace GUI_Testing_Automation
{
    public class Search
    {
        public static List<IElement> SearchIElement(IElement rootElement, AbstractSearchCondition condition)
        {
            List<IElement> re = new List<IElement>();
            DoSearch(rootElement, condition, ref re);
            return re;
        }

        private static void DoSearch(IElement element, AbstractSearchCondition condition, ref List<IElement> re)
        {
            if (element == null)
                return;
            if (condition.IsSatisfiable(element))
                re.Add(element);
            if (element.Children != null && element.Children.Count > 0)
                foreach (IElement child in element.Children)
                    DoSearch(child, condition, ref re);
        }

        public static List<AutomationElement> SearchListAutomationElementsWithCheckPath(IElement element)
        {
            return SearchListAutomationElementsWithCheckPath(element, true);
        }

        /// <summary>
        /// search list automation elements with check matching parent
        /// </summary>
        /// <param name="element"></param>
        /// <param name="updateWhenNull"> determine update element when search return null</param>
        /// <returns></returns>
        public static List<AutomationElement> SearchListAutomationElementsWithCheckPath(IElement element, bool updateWhenNull)
        {
            List<AutomationElement> listAutoElements = SearchListAutomationElements(element, updateWhenNull);
            List<AutomationElement> listAutoElementsFiltered = new List<AutomationElement>();
            foreach(AutomationElement autoElement in listAutoElements)
            {
                bool check = CheckElementsMatchPath(element, autoElement);
                if (check)
                    listAutoElementsFiltered.Add(autoElement);
            }
            return listAutoElementsFiltered;
        }

        /// <summary>
        /// check if IElement match path with AutomationElement
        /// </summary>
        /// <param name="element"></param>
        /// <param name="autoElement"></param>
        /// <returns></returns>
        public static bool CheckElementsMatchPath(IElement element, AutomationElement autoElement)
        {
            // implement here
            if (!element.Attributes.DesignedId.Equals(autoElement.Current.AutomationId) || 
                !element.Attributes.DesignedName.Equals(autoElement.Current.Name))
                return false;
            if (element.Parent == null)
                return true;
            TreeWalker treeWalker = TreeWalker.ControlViewWalker;
            AutomationElement autoParentElement = treeWalker.GetParent(autoElement);
            return CheckElementsMatchPath(element.Parent, autoParentElement);
        }

        public static List<AutomationElement> SearchListAutomationElements(IElement element)
        {
            return SearchListAutomationElements(element, true);
        }
        /// <summary>
        /// search list automation element without check parent match
        /// </summary>
        /// <param name="element"></param>
        /// <param name="updateWhenNull"> determine update element when search return null</param>
        /// <returns></returns>
        public static List<AutomationElement> SearchListAutomationElements(IElement element, bool updateWhenNull)
        {
            List<AutomationElement> listAutoRootElement = RuntimeInstance.listRootAutoElement;
            if (listAutoRootElement == null || listAutoRootElement.Count == 0)
                listAutoRootElement = new List<AutomationElement>() { AutomationElement.RootElement };
            return SearchListAutomationElements(element, listAutoRootElement, updateWhenNull);
        }

        public static List<AutomationElement> SearchListAutomationElements(
            IElement element, List<AutomationElement> listAutoRootElement, bool updateWhenNull)
        {
            List<AutomationElement> re = new List<AutomationElement>();
            foreach (AutomationElement autoRootElement in listAutoRootElement)
            {
                re.AddRange(SearchListAutomationElements(element, autoRootElement));
            }

            //when null, update (maybe new windows)
            if (re.Count == 0)
            {
                List<AutomationElement> newWindows = GUI_Utils.UpdateElement();
                //only search with new windows
                foreach (AutomationElement newWindow in newWindows)
                {
                    re.AddRange(SearchListAutomationElements(element, newWindow));
                }
            }
            return re;
        }

        public static List<AutomationElement> SearchListAutomationElements(IElement element, AutomationElement rootAutoElement)
        {
            IdAndNameDesignCondition idAndNameCondition = new IdAndNameDesignCondition(element.Attributes.DesignedId, element.Attributes.DesignedName);
            Condition idCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, idAndNameCondition.DesignId);
            Condition nameCondition = new PropertyCondition(AutomationElement.NameProperty, idAndNameCondition.DesignName);
            AndCondition condition = new AndCondition(idCondition, nameCondition);
            AutomationElementCollection listElements = rootAutoElement.FindAll(TreeScope.Subtree, condition);
            AutomationElement[] re = new AutomationElement[listElements.Count];
            listElements.CopyTo(re, 0);
            return re.ToList();
        }

        public static List<AutomationElement> SearchListAutomationElementsOnlyMatchId(IElement element, AutomationElement rootAutoElement)
        {
            Condition idCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, element.Attributes.DesignedId);
            AutomationElementCollection listElements = rootAutoElement.FindAll(TreeScope.Subtree, idCondition);
            AutomationElement[] re = new AutomationElement[listElements.Count];
            listElements.CopyTo(re, 0);
            return re.ToList();
        }

        public static List<AutomationElement> SearchListAutomationElementsOnlyMatchId(IElement element)
        {
            List<AutomationElement> listAutoRootElement = RuntimeInstance.listRootAutoElement;
            if (listAutoRootElement == null || listAutoRootElement.Count == 0)
                listAutoRootElement = new List<AutomationElement>() { AutomationElement.RootElement };
            List<AutomationElement> re = new List<AutomationElement>();
            foreach (AutomationElement autoRootElement in listAutoRootElement)
            {
                re.AddRange(SearchListAutomationElementsOnlyMatchId(element, autoRootElement));
            }
            return re;
        }
    }
}
