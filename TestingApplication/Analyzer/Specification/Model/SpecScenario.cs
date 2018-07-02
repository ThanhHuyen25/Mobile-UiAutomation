// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:17 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class SpecScenario : AbstractScenario
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SpecScenario()
        {
            this.userActionsInString = new List<string>();
            this.colors = new List<Color>();
            this.preConditionsExpression = new List<string>();
        }
        private List<IScenario> preConditions;

        public List<IScenario> PreConditions
        {
            get { return preConditions; }
            set { preConditions = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private int id;

        private List<string> userActionsInString;
        public List<string> UserActionsInString
        {
            get { return userActionsInString; }
            set { userActionsInString = value; }
        }

        private List<Color> colors;
        public List<Color> Colors
        {
            get { return colors; }
            set { colors = value; }
        }
        private List<string> preConditionsExpression;
        public List<string> PreConditionsExpression
        {
            get { return preConditionsExpression; }
            set { preConditionsExpression = value; }
        }

        public override IScenario Clone()
        {
            SpecScenario re = new SpecScenario();
            CopyAttributes(this, re);
            logger.Info("IScenario thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId);

            if (this.colors != null)
            {
                re.Colors = new List<Color>();
                foreach (var color in this.colors)
                    re.Colors.Add(Color.Green); //FromArgb(255, color));
            }
            re.Id = id;
            if (this.preConditionsExpression != null)
                re.PreConditionsExpression = new List<string>(this.preConditionsExpression);
            if (this.userActionsInString != null)
                re.UserActionsInString = new List<string>(this.UserActionsInString);
            if (this.preConditions != null)
            {
                re.PreConditions = new List<IScenario>();
                foreach (var scenario in this.preConditions)
                {
                    re.PreConditions.Add(scenario.Clone());
                }
            }
            return re;
        }
    }
}
