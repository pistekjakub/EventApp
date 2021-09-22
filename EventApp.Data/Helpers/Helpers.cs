namespace EventApp.Data
{
    public static class Helpers
    {
        public static string TrimSafe(string? inputString)
        {
            if (!string.IsNullOrEmpty(inputString))
            {
                return inputString.Trim();
            }

            return inputString;
        }
    }
}
