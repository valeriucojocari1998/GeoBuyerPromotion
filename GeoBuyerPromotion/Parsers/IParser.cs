using GeoBuyerPromotion.Models;

namespace GeoBuyerPromotion.Parsers
{
	public interface IParser
	{
		public List<Category> GetCategories(string file);
		public List<Product> GetProductsByCategory(string file, string categoryName);
	}
}

