// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 3:12 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class ParameterExpression
    {
        private String name;
        private String description;

        public ParameterExpression() { }

        public ParameterExpression(String name, String description)
        {
            this.name = name;
            this.description = description;
        }

        public String getName()
        {
            return name;
        }

        public void setName(String name)
        {
            this.name = name;
        }

        public String getDescription()
        {
            return description;
        }

        public void setDescription(String description)
        {
            this.description = description;
        }
    }
}
