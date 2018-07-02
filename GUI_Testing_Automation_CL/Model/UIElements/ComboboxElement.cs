using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace GUI_Testing_Automation
{
    public class ComboBoxElement : ElementBase
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ComboBoxElement(string id) : base(id) { }

        public ComboBoxElement() : base() {}
        public bool ChooseOption(int order)
        {
            IElement buttonDropDown = GetDropDownButton(this);
            bool re = false;
            if (buttonDropDown == null)
                re = this.Click(ClickOptions.RightElement);
            else
                re = buttonDropDown.Click();
            //Thread.Sleep(1000);
            var listOptions = GetListOptions();
            if (listOptions == null)
            {
                GUI_Utils.AddNewActionReport(new ActionReport(
                    "ChooseOption " + order + " of " + this.Attributes.Name + " fail",
                    ActionReport.STATUS_FAILTURE,
                    ActionReport.CATEGORY_PROCEDURE,
                    Validate.CaptureScreen()));
                return false;
            }
            re = re & ElementBase.Click(listOptions[order - 1]);
            return re;
        }

        /// <summary>
        /// get drop down button, which if was clicked, all item will be expand
        /// </summary>
        /// <param name="comboboxElement"></param>
        /// <returns></returns>
        public IElement GetDropDownButton(IElement comboboxElement)
        {
            List<IElement> listButton = Search.SearchIElement(
                comboboxElement, new TypeCondition(ElementBase.BUTTON_TYPE));
            if (listButton == null || listButton.Count == 0)
                return null;
            if (listButton.Count != 1)
                logger.Error("Got " + listButton.Count + " button drop down in a ComboBox element (expected 1)");
            return listButton[0];
        } 

        public List<IElement> GetListOptions(IElement comboboxElement)
        {
            List<IElement> listOptions = Search.SearchIElement(
                comboboxElement, new TypeCondition(ElementBase.LISTITEM_TYPE));
            if (listOptions.Count < 1)
                logger.Error("Got " + listOptions.Count + " list element(s) in a ComboBox element (expected 1 or more)");
            return listOptions;
        }

        public AutomationElementCollection GetListOptions()
        {
            AutomationElement comboboxElement = GetCurrentAutoElement();
            if (comboboxElement == null)
                return null;
            AutomationElementCollection children = comboboxElement.FindAll(TreeScope.Children, Condition.TrueCondition);
            return children;
        }
    }
}
