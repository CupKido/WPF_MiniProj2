﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneCharge
    {
        public int DroneId { get; set; }
        public int BaseStationId { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
