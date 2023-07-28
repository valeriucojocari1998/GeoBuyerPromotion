using GeoBuyerPromotion.Managers;
using GeoBuyerPromotion.Models;
using GeoBuyerPromotion.Parsers;
using Newtonsoft.Json;

namespace GeoBuyerPromotion
{
    internal class Program
    {
        private static readonly string Biedronka = "https://home.biedronka.pl/promocje";
        private static BiedronkaParser bedrionkaParser = new BiedronkaParser();


        static async Task Main(string[] args)
        {
            await GetBedrionkaProducts();
        }

        private static async Task GetBedrionkaProducts()
        {
            var categories = await GetBedrionkaCategories();
            var tasks = categories.Select(cat => GetBedrionkaProductsByCategory(cat));
            var productsLists = await Task.WhenAll(tasks);
            var products = productsLists.SelectMany(pr => pr).ToList();
            products.ForEach(product => Console.WriteLine(product + "\n"));
        }

        private static async Task<List<Category>> GetBedrionkaCategories()
        {
            var data = await HtmlSourceManager.DownloadHtmlSourceCode(Biedronka);
            return bedrionkaParser.GetCategories(data);
        }

        private static async Task<List<Product>> GetBedrionkaProductsByCategory(Category category)
        {
            var url = category.categoryUrl + "?start=0&sz=1000";
            var data = await HtmlSourceManager.DownloadHtmlSourceCode(url);
            return bedrionkaParser.GetProductsByCategory(data, category.name);
        }
    }
}
