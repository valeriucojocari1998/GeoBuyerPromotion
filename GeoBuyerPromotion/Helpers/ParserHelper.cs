using System.Globalization;
using System.Text.RegularExpressions;

namespace GeoBuyerPromotion.Helpers;

public record ParserHelper
{
    public static string RemoveMultipleSpaces(string? input)
    {
        if (input == null)
            return string.Empty;

        return Regex.Replace(input, @"\s+", " ");
    }


    public static decimal ParsePrice(string priceText)
    {
        decimal price;
        string cleanedText = priceText.Replace("\n", "").Replace("zł", "").Trim();
        cleanedText = RemoveMultipleSpaces(cleanedText);
        cleanedText = cleanedText.Replace(" ", ".");
        decimal.TryParse(cleanedText, NumberStyles.Any, CultureInfo.InvariantCulture, out price);
        return price;
    }

    public static string NormalizeUrl(string url)
    {
        string decodedUrl = Uri.UnescapeDataString(url);
        return decodedUrl;
    }
}
