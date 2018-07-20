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
        public AndroidDevice() { }
        public AndroidDevice(string serialNumber, string state, string ip, string name)
        {
            this.serialNumber = serialNumber;
            this.state = state;
            this.ip = ip;
            this.name = name;
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
    }
}
