using System.Globalization;
using System.Text.RegularExpressions;
using GeoBuyerPromotion.Models;
using HtmlAgilityPack;

namespace GeoBuyerPromotion.Parsers
{
	public record BiedronkaParser : IParser
	{
        public List<Product> GetProducts(string html)
        {

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var products = GetProductsInternal(doc);
            return products;
        }

        private static List<Product> GetProductsInternal(HtmlDocument document)
        {
            var products = new List<Product>();

            var productNodes = document.DocumentNode.SelectNodes("//div[contains(@class, 'product-tile js-product-tile')]");
            if (productNodes == null)
                return products;

            foreach (var productNode in productNodes)
            {
                string name = "";
                string category = "";
                string saleSpecification = "";
                decimal salePrice = decimal.MinValue;
                decimal oldPrice = decimal.MinValue;
                string priceLabel = "";
                // Parse name
                var nameNode = productNode.SelectSingleNode(".//div[contains(@class, 'product-tile__name product-tile__name--overflow')]");
                if (nameNode != null)
                    name = RemoveMultipleSpaces(nameNode.InnerText);

                var categoryNode = productNode.SelectSingleNode(".//div[contains(@class, 'product-tile__name')]");
                if (categoryNode != null)
                    category = RemoveMultipleSpaces(categoryNode.InnerText);

                var saleSpecificationNode = productNode.SelectSingleNode(".//div[contains(@class, 'badge-tile__item\n badge-tile__item--text')]");
                if (saleSpecificationNode != null)
                    saleSpecification = RemoveMultipleSpaces(saleSpecificationNode.InnerText);

                // Parse sale price
                var salePriceNode = productNode.SelectSingleNode(".//div[contains(@class, 'price-tile__sales')]");
                if (salePriceNode != null)
                    salePrice = ParsePrice(salePriceNode.InnerText);
                

                // Parse old price
                var oldPriceNode = productNode.SelectSingleNode(".//div[contains(@class, 'price-tile__standard')]");
                if (oldPriceNode != null)
                    oldPrice = ParsePrice(oldPriceNode.InnerText);
                

                var descriptionNode = productNode.SelectSingleNode(".//span[contains(@class, 'product-omnibus-price__label')]");
                if (descriptionNode != null)
                    priceLabel = RemoveMultipleSpaces(descriptionNode.InnerText);
                

                if (name != "" && salePrice != decimal.MinValue)
                {
                    products.Add(new(name, category, salePrice, oldPrice, priceLabel, saleSpecification));
                }

            }

            return products;
        }

        private static string RemoveMultipleSpaces(string? input)
        {
            if (input == null)
                return string.Empty;

            return Regex.Replace(input, @"\s+", " ");

        }


        private static decimal ParsePrice(string priceText)
        {
            decimal price;
            string cleanedText = priceText.Replace("\n", "").Replace("zł", "").Trim();
            cleanedText = RemoveMultipleSpaces(cleanedText);
            cleanedText = cleanedText.Replace(" ", ".");
            decimal.TryParse(cleanedText, NumberStyles.Any, CultureInfo.InvariantCulture, out price);
            return price;

        }


    }
}

