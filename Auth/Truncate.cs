namespace Auth
{
    public static class Utils
    {
        public static string Truncate(this string text, int numSize)
        {
            if (text.Length >= numSize)
                return text.Substring(0, numSize);

            return text;
        }
    }
}