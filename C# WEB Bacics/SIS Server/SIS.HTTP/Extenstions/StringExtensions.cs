using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Extenstions
{
    public class StringExtensions
    {
        public static string Capitalize(string extension)
        {
            char[] letters = extension.ToCharArray();
            letters[0] = char.ToUpper(letters[0]);

            for (int i = 1; i < letters.Length; i++)
            {
                letters[i] = char.ToLower(letters[i]);
            }

            return letters.ToString();
        }
    }
}
