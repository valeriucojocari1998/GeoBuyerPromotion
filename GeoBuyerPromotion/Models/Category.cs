using System;
namespace GeoBuyerPromotion.Models;

public record Category(Guid id, string name, string categoryUrl)
{
    public override string ToString()
    {
        return $"Category Name: {name}\n" + $"Url: {categoryUrl} \n";
    }
}

