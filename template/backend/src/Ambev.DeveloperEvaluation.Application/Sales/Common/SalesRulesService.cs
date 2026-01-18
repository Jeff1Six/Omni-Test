using System.ComponentModel.DataAnnotations;

public static class SalesRulesService
{
    public static decimal GetDiscountPercentage(int quantity)
    {
        if (quantity > 20)
            throw new ValidationException("Cannot sell above 20 identical items");

        if (quantity >= 10)
            return 0.20m;

        if (quantity >= 4)
            return 0.10m;

        return 0.00m;
    }
}
