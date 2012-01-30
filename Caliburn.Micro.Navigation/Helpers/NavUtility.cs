using System;
using System.Collections.Generic;
using System.Windows.Browser;

namespace Caliburn.Micro.Navigation.Helpers
{
    public static class NavUtility
    {
        public static IDictionary<string, string> ParseQueryString(string paramsString)
        {
            if (string.IsNullOrEmpty(paramsString))
                throw new ArgumentNullException("paramsString");

            // convert to dictionary
            var dict = new Dictionary<string, string>();

            // remove the leading ?
            if (paramsString.StartsWith("?"))
                paramsString = paramsString.Substring(1);

            var length = paramsString.Length;

            for (var i = 0; i < length; i++)
            {
                var startIndex = i;
                var pivotIndex = -1;

                while (i < length)
                {
                    char ch = paramsString[i];
                    if (ch == '=')
                    {
                        if (pivotIndex < 0)
                        {
                            pivotIndex = i;
                        }
                    }
                    else if (ch == '&')
                    {
                        break;
                    }
                    i++;
                }

                string name;
                string value;
                if (pivotIndex >= 0)
                {
                    name = paramsString.Substring(startIndex, pivotIndex - startIndex);
                    value = paramsString.Substring(pivotIndex + 1, (i - pivotIndex) - 1);
                }
                else
                {
                    name = paramsString.Substring(startIndex, i - startIndex);
                    value = null;
                }

                dict.Add(HttpUtility.UrlDecode(name), HttpUtility.UrlDecode(value));

                // if string ends with ampersand, add another empty token
                if ((i == (length - 1)) && (paramsString[i] == '&'))
                    dict.Add(null, string.Empty);
            }

            return dict;
        }
    }
}
