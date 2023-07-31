namespace GeoBuyerPromotion.Models;

public record Category(string id, string name, string categoryUrl);

public record ExtendedCategory(string id, string name, string marketId, string marketProvider, string categoryUrl)
{
    public ExtendedCategory(Category category, Spot market)
        : this(category.id, category.name, market.id, market.provider, category.categoryUrl)
    { }
}

