namespace GeoBuyerPromotion.Models;

public record Category(string id, string name, string categoryUrl);

public record ExtendedCategory(string id, string name, string categoryUrl, string marketId, string marketProvider)
{
    public ExtendedCategory(Category category, Spot market)
        : this(category.id, category.name, category.categoryUrl, market.id, market.provider)
    { }
}

