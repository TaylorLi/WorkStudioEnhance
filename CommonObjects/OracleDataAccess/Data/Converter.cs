using System;
using System.Collections.Generic;
using System.Text;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;
using System.Reflection;

namespace OracleDataAccess.Data
{
    public class Converter
    {
        /// <summary>
        /// Get Oracle Parameter for in/out,out type parameter.
        /// </summary>
        /// <typeparam name="T">destination type</typeparam>
        /// <param name="v">Oracle paramter</param>
        /// <returns></returns>
        public static T GetOracleParameterValue<T>(object v)
        {
            T defaultVal = default(T);
            Type t = v.GetType();
            if (v == null)
            {
                return defaultVal;
            }
            if (t.DeclaringType == typeof(T))
            {
                return (T)v;
            }
            else
            {
                PropertyInfo piIsNull = t.GetProperty("IsNull");
                PropertyInfo piVal = t.GetProperty("Value");
                if (piIsNull != null && piVal != null)
                {
                    bool isNull = (bool)piIsNull.GetValue(v, null);
                    if (isNull)
                        return defaultVal;
                    else
                    {
                        return (T)Convert.ChangeType(piVal.GetValue(v, null), typeof(T));
                    }
                }
                else
                {
                    try
                    {
                        return (T)Convert.ChangeType(v, t);
                    }
                    catch
                    {
                        return defaultVal;
                    }
                }
            }
        }
    }
}
