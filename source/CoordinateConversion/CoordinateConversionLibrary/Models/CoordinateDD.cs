﻿/******************************************************************************* 
  * Copyright 2015 Esri 
  *  
  *  Licensed under the Apache License, Version 2.0 (the "License"); 
  *  you may not use this file except in compliance with the License. 
  *  You may obtain a copy of the License at 
  *  
  *  http://www.apache.org/licenses/LICENSE-2.0 
  *   
  *   Unless required by applicable law or agreed to in writing, software 
  *   distributed under the License is distributed on an "AS IS" BASIS, 
  *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
  *   See the License for the specific language governing permissions and 
  *   limitations under the License. 
  ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CoordinateConversionLibrary.Models
{
    public class CoordinateDD : CoordinateBase
    {
        public CoordinateDD() { Lat = 40.123; Lon = -78.456; }

        public CoordinateDD(double lat, double lon)
        {
            Lat = lat;
            Lon = lon;
        }

        public CoordinateDD(CoordinateDDM ddm)
        {
            Lat = (Math.Abs((double)ddm.LatDegrees) + (ddm.LatMinutes / 60.0)) * ((ddm.LatDegrees < 0) ? -1.0 : 1.0);
            Lon = (Math.Abs((double)ddm.LonDegrees) + (ddm.LonMinutes / 60.0)) * ((ddm.LonDegrees < 0) ? -1.0 : 1.0);
        }

        public CoordinateDD(CoordinateDMS dms)
        {
            Lat = (Math.Abs((double)dms.LatDegrees) + ((double)dms.LatMinutes / 60.0) + (dms.LatSeconds / 3600.0)) * ((dms.LatDegrees < 0) ? -1.0 : 1.0);
            Lon = (Math.Abs((double)dms.LonDegrees) + ((double)dms.LonMinutes / 60.0) + (dms.LatSeconds / 3600.0)) * ((dms.LonDegrees < 0) ? -1.0 : 1.0);
        }

        #region Properties

        private double _lat = 40.446;
        private double _lon = -79.982;

        public double Lat
        {
            get { return _lat; }
            set { _lat = value; }
        }

        public double Lon
        {
            get { return _lon; }
            set { _lon = value; }
        }
        
        #endregion Properties

        #region Methods

        public static bool TryParse(string input, out CoordinateDD coord)
        {
            coord = new CoordinateDD();

            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = input.Trim();

            Regex regexDD = new Regex(@"^ *[+]*(?<latitudeSuffix>[NS])?(?<latitude>[^NSDd*° ,]*)?[Dd*° ,]*(?<latitudeSuffix>[NS])?[+,;:\s]*(?<longitudeSuffix>[EW])?(?<longitude>[^EWDd*° ]*)?[Dd*° ]*(?<longitudeSuffix>[EW])?");

            var matchDD = regexDD.Match(input);

            if (matchDD.Success && matchDD.Length == input.Length)
            {
                if (ValidateNumericCoordinateMatch(matchDD, new string[] { "latitude", "longitude" }))
                {
                    try
                    {
                        coord.Lat = Double.Parse(matchDD.Groups["latitude"].Value);
                        coord.Lon = Double.Parse(matchDD.Groups["longitude"].Value);

                        if (coord.Lat > 90.0 || coord.Lat < -90.0)
                            return false;
                        if (coord.Lon > 180.0 || coord.Lon < -180.0)
                            return false;

                        var temp = matchDD.Groups["latitudeSuffix"];
                        if (temp.Success && temp.Value.ToUpper().Equals("S"))
                        {
                            coord.Lat = Math.Abs(coord.Lat) * -1;
                        }
                        temp = matchDD.Groups["longitudeSuffix"];
                        if (temp.Success && temp.Value.ToUpper().Equals("W"))
                        {
                            coord.Lon = Math.Abs(coord.Lon) * -1;
                        }
                    }
                    catch
                    {
                        return false;
                    }

                    return true;
                }        
            }

            return false;
        }

        #endregion Methods

        #region ToString

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            var temp = base.ToString(format, formatProvider);

            if (!string.IsNullOrWhiteSpace(temp))
                return temp;

            var sb = new StringBuilder();

            if (format == null)
                format = "DD";

            NumberFormatInfo fi = NumberFormatInfo.InvariantInfo;

            switch (format.ToUpper())
            {
                case "":
                case "DD":
                    sb.AppendFormat(fi, "x = {0:0.0000}", this.Lon);
                    sb.AppendFormat(fi, " y = {0:0.0000}", this.Lat);
                    break;
                default:
                    throw new Exception("CoordinateDD.ToString(): Invalid formatting string.");
            }

            return sb.ToString();
        }

        #endregion ToString
    }

    public class CoordinateDDFormatter : CoordinateFormatterBase
    {
        public override string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is CoordinateDD)
            {
                if (string.IsNullOrWhiteSpace(format))
                {
                    return this.Format("Y-0.000000 X-0.000000", arg, this);
                }
                else
                {
                    var coord = arg as CoordinateDD;
                    var cnum = coord.Lat;
                    var sb = new StringBuilder();
                    var olist = new List<object>();
                    bool startIndexNeeded = false;
                    bool endIndexNeeded = false;
                    int currentIndex = 0;

                    foreach (char c in format)
                    {
                        if (startIndexNeeded && (c == '#' || c == '.' || c == '0'))
                        {
                            // add {<index>:
                            sb.AppendFormat("{{{0}:", currentIndex++);
                            startIndexNeeded = false;
                            endIndexNeeded = true;
                        }

                        if (endIndexNeeded && (c != '#' && c != '.' && c != '0'))
                        {
                            sb.Append("}");
                            endIndexNeeded = false;
                        }

                        switch (c)
                        {
                            case 'X': // longitude coordinate
                                cnum = coord.Lon;
                                olist.Add(Math.Abs(cnum));
                                startIndexNeeded = true;
                                break;
                            case 'Y': // latitude coordinate
                                cnum = coord.Lat;
                                olist.Add(Math.Abs(cnum));
                                startIndexNeeded = true;
                                break;
                            case '+': // show +
                                if (cnum > 0.0)
                                    sb.Append("+");
                                break;
                            case '-': // show -
                                if (cnum < 0.0)
                                    sb.Append("-");
                                break;
                            case 'N':
                            case 'S': // N or S
                                if (coord.Lat > 0.0)
                                    sb.Append("N"); // do we always want UPPER
                                else
                                    sb.Append("S");
                                break;
                            case 'E':
                            case 'W': // E or W
                                if (coord.Lon > 0.0)
                                    sb.Append("E");
                                else
                                    sb.Append("W");
                                break;
                            default:
                                sb.Append(c);
                                break;
                        }
                    }

                    if (endIndexNeeded)
                    {
                        sb.Append("}");
                        endIndexNeeded = false;
                    }

                    return String.Format(sb.ToString(), olist.ToArray());

                }
            }

            if (arg is IFormattable)
            {
                return ((IFormattable)arg).ToString(format, formatProvider);
            }
            else
            {
                return arg.ToString();
            }
        }
    }
}
