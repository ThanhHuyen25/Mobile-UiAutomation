// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 6:13 PM 2018/1/21
using GUI_Testing_Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public abstract class AbstractScreen : IScreen
    {
        protected List<IScenario> scenarios;
        protected string name;
        protected ListUIElements allUIElements;

        public List<IScenario> Scenarios
        {
            get { return scenarios; }
            set { scenarios = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public ListUIElements AllUIElements
        {
            get { return allUIElements; }
            set { allUIElements = value; }
        }

        /// <summary>
        /// copy attributes from screen1 to screen2
        /// </summary>
        /// <param name="screen1"></param>
        /// <param name="screen2"></param>
        public static void CopyAttributes(AbstractScreen screen1, AbstractScreen screen2)
        {
            if (screen1.AllUIElements != null)
            {
                screen2.AllUIElements = screen1.AllUIElements.Clone();
            }
            if (screen1.Scenarios != null)
            {
                screen2.Scenarios = new List<IScenario>();
                foreach (var scenario in screen1.Scenarios)
                    screen2.Scenarios.Add(scenario.Clone());
            }
            screen2.Name = screen1.Name;
        }
    }

    /// <summary>
    /// contain indicator root elements (not contain children) or list of all elements
    /// </summary>
    public class ListUIElements
    {
        private ListElementsIndicator indicator;

        public ListElementsIndicator Indicator
        {
            get { return indicator; }
            set { indicator = value; }
        }

        public List<IElement> Elements
        {
            get { return elements; }
            set { elements = value; }
        }

        private List<IElement> elements;

        public ListUIElements(ListElementsIndicator indicator, List<IElement> elements)
        {
            this.indicator = indicator;
            this.elements = elements;
        }

        public ListUIElements Clone()
        {
            return new ListUIElements(this.indicator, elements == null ? null : new List<IElement>(elements));
        }
    }

    public enum ListElementsIndicator { OnlyRootElements, AllElements}
}
