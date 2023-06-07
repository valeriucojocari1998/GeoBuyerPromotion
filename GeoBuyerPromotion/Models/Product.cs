namespace GeoBuyerPromotion.Models
{
	public record Product(string name, string category, decimal salePrice, decimal oldPrice, string? priceLabel, string? saleSpecification)
	{
        public override string ToString()
        {
            return $"Name: {name}\n" + $"Category: {category}\n" + $"Sale Specification: {saleSpecification}\n" +  $"Sale Price: {salePrice}\n" + $"Old Price: {oldPrice}\n" + $"Price Label: {priceLabel}\n";
        }
    }
}

