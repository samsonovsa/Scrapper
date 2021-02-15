
using System.Text;

namespace Scrapper.Domain.Extensions
{
    public static class MethodExtensions
    {
        //public static void AddWithComma(this string baseString, string additionalString)
        //{

        //    if (string.IsNullOrEmpty(additionalString))
        //        return;

        //    StringBuilder stringBuilder = new StringBuilder(baseString);
            
        //    if (!string.IsNullOrEmpty(baseString))
        //        stringBuilder.Append(", ");

        //    stringBuilder.Append(additionalString);
        //    baseString = stringBuilder.ToString();

        //    return;
        //}

        public static string AddWithComma(this string baseString, string additionalString)
        {

            if (string.IsNullOrEmpty(additionalString))
                return baseString;

            if (!string.IsNullOrEmpty(baseString))
                baseString += ", ";

            baseString += additionalString;

            return baseString;
        }

        public static string RemoveFrom(this string baseString, string patten)
        {
            int i = baseString.LastIndexOf(patten, System.StringComparison.InvariantCultureIgnoreCase);
            if (i >= 0)
            {
                baseString = baseString.Remove(i);
            }

            return baseString;
        }
    }
}
