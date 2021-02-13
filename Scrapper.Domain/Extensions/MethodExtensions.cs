
namespace Scrapper.Domain.Extensions
{
    public static class MethodExtensions
    {
        public static string AddWithComma(this string baseString, string additionalString)
        {
            if (string.IsNullOrEmpty(additionalString))
                return baseString;

            if (!string.IsNullOrEmpty(baseString))
                baseString = ", ";

            baseString += additionalString;

            return baseString;
        }
    }
}
