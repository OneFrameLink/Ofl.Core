namespace Ofl.Core
{
    public class BooleanExtensions
    {
        public static bool? TryParse(string value)
        {
            // The output.
            bool result;

            // If success, return the output.
            if (bool.TryParse(value, out result)) return result;

            // Return null.
            return null;
        }
    }
}
