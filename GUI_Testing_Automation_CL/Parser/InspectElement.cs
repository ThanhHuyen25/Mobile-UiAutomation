using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace GUI_Testing_Automation
{
    public class InspectElement : IInspecting
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AutomationElementCollection Inspect(string pathToProgram)
        {
            return GUI_Utils.OpenApp1(pathToProgram, TreeScope.Children, true);
        }

        public AutomationElementCollection Inspect(Process process)
        {
            return GUI_Utils.DoOpenApp(process, TreeScope.Children, true).Item1;
        }

        public AutomationElementCollection Inspect(int processId)
        {
            return GUI_Utils.DoOpenApp(processId, TreeScope.Children, true).Item1;
        }

        public void TraverseElement(AutomationElement window, int level)
        {
            //print info
            //LogDebug(window, level);

            if (window.Current.LocalizedControlType == ElementBase.TREE_ITEM)
            {
                Object tree;
                window.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out tree);
                if (tree != null)
                {
                    ExpandCollapsePattern treeConvert = (ExpandCollapsePattern)tree;
                    if (treeConvert.Current.ExpandCollapseState == ExpandCollapseState.Collapsed)
                    {
                        treeConvert.Expand();
                    }
                }
            }
            else if (window.Current.LocalizedControlType == ElementBase.TAB_ITEM)
            {
                Object tabSelection;
                window.TryGetCurrentPattern(SelectionItemPattern.Pattern, out tabSelection);
                if (tabSelection != null)
                {
                    ((SelectionItemPattern)tabSelection).Select();
                }
            }

            //found item element in window
            AutomationElementCollection itemCollection = window.FindAll(TreeScope.Children, Condition.TrueCondition);

            //resurcive transfer all children
            foreach (AutomationElement item in itemCollection)
            {
                TraverseElement(item, level + 1);
            }
        }

        private string GetTab(int level)
        {
            string tab = "";
            for (int i = 0; i < level; i++)
            {
                tab += "\t";
            }
            return tab;
        }

        private void LogDebug(AutomationElement element, int level)
        {
            Debug.WriteLine(GetTab(level) + " " + element.Current.AutomationId + "-" + element.Current.Name);
        }
    }
}