﻿using GeoBuyerPromotion.Models;
using GeoBuyerPromotion.Parsers;
using GeoBuyerPromotion.Repositories;

namespace GeoBuyerPromotion.Services;

interface ISpotService
{
    IRepository Repository { get; }
    IParser Parser { get; }
    string Spot { get; }
    string SpotUrl { get; }
    public Task GetProducts();
    public Task<List<Category>> GetCategories();
    public Task<List<Product>> GetProductsByCategory(Category category);
}