using System.Data;

namespace CarModelManagementAPI.DataAccess
{
    public static class DbField
    {
        public static bool GetBoolean(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return false;
            }
            return rec.GetBoolean(fieldIndex);
        }

        public static bool GetBoolean(IDataRecord rec, string fieldName)
        {
            return GetBoolean(rec, rec.GetOrdinal(fieldName));
        }

        public static bool GetBoolean(IDataRecord rec, int fieldIndex, decimal compareTo)
        {
            return (GetDecimal(rec, fieldIndex) == compareTo);
        }

        public static bool GetBoolean(IDataRecord rec, int fieldIndex, int compareTo)
        {
            return (GetInt(rec, fieldIndex) == compareTo);
        }

        public static bool GetBoolean(IDataRecord rec, int fieldIndex, string compareTo)
        {
            return (GetString(rec, fieldIndex) == compareTo);
        }

        public static bool GetBoolean(IDataRecord rec, string fieldName, decimal compareTo)
        {
            return (GetDecimal(rec, rec.GetOrdinal(fieldName)) == compareTo);
        }

        public static bool GetBoolean(IDataRecord rec, string fieldName, int compareTo)
        {
            return (GetInt32(rec, rec.GetOrdinal(fieldName)) == compareTo);
        }

        public static bool GetBoolean(IDataRecord rec, string fieldName, string compareTo)
        {
            return (GetString(rec, rec.GetOrdinal(fieldName)) == compareTo);
        }

