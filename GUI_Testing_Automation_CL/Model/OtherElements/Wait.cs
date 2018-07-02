// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 10:49 AM 2018/1/12
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GUI_Testing_Automation
{
    public class Wait
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const int ITME_INTERVAL = 50;
        public static void WaitUtilExist(IElement element)
        {
            int count = 0;
            while (!Validate.Exists(element) && count < 20)
            {
                Thread.Sleep(ITME_INTERVAL);
                count += 1;
            }
            //check Exist with match autoamtionId only
            while (!Validate.ExistsWithIdOnly(element) && count < 200)
            {
                Thread.Sleep(ITME_INTERVAL);
                count += 1;
            }
            if (!Validate.ExistsWithIdOnly(element))
                logger.Error("Time out, quit wait");
        }
    }
}
