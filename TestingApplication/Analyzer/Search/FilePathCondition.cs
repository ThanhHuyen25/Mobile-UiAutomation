using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class FilePathCondition : SearchCondition
    {
        private string path;

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public bool IgnoreCase
        {
            get { return ignoreCase; }
            set { ignoreCase = value; }
        }
        /// <summary>
        /// indicate compare path with case sensitive or not
        /// </summary>
        private bool ignoreCase = false;

        public override bool IsSatisfiable(IFile file)
        {
            if (ignoreCase)
                return file.Path.ToLower().Equals(path.ToLower());
            return file.Path.Equals(path);
        }
    }
}
