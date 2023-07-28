using System.Globalization;
using System.Text.RegularExpressions;
using GeoBuyerPromotion.Models;
using HtmlAgilityPack;

namespace GeoBuyerPromotion.Parsers
{
    public record BiedronkaParser : IParser
    {
        public List<Category> GetCategories(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var categories = GetCategoriesInternal(doc);
            return categories;
        }

        private static List<Category> GetCategoriesInternal(HtmlDocument document)
        {
            var categoryNodes = document.DocumentNode.SelectNodes("//li[contains(@class, 'refinement-category__list')]");
            if (categoryNodes == null)
                return new List<Category>();

            return categoryNodes.Select(node =>
            {
                string name = "";
                string categoryUrl = "";

                // gets the node of the category
                var categoryNode = node.SelectSingleNode(".//a[contains(@class, 'refinement-category__link')]");
                if (categoryNode != null)
                {
                    name = RemoveMultipleSpaces(categoryNode.InnerHtml);
                    categoryUrl = RemoveMultipleSpaces(categoryNode.Attributes.FirstOrDefault(x => x.Name == "href")?.Value);
                    Console.WriteLine(categoryNode.GetAttributes());
                }

                return new Category(Guid.NewGuid(), name, categoryUrl);
            }).ToList();
        }

        public List<Product> GetProductsByCategory(string html, string categoryName)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var products = GetProductsInternalByCategory(doc, categoryName);
            return products;
        }


        private static List<Product> GetProductsInternalByCategory(HtmlDocument document, string categoryName)
        {
            var products = new List<Product>();

            var productNodes = document.DocumentNode.SelectNodes("//div[contains(@class, 'product-tile js-product-tile')]");
            if (productNodes == null)
                return products;

            return productNodes.Select(productNode =>
            {
                string name = "";
                string brand = "";
                string saleSpecification = "";
                decimal currentPrice = decimal.MinValue;
                decimal? oldPrice = null;
                string priceLabel = "";
                string imageUrl = "";
                // Parse name
                var nameNode = productNode.SelectSingleNode(".//div[contains(@class, 'product-tile__name product-tile__name--overflow')]");
                if (nameNode != null)
                    name = RemoveMultipleSpaces(nameNode.InnerText);

                var brandNode = productNode.SelectSingleNode(".//div[contains(@class, 'product-tile__name')]");
                if (brandNode != null)
                    brand = RemoveMultipleSpaces(brandNode.InnerText);

                var saleSpecificationNode = productNode.SelectSingleNode(".//div[contains(@class, 'badge-tile__item\n badge-tile__item--text')]");
                if (saleSpecificationNode != null)
                    saleSpecification = RemoveMultipleSpaces(saleSpecificationNode.InnerText);

                // Parse sale price
                var currentPriceNode = productNode.SelectSingleNode(".//div[contains(@class, 'price-tile__sales')]");
                if (currentPriceNode != null)
                    currentPrice = ParsePrice(currentPriceNode.InnerText);


                // Parse old price
                var oldPriceNode = productNode.SelectSingleNode(".//div[contains(@class, 'price-tile__standard')]");
                if (oldPriceNode != null)
                    oldPrice = ParsePrice(oldPriceNode.InnerText);


                var descriptionNode = productNode.SelectSingleNode(".//span[contains(@class, 'product-omnibus-price__label')]");
                if (descriptionNode != null)
                    priceLabel = RemoveMultipleSpaces(descriptionNode.InnerText);

                var imageNode = productNode.SelectSingleNode(".//picture[contains(@class, 'tile-image__container')]");
                if (imageNode != null)
                {
                    var pictureNode = imageNode.SelectSingleNode(".//img");
                    imageUrl = RemoveMultipleSpaces(pictureNode.Attributes.FirstOrDefault(x => x.Name == "srcset")?.Value);

                }

                return new Product(
                    id: Guid.NewGuid(),
                    name: name,
                    currentPrice: currentPrice,
                    oldPrice: oldPrice,
                    category: categoryName,
                    brand: brand,
                    priceLabel: priceLabel,
                    saleSpecification: saleSpecification,
                    imageUrl: imageUrl);
            }).ToList();
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