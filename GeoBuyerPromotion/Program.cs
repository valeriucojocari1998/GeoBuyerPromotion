using GeoBuyerPromotion.Parsers;
using GeoBuyerPromotion.Services;
using GeoBuyerPromotion.Enums;
using GeoBuyerPromotion.Repositories;

IRepository repository = new Repository("", "");
/// <summary>
/// Initiate Biedronka Service and Get Date
/// </summary>
string BiedronkaUrl = "https://home.biedronka.pl/promocje";
IParser biedronkaParser = new BiedronkaParser();
ISpotService biedronkaService = new SpotService(repository, biedronkaParser, MarketProvider.Biedronka, BiedronkaUrl);
await biedronkaService.GetProducts();