﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BaseStation
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public Location StationLocation { get; set; }
        public int ChargeSlots { get; set; }

        private List<DroneInCharge> droneInChargesList = new List<DroneInCharge>();

        public List<DroneInCharge> DroneInChargesList
        {
            get
            {
                return droneInChargesList;
            }
            set
            {
                droneInChargesList = value;
            }
        }

        public override string ToString()
        {

            #region Longitude & Latitude Calculations

            char lon = 'N';
            if (StationLocation.Longitude < 0)
            {
                lon = 'S';
                StationLocation.Longitude *= -1;
            }
            double lonDegreesWithFraction = StationLocation.Longitude;
            int londegrees = (int)lonDegreesWithFraction; // = 48

            double lonfractionalDegrees = lonDegreesWithFraction - londegrees; // = .858222
            double lonminutesWithFraction = 60 * lonfractionalDegrees; // = 51.49332
            int lonminutes = (int)lonminutesWithFraction; // = 51

            double lonfractionalMinutes = lonminutesWithFraction - lonminutes; // = .49332
            double lonsecondsWithFraction = 60 * lonfractionalMinutes; // = 29.6

            char lat = 'E';
            if (StationLocation.Latitude < 0)
            {
                lat = 'W';
                StationLocation.Latitude *= -1;
            }

            double latDegreesWithFraction = StationLocation.Latitude;
            int latdegrees = (int)latDegreesWithFraction; // = 48

            double latfractionalDegrees = latDegreesWithFraction - latdegrees; // = .858222
            double latminutesWithFraction = 60 * latfractionalDegrees; // = 51.49332
            int latminutes = (int)latminutesWithFraction; // = 51

            double latfractionalMinutes = latminutesWithFraction - latminutes; // = .49332
            double latsecondsWithFraction = 60 * latfractionalMinutes; // = 29.6

            #endregion

            string DronesPrinted = "";
            foreach(DroneInCharge DIC in DroneInChargesList)
            {
                DronesPrinted += "ID: " + DIC.Id + " Battery: " + DIC.Battery + "\n";
            }

            return "ID: " + Id + "\nName: " + Name +
                "\nLongitude: " + londegrees + "°" + lonminutes + "'" + Math.Round(lonsecondsWithFraction, 3) + "\"" + lon +
                "       Latitude: " + latdegrees + "°" + latminutes + "'" + Math.Round(latsecondsWithFraction, 3) + "\"" + lat +
                "\nChargeSlots: " + ChargeSlots + "\nDrones in charging:\n" + ((DronesPrinted == "") ? "None" : DronesPrinted);

        }
    }
}
