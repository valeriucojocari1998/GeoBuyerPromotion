using GeoBuyerPromotion.Managers;
using GeoBuyerPromotion.Parsers;
using Newtonsoft.Json;

namespace GeoBuyerPromotion
{
    internal class Program
    {
        private static readonly string Biedronka = "https://home.biedronka.pl/promocje/?start=0&sz=1000";

        static async Task Main(string[] args)
        {
            await GetBedrionkaProducts();
        }

        private static async Task GetBedrionkaProducts()
        {
            var data = await HtmlSourceManager.DownloadHtmlSourceCode(Biedronka);
            var parser = new BiedronkaParser();
            var products = parser.GetProducts(data);
            var json = JsonConvert.SerializeObject(products);
            products.ForEach(product => Console.WriteLine(product + "\n"));
        }
    }
}
