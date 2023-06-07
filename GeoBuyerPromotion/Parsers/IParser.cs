using GeoBuyerPromotion.Models;

namespace GeoBuyerPromotion.Parsers
{
	public interface IParser
	{
		public List<Product> GetProducts(string file);
	}
}

