// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 3:16 PM 2018/6/6
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class ScriptGenerationParams : IScriptGenerationParams
    {
        public ScriptGenerationParams() { }
        private SpecNode specNode;
        private ListUIElements listUIElements;
        private Color color;
        private MyLog myLog;
        private string screenName;
        private int id;
        private string instanceName = CSharpScriptsGeneration.INSTANCE_NAME;
        private string pathToApp;

        public SpecNode SpecNode
        {
            get { return specNode; }
            set { specNode = value; }
        }
        public ListUIElements ListUIElements
        {
            get { return listUIElements; }
            set { listUIElements = value; }
        }
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        public MyLog MyLog
        {
            get { return myLog; }
            set { myLog = value; }
        }
        public string ScreenName
        {
            get { return screenName; }
            set { screenName = value; }
        }

        //scenario id ??
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string InstanceName
        {
            get { return instanceName; }
            set { instanceName = value; }
        }

        public string PathToApp
        {
            get { return pathToApp; }
            set { pathToApp = value; }
        }

        public virtual IScriptGenerationParams Clone()
        {
            IScriptGenerationParams re;
            if (this is RanorexValidationUCScriptGenerationParams)
            {
                re = new RanorexValidationUCScriptGenerationParams();
                ((RanorexValidationUCScriptGenerationParams)re).CopyAttributesFrom(this as RanorexValidationUCScriptGenerationParams);
                
            }
            else if (this is RanorexWaitValidateScriptGenerationParams)
            {
                re = new RanorexWaitValidateScriptGenerationParams();
                ((RanorexWaitValidateScriptGenerationParams)re).CopyAttributesFrom(this as RanorexWaitValidateScriptGenerationParams);
            }
            else if (this is RanorexUCScriptGenerationParams)
            {
                re = new RanorexUCScriptGenerationParams();
                ((RanorexUCScriptGenerationParams)re).CopyAttributesFrom(this as RanorexUCScriptGenerationParams);
            }
            else if (this is RanorexScriptGenerationParams)
            {
                re = new RanorexScriptGenerationParams();
                ((RanorexUCScriptGenerationParams)re).CopyAttributesFrom(this as RanorexUCScriptGenerationParams);
            }
            else if (this is ValidationUCScriptGenerationParams)
            {
                re = new ValidationUCScriptGenerationParams();
                ((ValidationUCScriptGenerationParams)re).CopyAttributesFrom(this as ValidationUCScriptGenerationParams);
            }
            else if (this is WaitValidateScriptGenerationParams)
            {
                re = new WaitValidateScriptGenerationParams();
                ((WaitValidateScriptGenerationParams)re).CopyAttributesFrom(this as WaitValidateScriptGenerationParams);
            }
            else if (this is UserCodeScriptGenerationParams)
            {
                re = new UserCodeScriptGenerationParams();
                ((UserCodeScriptGenerationParams)re).CopyAttributesFrom(this as UserCodeScriptGenerationParams);
            }
            else
            {
                re = new ScriptGenerationParams();
                ((ScriptGenerationParams)re).CopyAttributesFrom(this as ScriptGenerationParams);
            }
            CopyAttributes(re, this);
            return re;
        }

        public static void CopyAttributes(IScriptGenerationParams target, IScriptGenerationParams origin)
        {
            target.Color = origin.Color;
            target.Id = origin.Id;
            target.InstanceName = origin.InstanceName;
            if (origin.ListUIElements != null)
                target.ListUIElements = origin.ListUIElements.Clone();
            target.MyLog = origin.MyLog; // TODO: consider to clone here
            target.PathToApp = origin.PathToApp;
            target.ScreenName = origin.ScreenName;
            if (origin.SpecNode != null)
                target.SpecNode = origin.SpecNode.Clone() as SpecNode;
        }

        public void CopyAttributesFrom(ScriptGenerationParams source)
        {
            CopyAttributes(this, source);
        }
    }
}
