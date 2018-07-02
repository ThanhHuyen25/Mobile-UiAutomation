using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Testing_Automation
{
    public class ElementPath : IElementPath
    {
        string path;
        public ElementPath(string path)
        {
            this.path = path;
        }
        public const string SLASH = "/";

        public ElementPath AppendWith(string strAppend)
        {
            return new ElementPath(SLASH + strAppend);
        }

        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
            }
        }
    }
}
