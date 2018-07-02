// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 5:20 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    /// <summary>
    /// store logs
    /// </summary>
    public class MyLog
    {
        private List<string> warnLogs = new List<string>();
        private List<string> errorLogs = new List<string>();

        public void Warn(string msg)
        {
            warnLogs.Add(msg);
        }

        public void Warn(string msg, log4net.ILog logger)
        {
            warnLogs.Add(msg);
            logger.Warn(msg);
        }

        public void Error(string msg)
        {
            errorLogs.Add(msg);
        }

        public List<string> GetWarnLogs() { return warnLogs; }
        public List<string> GetErrorLogs() { return errorLogs; }
    }
}
