using System;
using System.Collections.Generic;
using System.Windows.Automation;
using System.Threading;
using WindowsInput.Native;
using System.Windows.Forms;

namespace GUI_Testing_Automation
{
    public class EditableTextElement : ElementBase
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public EditableTextElement(string id) : base(id) { }

        public EditableTextElement() : base()
        {
        }

        /// <summary>
        /// focus, then input normal string into this text element
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool InputString(string input)
        {
            AutomationElement element = GetCurrentAutoElement();
            if (element == null)
            {
                GUI_Utils.AddNewActionReport(new ActionReport(
                    "Input string '" + input + "' into " + this.Attributes.Name + " fail",
                    ActionReport.STATUS_FAILTURE,
                    ActionReport.CATEGORY_PROCEDURE,
                    Validate.CaptureScreen()));
                return false;
            }
            //element.SetFocus();
            //ValuePattern valuePatternA = element.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
            //valuePatternA.SetValue(input);

            object valuePattern = null;

            // Control does not support the ValuePattern pattern 
            // so use keyboard input to insert content.
            //
            // NOTE: Elements that support TextPattern 
            //       do not support ValuePattern and TextPattern
            //       does not support setting the text of 
            //       multi-line edit or document controls.
            //       For this reason, text input must be simulated
            //       using one of the following methods.
            //       
            if (!element.TryGetCurrentPattern(
                ValuePattern.Pattern, out valuePattern))
            {
                logger.Info("The control with an AutomationID of " +
                    element.Current.AutomationId.ToString() +
                    " does not support ValuePattern." +
                    " Using keyboard input.");

                // Set focus for input functionality and begin.
                element.SetFocus();

                // Pause before sending keyboard input.
                //Thread.Sleep(100);

                // Delete existing content in the control and insert new content.
                //SendKeys.SendWait("^{HOME}");   // Move to start of control
                //SendKeys.SendWait("^+{END}");   // Select everything
                //SendKeys.SendWait("{DEL}");     // Delete selection
                SendKeys.SendWait(input);
                Thread.Sleep(100);
                Keyboard.SendCombinedKeys(Keyboard.K_CTRL, Keyboard.K_END);
            }
            // Control supports the ValuePattern pattern so we can 
            // use the SetValue method to insert content.
            else
            {
                logger.Info("The control with an AutomationID of " +
                    element.Current.AutomationId.ToString() +
                    " supports ValuePattern." +
                    " Using ValuePattern.SetValue().");

                // Set focus for input functionality and begin.
                element.SetFocus();
                string old = "";
                try
                {
                    old = ((ValuePattern)valuePattern).Current.Value;
                }
                catch (Exception) { }
                ((ValuePattern)valuePattern).SetValue(old + input);
                Keyboard.SendCombinedKeys(Keyboard.K_CTRL, Keyboard.K_END);
            }

            return true;
        }

        public bool ValidateAttribute(string nameAttribute, string valueAttribute)
        {
            AutomationElement autoElement = GetCurrentAutoElement();
            switch(nameAttribute)
            {
                case TEXT:
                    string realValue = EditableTextElement.GetTextElement(autoElement);
                    return realValue == valueAttribute;
                default:
                    logger.Error("Not handled validate " + nameAttribute);
                    return false;
            }
        }
        public static string GetTextElement(AutomationElement element)
        {
            object patternObj;
            if (element.TryGetCurrentPattern(ValuePattern.Pattern, out patternObj))
            {
                var valuePattern = (ValuePattern)patternObj;
                return valuePattern.Current.Value;
            }
            else if (element.TryGetCurrentPattern(TextPattern.Pattern, out patternObj))
            {
                var textPattern = (TextPattern)patternObj;
                return textPattern.DocumentRange.GetText(-1).TrimEnd('\r'); // often there is an extra '\r' hanging off the end.
            }
            else
            {
                return element.Current.Name;
            }
        }
    }
}
