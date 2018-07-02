// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:40 PM 2018/1/19
using GUI_Testing_Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace TestingApplication
{
    public class ElementDiscoverManual
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private INewElementAddedNotify newElementAddedNotify;

        public INewElementAddedNotify NewElementAddedNotify
        {
            get { return newElementAddedNotify; }
            set { newElementAddedNotify = value; }
        }

        public List<IElement> ListElementAdded
        {
            get { return listElementAdded; }
            set { listElementAdded = value; }
        }

        private List<AutomationElement> listAutoElementAdded = new List<AutomationElement>();

        private List<IElement> listElementAdded = new List<IElement>();

        public ElementDiscoverManual(INewElementAddedNotify newElementAdded)
        {
            this.newElementAddedNotify = newElementAdded;
            listAutoElementAdded = new List<AutomationElement>();
            listElementAdded = new List<IElement>();
        }

        private List<int[]> runtimeIdList = new List<int[]>();

        /// <summary>
        /// Handles structure-changed events. If a new app window has been added, this method ensures
        /// it's in the list of runtime IDs and subscribed to window-close events.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        /// An exception can be thrown by the UI Automation core if the element disappears
        /// before it can be processed -- for example, if a menu item is only briefly visible. 
        /// This exception cannot be caught here because it crosses native/managed boundaries. 
        /// In the debugger, you can ignore it and continue execution. The exception does not cause
        /// a break when the executable is being run.
        /// </remarks>
        private void OnStructureChanged(object sender, StructureChangedEventArgs e)
        {
            AutomationElement element = sender as AutomationElement;
            //element
            logger.Info("OnStructureChanged: " + e.StructureChangeType);

            if (e.StructureChangeType == StructureChangeType.ChildAdded)
            {
                try
                {
                    int[] rid = e.GetRuntimeId();
                    if (!runtimeIdList.Contains(rid) &&
                        !Comparison.CheckListElementContain(listAutoElementAdded, element) &&
                        !Comparison.CheckListElementContain(listElementAdded, element))
                    {
                        newElementAddedNotify.NewElementAddedCallback(element);
                    }
                    runtimeIdList.Add(rid);
                    listAutoElementAdded.Add(element);
                } catch (ElementNotAvailableException ex)
                {
                    logger.Error("Element not available exception. " + Utils.ExceptionToString(ex));
                }
            }
        }

        public void AddEvent(AutomationElement elementRoot)
        {
            //Task.Factory.StartNew(() =>
            Automation.AddStructureChangedEventHandler(elementRoot, TreeScope.Subtree,
                new StructureChangedEventHandler(OnStructureChanged));
        }

        private AutomationElement FindRuntimeId(int[] id, List<AutomationElement> listElement)
        {
            foreach (AutomationElement element in listAutoElementAdded)
            {
                var l = element.GetRuntimeId();
                if (l != null && l.SequenceEqual(id))
                {
                    return element;
                }
            }
            return null;
        }
    }

    public interface INewElementAddedNotify
    {
        void NewElementAddedCallback(AutomationElement element);
    }
}
