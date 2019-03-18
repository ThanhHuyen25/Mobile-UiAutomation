using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Testing_Automation
{
    public class ButtonAndroidElement:ElementBase
    {
        public ButtonAndroidElement(string id) : base(id) { }
        public ButtonAndroidElement()
            : base()
        {
            //this.Name = name;
            //this.Children = new List<IElement>();
        }
    }
}
