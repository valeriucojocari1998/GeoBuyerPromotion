using GeoBuyerPromotion.Enums;
using GeoBuyerPromotion.Managers;
using GeoBuyerPromotion.Models;
using GeoBuyerPromotion.Parsers;
using GeoBuyerPromotion.Repositories;

namespace GeoBuyerPromotion.Services;

public record SpotService: ISpotService
{
    public IRepository Repository { get; }
    public IParser Parser { get; }
    public string Spot { get; }
    public string SpotUrl { get; }

    public SpotService(IRepository repository, IParser parser, string market, string marketUrl)
    {
        Repository = repository;
        Parser = parser;
        Spot = market;
        SpotUrl = marketUrl;
    }
    public async Task GetProducts()
    {
        var spot = Repository.GetSpotByName(Spot);
        var categories = await GetCategories();
        var productsLists = await Task.WhenAll(categories.Select(async category =>
        {
            var products = await GetProductsByCategory(category);
            return products.Select(product => new ExtendedProduct(product, category, spot));
        }));

        var extendedCategories = categories.Select(cat => new ExtendedCategory(cat, spot));
        var extendedProducts = productsLists.SelectMany(pr => pr);
        foreach (var product in extendedProducts)
        {
            Console.WriteLine(product + "\n");
        }
    }

    public async Task<List<Category>> GetCategories()
    {
        var data = await HtmlSourceManager.DownloadHtmlSourceCode(SpotUrl);
        return Parser.GetCategories(data);
    }

    public async Task<List<Product>> GetProductsByCategory(Category category)
    {
        var url = category.categoryUrl + "?start=0&sz=1000";
        var data = await HtmlSourceManager.DownloadHtmlSourceCode(url);
        return Parser.GetProductsByCategory(data, category.name);
    }
}
