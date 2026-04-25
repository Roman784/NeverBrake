using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils
{
    public static class ValuesInserterExtensions    
    {
        public static string InsertValues(this string str, params int[] values)
        {
            return str.InsertValues(values.Select(v => v.ToString()).ToArray());
        }

        public static string InsertValues(this string str, params float[] values)
        {
            return str.InsertValues(values.Select(v => v.ToString()).ToArray());
        }

        public static string InsertValues(this string str, params string[] values)
        {
            var formattedStr = new string(str);
            for (int i = 0; i < values.Length; i++)
            {
                var reg = "{" + i.ToString() + "}";
                if (formattedStr.Contains(reg))
                    formattedStr = formattedStr.Replace(reg, values[i]);
            }
            return formattedStr;
        }
    }
}