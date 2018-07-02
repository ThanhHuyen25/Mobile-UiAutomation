using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Testing_Automation
{
    public abstract class ContainerElementBase : ElementBase, IContainerElement
    {
        public ContainerElementBase(string id) : base(id) { }
        public ContainerElementBase() : base()
        {

        }
    }
}
