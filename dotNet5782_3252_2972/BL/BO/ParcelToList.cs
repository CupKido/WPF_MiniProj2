﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelToList
    {

        public int Id { get; set; }

        public String SenderName { get; set; }

        public String TargetName { get; set; }

        public WeightCategories Weight { get; set; }

        public Priorities Priority { get; set; }

    }
}
