using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAIF
{
    public static class EnergyClass
    {
        public static String GetBaseLevel(int floors, int lifts, int degrees, bool heatingOnly = false)
        {
            double value = 0;

            degrees = (int)(Math.Round(((double)degrees) / 1000, 0));

            switch (degrees)
            {
                case 1:
                case 2:
                    if (floors <= 2) value = 215;
                    if (floors > 2 && floors <= 4) value = 206;
                    if (floors > 4 && floors <= 6) value = 203;
                    if (floors > 6 && floors <= 8) value = 201;
                    if (floors > 8 && floors <= 10) value = 199;
                    if (floors >= 11) value = 198;
                    break;
                case 3:
                    if (floors <= 2) value = 228;
                    if (floors > 2 && floors <= 4) value = 216;
                    if (floors > 4 && floors <= 6) value = 212;
                    if (floors > 6 && floors <= 8) value = 208;
                    if (floors > 8 && floors <= 10) value = 205;
                    if (floors >= 11) value = 203;
                    break;
                case 4:
                    if (floors <= 2) value = 256;
                    if (floors > 2 && floors <= 4) value = 239;
                    if (floors > 4 && floors <= 6) value = 234;
                    if (floors > 6 && floors <= 8) value = 229;
                    if (floors > 8 && floors <= 10) value = 225;
                    if (floors >= 11) value = 223;
                    break;
                case 5:
                    if (floors <= 2) value = 284;
                    if (floors > 2 && floors <= 4) value = 263;
                    if (floors > 4 && floors <= 6) value = 256;
                    if (floors > 6 && floors <= 8) value = 251;
                    if (floors > 8 && floors <= 10) value = 245;
                    if (floors >= 11) value = 242;
                    break;
                case 6:
                    if (floors <= 2) value = 312;
                    if (floors > 2 && floors <= 4) value = 287;
                    if (floors > 4 && floors <= 6) value = 278;
                    if (floors > 6 && floors <= 8) value = 272;
                    if (floors > 8 && floors <= 10) value = 265;
                    if (floors >= 11) value = 262;
                    break;
                case 7:
                    if (floors <= 2) value = 341;
                    if (floors > 2 && floors <= 4) value = 316;
                    if (floors > 4 && floors <= 6) value = 302;
                    if (floors > 6 && floors <= 8) value = 294;
                    if (floors > 8 && floors <= 10) value = 286;
                    if (floors >= 11) value = 283;
                    break;
                case 8:
                    if (floors <= 2) value = 370;
                    if (floors > 2 && floors <= 4) value = 337;
                    if (floors > 4 && floors <= 6) value = 326;
                    if (floors > 6 && floors <= 8) value = 317;
                    if (floors > 8 && floors <= 10) value = 308;
                    if (floors >= 11) value = 323;
                    break;
                case 9:
                    if (floors <= 2) value = 398;
                    if (floors > 2 && floors <= 4) value = 360;
                    if (floors > 4 && floors <= 6) value = 348;
                    if (floors > 6 && floors <= 8) value = 338;
                    if (floors > 8 && floors <= 10) value = 328;
                    if (floors >= 11) value = 203;
                    break;
                case 10:
                default:
                    if (floors <= 2) value = 426;
                    if (floors > 2 && floors <= 4) value = 384;
                    if (floors > 4 && floors <= 6) value = 370;
                    if (floors > 6 && floors <= 8) value = 359;
                    if (floors > 8 && floors <= 10) value = 348;
                    if (floors >= 11) value = 342;
                    break;
            }

            if (lifts == 0) value -= 3;

            return value.ToString();
        }

        public static string GetEnergyClass(double value)
        {
            if (value <= -60) return "A++";
            if (value > -60 && value <= -50) return "A+";
            if (value > -50 && value <= -40) return "A";
            if (value > -40 && value <= -30) return "B";
            if (value > -30 && value <= -15) return "C";
            if (value > -15 && value <= 0) return "D";
            if (value <= 25 && value > 0) return "E";
            if (value <= 50 && value > 25) return "F";
            if (value > 50) return "G";
            return "None";
        }
    }
}
