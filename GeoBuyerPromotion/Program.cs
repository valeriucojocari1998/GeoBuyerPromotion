﻿using GeoBuyerPromotion.Enums;
using GeoBuyerPromotion.Parsers;
using GeoBuyerPromotion.Repositories;
using GeoBuyerPromotion.Services;

IRepository repository = new Repository("", "");
/// <summary>
/// Initiate Biedronka Service and Get Data
/// </summary>
string biedronkaUrl = "https://home.biedronka.pl/promocje";
IParser biedronkaParser = new BiedronkaParser();
ISpotService biedronkaService = new SpotService(repository, biedronkaParser, SpotProvider.Biedronka, biedronkaUrl, "?start=0&sz=1000");
await biedronkaService.GetProducts();

/// <summary>
/// Initiate Kaufland Service and Get Data
/// </summary>
string kauflandUrl = "https://www.kaufland.pl/oferta/aktualny-tydzien/przeglad.category=01_Mi%C4%99so__Dr%C3%B3b__W%C4%99dliny.html";
IParser kauflandParser = new KauflandParser();
ISpotService kauflandService = new SpotService(repository, kauflandParser, SpotProvider.Kaufland, kauflandUrl, "");
await kauflandService.GetProducts();

/// <summary>
/// Initiate Lidl Service and Get Data
/// </summary>
string lidlUrl = "https://www.lidl.pl/q/query/wyprzedaz";
IParser lidlParser = new LidlParser();
ISpotService lidlService = new SpotService(repository, lidlParser, SpotProvider.Lidl, lidlUrl, "");
await lidlService.GetProducts();

/// <summary>
/// Initiate Spar Service and Get Data
/// </summary>
string sparUrl = "https://e-spar.com.pl/";
IParser sparParser = new SparParser();
ISpotService sparService = new SpotService(repository, sparParser, SpotProvider.Spar, sparUrl, "");
await sparService.GetProducts();