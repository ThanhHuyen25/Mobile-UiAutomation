using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    /// <summary>
    /// all setting user control must implement this interface
    /// </summary>
    public interface IComponentState
    {
        bool IsChanged();
        bool Save();
        bool Discard();
    }
}
