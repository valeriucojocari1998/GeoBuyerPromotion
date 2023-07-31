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
    public string SpotUrlAddition { get; }

    public SpotService(IRepository repository, IParser parser, string market, string marketUrl, string spotUrlAddition)
    {
        Repository = repository;
        Parser = parser;
        Spot = market;
        SpotUrl = marketUrl;
        SpotUrlAddition = spotUrlAddition;
    }
    public async Task GetProducts()
    {
        var spot = Repository.GetSpotByProvider(Spot);
        var categories = await GetCategories();
        var productsLists = await Task.WhenAll(categories.Select(async category =>
        {
            var products = await GetProductsByCategory(category);
            return products.Select(product => new ExtendedProduct(product, category, spot));
        }));

        var extendedCategories = categories.Select(cat => new ExtendedCategory(cat, spot));
        var extendedProducts = productsLists.SelectMany(pr => pr);
    }

    public async Task<List<Category>> GetCategories()
    {
        var data = await HtmlSourceManager.DownloadHtmlSourceCode(SpotUrl);
        return Parser.GetCategories(data);
    }

    public async Task<List<Product>> GetProductsByCategory(Category category)
    {
        var url = category.categoryUrl + SpotUrlAddition;
        var data = await HtmlSourceManager.DownloadHtmlSourceCode(url);
        return Parser.GetProductsByCategory(data, category.name);
    }
}
