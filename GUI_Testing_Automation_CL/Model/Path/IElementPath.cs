using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * represent xpath for element
 * like this: MainForm/Page1/Button1
 **/

namespace GUI_Testing_Automation
{
    public interface IElementPath
    {
        // append new string to path
        ElementPath AppendWith(string strAppend);
        string Path { get; set; }
    }
}
