
namespace Obelus
{
    public static class StringExtension
    {
        public static string OrIfEmpty(this string s, string defaultValue)
        {
            if (s == null || s.Length == 0)
            {
                return defaultValue;
            }

            return s;
        }
    }
}
