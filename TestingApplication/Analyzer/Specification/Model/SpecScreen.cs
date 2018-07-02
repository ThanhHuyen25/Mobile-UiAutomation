// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 6:19 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class SpecScreen : AbstractScreen
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected Dictionary<string, string> mappingAliasWithNode;

        public Dictionary<string, string> MappingAliasWithNode
        {
            get { return mappingAliasWithNode; }
            set { mappingAliasWithNode = value; }
        }

        public List<SpecNode> ListSpecNodes
        {
            get { return listSpecNodes; }
            set { listSpecNodes = value; }
        }

        public List<Tuple<int, int>> ListValidationUserCode
        {
            get { return listValidationUserCode; }
            set { listValidationUserCode = value; }
        }

        protected List<SpecNode> listSpecNodes;
        protected List<Tuple<int, int>> listValidationUserCode;

        public SpecScreen Clone()
        {
            logger.Info("SpecScreen thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId);

            SpecScreen re = new SpecScreen();
            if (mappingAliasWithNode != null)
                re.MappingAliasWithNode = new Dictionary<string, string>(mappingAliasWithNode);
            if (listSpecNodes != null)
            {
                re.ListSpecNodes = new List<SpecNode>();
                foreach (var specNode in listSpecNodes)
                    re.ListSpecNodes.Add(specNode.Clone() as SpecNode);
            }
            if (listValidationUserCode != null)
            {
                re.ListValidationUserCode = new List<Tuple<int, int>>(listValidationUserCode);
            }
            CopyAttributes(this, re);
            return re;
        }
    }
}
