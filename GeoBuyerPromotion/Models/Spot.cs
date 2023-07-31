namespace GeoBuyerPromotion.Models
{
    public record Spot(string id, string name, string? branchName = null, string? latitude = null, string? longitude = null);
}
