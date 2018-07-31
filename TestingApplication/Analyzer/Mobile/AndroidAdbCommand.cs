// Copyright (c) 2018 fit.uet.vnu.edu.vn
// author @duongtd
// created on 10:12 AM 2018/7/3
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class AndroidAdbCommand
    {
        public List<AndroidDevice> GetAllDevices()
        {
            // send adb command "adb devices -l" here
            throw new NotImplementedException();
        }

        /// <summary>
        /// get GUI hierarchy of device
        /// </summary>
        /// <param name="device">target</param>
        /// <returns>path to dump .xml file</returns>
        public string DumpGUI(AndroidDevice device)
        {
            // send command "adb dump" to $device
            throw new NotImplementedException();
        }
    }
}
