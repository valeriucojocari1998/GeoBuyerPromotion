namespace GeoBuyerPromotion.Models;

public record Product(
    Guid id,
    string name,
    decimal currentPrice,
    decimal? oldPrice = null,
    string? category = null,
    string? brand = null,
    string? priceLabel = null,
    string? saleSpecification = null,
    string? imageUrl = null)
{
    public override string ToString()
    {
        return $"Name: {name}\n" + $"Category: {category}\n" + $"Sale Specification: {saleSpecification}\n" +  $"Price: {currentPrice}\n" + $"Old Price: {oldPrice}\n" + $"Price Label: {priceLabel}\n";
    }
}

