using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OracleCodeBuilderLibrary.SQL
{
    public class Util
    {       
        public static string ConvertStyle(string input, CodeDecorateStyle style)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            if (style == CodeDecorateStyle.None)
                return input;
            else
            {
                List<string> arrs = new List<string>();
                Regex r = new Regex("^([a-z0-9]+?(?<g1>[^a-z0-9]))+?([a-z0-9]+?)$", RegexOptions.IgnoreCase);
                string splitStr = string.Empty;
                if (r.IsMatch(input))
                {
                    GroupCollection gs = r.Match(input).Groups;
                    splitStr = gs["g1"].Value;
                    arrs.AddRange(input.Split(new string[] { splitStr }, StringSplitOptions.RemoveEmptyEntries));
                }
                else
                {
                    arrs.Add(input);
                }
                StringBuilder sb = new StringBuilder();
                int i = 0;
                foreach (string v in arrs)
                {
                    if (style == CodeDecorateStyle.Pascal)
                    {
                        if (i != 0)
                            sb.Append(splitStr);
                        sb.Append(v.Substring(0, 1).ToUpper());
                        if (v.Length > 1)
                            sb.Append(v.Substring(1, v.Length - 1).ToLower());
                    }
                    else
                    {
                        if (i == 0)
                        {
                            sb.Append(v.Substring(0, 1).ToLower());
                        }
                        else
                        {
                            sb.Append(splitStr);
                            sb.Append(v.Substring(0, 1).ToUpper());
                        }
                        if (v.Length > 1)
                            sb.Append(v.Substring(1, v.Length - 1).ToLower());
                    }
                    i++;
                }
                return sb.ToString();
            }
        }
    }
}
