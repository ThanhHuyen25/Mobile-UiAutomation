// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 5:38 PM 2017/11/22
using System;
using System.Collections.Generic;

namespace TestingApplication
{
    public class ProjectModifier
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// modify list of .cs files
        /// </summary>
        /// <param name="listCsFiles"></param>
        /// <param name="listScreens"></param>
        /// <returns></returns>
        public bool ModifyListCsFile(List<CsFile> listCsFiles, List<CsClass> listScreens)
        {
            bool re = true;
            string tempFileName = "temp_file" + DateTime.Now.ToString("_HHmmss") + ".txt";
            CsFileModifier csFileModifier = new CsFileModifier();
            foreach (CsFile csFile in listCsFiles)
            {
                re &= csFileModifier.ModifyConstructorFunc(csFile, listScreens, tempFileName);
            }
            return re;
        }
    }
}
