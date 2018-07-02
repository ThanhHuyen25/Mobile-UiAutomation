// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:15 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public abstract class AbstractScenario : IScenario
    {
        protected List<IUserAction> userActions;

        public List<IUserAction> UserActions
        {
            get { return userActions; }
            set { userActions = value; }
        }

        public abstract IScenario Clone();

        public static void CopyAttributes(AbstractScenario scenario1, AbstractScenario scenario2)
        {
            if (scenario1.UserActions != null)
            {
                scenario2.UserActions = new List<IUserAction>();
                foreach (var userAction in scenario1.UserActions)
                    scenario2.UserActions.Add(userAction.Clone());
            }
        }
    }
}
