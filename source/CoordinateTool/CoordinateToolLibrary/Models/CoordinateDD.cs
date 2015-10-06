﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoordinateToolLibrary.Models
{
    public class CoordinateDD : CoordinateBase
    {
        public CoordinateDD() { }

        public CoordinateDD(double lat, double lon)
        {
            Lat = lat;
            Lon = lon;
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
            coord = new CoordinateDD(0.0,0.0);

            input = input.Trim();

            Regex regexDD = new Regex("^ *[+]*(?<latitudeSuffix>[NS])?(?<latitude>[^NSDd*° ,]*)?[Dd*° ,]*(?<latitudeSuffix>[NS])? *[+,]*(?<longitudeSuffix>[EW])?(?<longitude>[^EWDd*° ]*)?[Dd*° ]*(?<longitudeSuffix>[EW])?");

            var matchDD = regexDD.Match(input);

            if (matchDD.Success && matchDD.Length == input.Length)
            {
                if (ValidateNumericCoordinateMatch(matchDD, new string[] { "latitude", "longitude" }))
                {
                    try
                    {
                        coord.Lat = Double.Parse(matchDD.Groups["latitude"].Value);
                        coord.Lon = Double.Parse(matchDD.Groups["longitude"].Value);

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

            //if (formatProvider != null)
            //{
            //    if (formatProvider is CoordinateDDFormatter && !format.Contains("{0:"))
            //    {
            //        format = string.Format("{{0:{0}}}", format);
            //    }

            //    return string.Format(formatProvider, format, new object[] { this });
            //}

            var sb = new StringBuilder();

            if (format == null)
                format = "DD";

            NumberFormatInfo fi = NumberFormatInfo.InvariantInfo;

            switch (format.ToUpper())
            {
                case "":
                case "DD":
                    sb.AppendFormat(fi, "x = {0:#.0000}", this.Lon);
                    sb.AppendFormat(fi, " y = {0:#.0000}", this.Lat);
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
        //public object GetFormat(Type formatType)
        //{
        //    return (formatType == typeof(ICustomFormatter)) ? this : null;
        //}

        public override string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is CoordinateDD)
            {
                if (string.IsNullOrWhiteSpace(format))
                {
                    return this.Format("Y-##.0000 X-###.0000", arg, this);
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

                    foreach (char c in format.ToUpper())
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
                            case 'N': // N or S
                                if (coord.Lat > 0.0)
                                    sb.Append("N"); // do we always want UPPER
                                else
                                    sb.Append("S");
                                break;
                            case 'E': // E or W
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