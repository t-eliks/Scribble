namespace Scribble.Logic
{
    using System;

    public static class StringHelper
    {
        public static bool Contains(string str, string query)
        {
            return str.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
