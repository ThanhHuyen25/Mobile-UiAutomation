using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Drawing.Imaging;

using Microsoft.Test.Input;

namespace GUI_Testing_Automation
{
    public class ButtonElement : ElementBase
    {
        public ButtonElement(string id) : base(id) { }
        public ButtonElement() 
            : base()
        {
            //this.Name = name;
            //this.Children = new List<IElement>();
        }
        
    }
}
