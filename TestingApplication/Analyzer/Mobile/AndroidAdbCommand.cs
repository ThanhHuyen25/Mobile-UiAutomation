﻿// Copyright (c) 2018 fit.uet.vnu.edu.vn
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
            int i = 26;
            List<AndroidDevice> Devices = new List<AndroidDevice>();
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "adb";
            p.StartInfo.Arguments = "devices";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.Start();
            string output1 = p.StandardOutput.ReadToEnd();
            int l = output1.Length;
            while ((i + 2) < output1.Length)
            {
                string test = output1.Substring(i, 19);
                AndroidDevice android = new AndroidDevice();
                android.Ip = test;
                p.StartInfo.Arguments = "-s" + test + " shell getprop ro.product.model";
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                output = output.Replace("\r", "");
                output = output.Replace("\n", "");
                android.Name = output;
                Devices.Add(android);
                i = i + 28;

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
            p.StartInfo.Arguments = "-s "+device.Ip+ " shell uiautomator dump";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.Start();
            string command = p.StandardOutput.ReadToEnd();
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
                p.StartInfo.Arguments ="-s "+device.Ip+ " pull " + path + " C:/Users/catty/Desktop";
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                return "C:/Users/catty/Desktop/window_dump.xml";

                // string output = "pull " + path + " C:/Users/catty/Desktop";
                //this.TextBox1.Text = output;
            }
            

            // send command "adb dump" to $device
            throw new NotImplementedException();
        }
    }
}