        public static byte GetByte(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return 0;
            }
            return rec.GetByte(fieldIndex);
        }

        public static byte GetByte(IDataRecord rec, string fieldName)
        {
            return GetByte(rec, rec.GetOrdinal(fieldName));
        }

        public static DateTime GetDateTime(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return DateTime.MinValue;
            }
            return rec.GetDateTime(fieldIndex);
        }
        public static DateTime? GetNullableDateTime(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return null;
            }
            return rec.GetDateTime(fieldIndex);
        }

        public static DateTime GetDateTime(IDataRecord rec, string fieldName)
        {
            return GetDateTime(rec, rec.GetOrdinal(fieldName));
        }
        public static DateTime? GetNullableDateTime(IDataRecord rec, string fieldName)
        {
            return GetNullableDateTime(rec, rec.GetOrdinal(fieldName));
        }

        public static decimal GetDecimal(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return 0M;
            }
            return rec.GetDecimal(fieldIndex);
        }

        public static decimal? GetNullableDecimal(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return null;
            }
            return rec.GetDecimal(fieldIndex);
        }
        public static decimal? GetNullableDecimal(IDataRecord rec, string fieldName)
        {
            return GetNullableDecimal(rec, rec.GetOrdinal(fieldName));
        }

        public static decimal GetDecimal(IDataRecord rec, string fieldName)
        {
            return GetDecimal(rec, rec.GetOrdinal(fieldName));
        }

        public static double GetDouble(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return 0.0;
            }
            return rec.GetDouble(fieldIndex);
        }

        public static double GetDouble(IDataRecord rec, string fieldName)
        {
            return GetDouble(rec, rec.GetOrdinal(fieldName));
        }

        public static float GetFloat(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return 0f;
            }
            if (rec[fieldIndex].GetType().FullName == "System.Decimal")
            {
                return decimal.ToSingle(rec.GetDecimal(fieldIndex));
            }
            return rec.GetFloat(fieldIndex);
        }

        public static float GetFloat(IDataRecord rec, string fieldName)
        {
            return GetFloat(rec, rec.GetOrdinal(fieldName));
        }

        public static Guid GetGuid(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return Guid.Empty;
            }
            if (rec.GetValue(fieldIndex).GetType().ToString().Equals("System.String"))
                return new Guid(rec.GetString(fieldIndex));
            else
                return rec.GetGuid(fieldIndex);
        }

        public static Guid GetGuid(IDataRecord rec, string fieldName)
        {

            return GetGuid(rec, rec.GetOrdinal(fieldName));
        }

        public static int? GetNullableInt(IDataRecord rec, string fieldName)
        {
            return GetNullableInt(rec, rec.GetOrdinal(fieldName));
        }


        public static int? GetNullableInt(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return null;
            }
            if (rec[fieldIndex].GetType().FullName == "System.Decimal")
            {
                return decimal.ToInt32(rec.GetDecimal(fieldIndex));
            }
            if (rec[fieldIndex].GetType().FullName == "System.String")
            {

                string tmpVal = rec.GetString(fieldIndex);
                if (string.IsNullOrWhiteSpace(tmpVal))
                    return null;
                else
                {
                    int val;
                    if (Int32.TryParse(tmpVal, out val))
                    {
                        return val;
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            return rec.GetInt32(fieldIndex);
        }

        public static int GetInt(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return 0;
            }
            if (rec[fieldIndex].GetType().FullName == "System.Decimal")
            {
                return decimal.ToInt32(rec.GetDecimal(fieldIndex));
            }
            if (rec[fieldIndex].GetType().FullName == "System.String")
            {

                string tmpVal = rec.GetString(fieldIndex);
                if (string.IsNullOrWhiteSpace(tmpVal))
                    return 0;
                else
                {
                    int val;
                    if (Int32.TryParse(tmpVal, out val))
                    {
                        return val;
                    }
                    else
                    {
                        return 0;
                    }
                }

            }
            return rec.GetInt32(fieldIndex);
        }

        public static int GetInt(IDataRecord rec, string fieldName)
        {
            return GetInt(rec, rec.GetOrdinal(fieldName));
        }

        public static short GetInt16(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return 0;
            }
            return rec.GetInt16(fieldIndex);
        }

        public static short GetInt16(IDataRecord rec, string fieldName)
        {
            return GetInt16(rec, rec.GetOrdinal(fieldName));
        }

        public static int GetInt32(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return 0;
            }
            return GetInt(rec, fieldIndex);
        }

        public static int GetInt32(IDataRecord rec, string fieldName)
        {
            return GetInt32(rec, rec.GetOrdinal(fieldName));
        }

        public static long GetInt64(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return 0L;
            }
            return rec.GetInt64(fieldIndex);
        }

        public static long GetInt64(IDataRecord rec, string fieldName)
        {
            return GetInt64(rec, rec.GetOrdinal(fieldName));
        }

        public static string GetString(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return "";
            }
            return rec.GetString(fieldIndex);
        }

        public static string GetString(IDataRecord rec, string fieldName)
        {
            return GetString(rec, rec.GetOrdinal(fieldName));
        }

        public static T GetValue<T>(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return default(T);
            }
            return (T)rec.GetValue(fieldIndex);
        }

        public static T GetValue<T>(IDataRecord rec, string fieldName)
        {
            return GetValue<T>(rec, rec.GetOrdinal(fieldName));
        }
        public static Boolean? GetNullableBoolean(IDataRecord rec, string fieldName)
        {
            return GetNullableBoolean(rec, rec.GetOrdinal(fieldName));
        }
        public static Boolean? GetNullableBoolean(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return null;
            }
            return rec.GetBoolean(fieldIndex);
        }

        public static Int64? GetNullableInt64(IDataRecord rec, string fieldName)
        {
            return GetNullableInt64(rec, rec.GetOrdinal(fieldName));
        }
        public static Int64? GetNullableInt64(IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex))
            {
                return null;
            }
            if (rec[fieldIndex].GetType().FullName == "System.Decimal")
            {
                return decimal.ToInt64(rec.GetDecimal(fieldIndex));
            }
            if (rec[fieldIndex].GetType().FullName == "System.String")
            {
                string tmpVal = rec.GetString(fieldIndex);
                if (string.IsNullOrWhiteSpace(tmpVal))
                    return null;
                else
                {
                    Int64 val;
                    if (Int64.TryParse(tmpVal, out val))
                    {
                        return val;
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            return rec.GetInt64(fieldIndex);
        }
    }
}
