// Copyright (c) 2018 fit.uet.vnu.edu.vn
// author @duongtd
// created on 10:12 AM 2018/7/3
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class AndroidAdbCommand
    {
        public List<AndroidDevice> GetAllDevices()
        {
            // send adb command "adb devices -l" here
            int i = 26;
            List<AndroidDevice> Devices = new List<AndroidDevice>();

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "adb";
            p.StartInfo.Arguments = "devices";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true; //an man hinh
            p.Start();
            string output1 = p.StandardOutput.ReadToEnd();

            int l = output1.Length - 1;
            while ((i + 2) <l)
            {

                string test = output1.Substring(i, 20);
                AndroidDevice android = new AndroidDevice();
                android.Ip = test;
                p.StartInfo.Arguments = "-s" + test + " shell getprop ro.product.model";
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true; // an man hinh
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                output = output.Replace("\r", "");
                output = output.Replace("\n", "");
                android.Name = output;
                Devices.Add(android);
                i = i + 28 + 1;

            }
            //throw new NotImplementedException();
            return Devices;
        }

        /// <summary>
        /// get GUI hierarchy of device
        /// </summary>
        /// <param name="device">target</param>
        /// <returns>path to dump .xml file</returns>
        public string DumpGUI(AndroidDevice device)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "adb";
            p.StartInfo.Arguments = "-s " + device.Ip + " shell uiautomator dump -a";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true; //an man hinh
            p.Start();
            string command = p.StandardOutput.ReadToEnd();
            //p.Close();
            command = command.Replace("\r", " ");
            string path = "";
            string[] words = command.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Contains(".xml"))
                {
                    path = words[i];
                }
            }
            if (path != null)
            {
                p.StartInfo.Arguments = "-s " + device.Ip + " pull " + path + " C:/ProgramData";
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.Start();
                //string output = p.StandardOutput.ReadToEnd();
               
                p.StartInfo.Arguments = "-s " + device.Ip + " shell screencap -p /sdcard/screen.png";
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                Thread.Sleep(1000);
                p.StartInfo.Arguments = "-s " + device.Ip + " pull /sdcard/screen.png C:/ProgramData/screen.png";
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                Thread.Sleep(1000);

                return "C:/ProgramData/window_dump.xml";

            }
            //Lấy activity của package
            // adb shell "dumpsys window windows | grep -E mCurrentFocus"
            // send command "adb dump" to $device
            throw new NotImplementedException();
        }
        //public void getImage()
        //{
        //    System.Diagnostics.Process p = new System.Diagnostics.Process();
        //    p.StartInfo.UseShellExecute = false;
            
         
        //}
    }
}
