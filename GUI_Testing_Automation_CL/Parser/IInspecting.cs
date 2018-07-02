using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace GUI_Testing_Automation
{
    public interface IInspecting
    {        
        AutomationElementCollection Inspect(string pathToProgram);

        AutomationElementCollection Inspect(Process process);

        AutomationElementCollection Inspect(int processId);
    }
}
