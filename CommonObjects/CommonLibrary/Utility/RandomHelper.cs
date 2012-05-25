using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary.Utility
{
    public class RandomHelper
    {
        static Random _Random = new Random();
        static char[] _Chars62 = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        static char[] _Chars36 = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        static char[] _Chars26 = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        public static string GetBase26String(int len)
        {
            string value = string.Empty;
            for (int i = 0; i < len; i++)
            {
                value += _Chars26[_Random.Next(0, 25)];
            }

            return value;
        }

        public static string GetBase36String(int len)
        {
            string value = string.Empty;
            for (int i = 0; i < len; i++)
            {
                value += _Chars36[_Random.Next(0, 35)];
            }

            return value;
        }

        public static string GetBase62String(int len)
        {
            string value = string.Empty;
            for (int i = 0; i < len; i++)
            {
                value += _Chars62[_Random.Next(0, 61)];
            }

            return value;
        }
    }
}
