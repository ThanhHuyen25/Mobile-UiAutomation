// Copyright (c) 2018 fit.uet.vnu.edu.vn
// author @duongtd
// created on 10:13 AM 2018/7/3
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    /// <summary>
    /// represent for a android device
    /// </summary>
    public class AndroidDevice
    {
        string serialNumber;
        string state;
        string description;
        string ip;
        string name;
        string version;
        string activity;
        string package;
        public AndroidDevice() { }
        public AndroidDevice(string ip, string name, string version, string activity, string package)
        {
            this.ip = ip;
            this.name = name;
            this.version = version;
            this.activity = activity;
            this.package = package;
        }

        public string SerialNumber
        {
            get
            {
                return serialNumber;
            }

            set
            {
                serialNumber = value;
            }
        }

        public string State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }
        public string Ip
        {
            get
            {
                return ip;
            }

            set
            {
                ip = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
        public string Version
        {
            get
            {
                return version;
            }

            set
            {
                version = value;
            }
        }
        public string Activity
        {
            get
            {
                return activity;
            }

            set
            {
                activity = value;
            }
        }
        public string Package
        {
            get
            {
                return package;
            }

            set
            {
                package = value;
            }
        }

    }
}
