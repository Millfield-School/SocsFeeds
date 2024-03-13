using System.Collections.Generic;
using System.Linq;

namespace SocsFeeds.helpers
{
    public static class ParserUtility
    {
        public static List<T> ParseCommaSeparatedValues<T>(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new List<T>();
            }

            return input.Split(',')
                .Select(s => ConvertToType<T>(s))
                .Where(n => n != null) 
                .Select(n => (T)n) 
                .ToList();
        }

        private static object ConvertToType<T>(string input)
        {
            try
            {
                if (typeof(T) == typeof(int))
                {
                    if (int.TryParse(input, out int result))
                    {
                        return result;
                    }
                }
                else if (typeof(T) == typeof(string))
                {
                    return input; 
                }
            }
            catch
            {
                return null;
            }
            return null; 
        }
    }
}
