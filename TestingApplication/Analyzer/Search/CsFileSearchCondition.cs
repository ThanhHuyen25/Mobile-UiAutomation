using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    class CsFileSearchCondition : SearchCondition
    {
        public override bool IsSatisfiable(IFile file)
        {
            return file is CsFile;
        }
    }
}
