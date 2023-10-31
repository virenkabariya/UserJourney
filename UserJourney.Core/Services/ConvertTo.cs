using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UserJourney.Core.Constants;

namespace UserJourney.Core.Services
{
    public static class ConvertTo
    {
        #region Common type conversion

        public static string String(this object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(System.DBNull))
                {
                    return Convert.ToString(readField, CultureInfo.InvariantCulture);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ToStringTrim(this object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(System.DBNull))
                {
                    return readField.ToString().Trim();
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static bool ToBoolean(this object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(System.DBNull))
                {
                    string stringReadField = Convert.ToString(readField, CultureInfo.InvariantCulture);

                    if (stringReadField == "1")
                    {
                        return true;
                    }

                    bool x;
                    if (bool.TryParse(stringReadField, out x))
                    {
                        return x;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static int ToInteger(this object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(System.DBNull))
                {
                    if (readField.ToString().Trim().Length == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        int toReturn;
                        if (int.TryParse(Convert.ToString(readField, CultureInfo.InvariantCulture), out toReturn))
                        {
                            return toReturn;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public static DateTime ToDate(this object readField)
        {
            return Convert.ToDateTime(readField, CultureInfo.CurrentCulture);
        }
        #endregion
    }
}
