using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Testing_Automation
{
    /**
     * represent a window, like MainWindowForm
     **/
    public class WindowElement:ElementBase
    {
        public WindowElement(string id) : base(id) { }
        public WindowElement() : base()
        {

        }
    }
}
