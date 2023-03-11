namespace DeliveryScheduler.Repository.Repositories;

public class MockProduct : IProduct
{
    public IEnumerable<Product> GetProducts()
    {
        return new List<Product>
        {
            new() { ProductId = 1, Name = "LG TV", ProductType = ProductType.Normal, DeliveryDays = new List<DaysOfWeek>
            {
                DaysOfWeek.Monday, DaysOfWeek.Tuesday, DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday
            },DaysInAdvance = GetDaysInAdvance(ProductType.Normal) },

            new() { ProductId = 2, Name = "AC", ProductType = ProductType.External, DeliveryDays = new List<DaysOfWeek>
            {
                DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday
            }, DaysInAdvance = GetDaysInAdvance(ProductType.External)},

            new() { ProductId = 3, Name = "Dell Laptop", ProductType = ProductType.External, DeliveryDays = new List<DaysOfWeek>
            {
                DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday
            }, DaysInAdvance = GetDaysInAdvance(ProductType.External), },

            new() { ProductId = 4, Name = "LED Light", ProductType = ProductType.Temporary, DeliveryDays = new List<DaysOfWeek>
            {
                DaysOfWeek.Monday, DaysOfWeek.Tuesday, DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday
            }, DaysInAdvance = GetDaysInAdvance(ProductType.Temporary)},

            new() { ProductId = 5, Name = "Keyboard", ProductType = ProductType.Temporary,  DeliveryDays = new List<DaysOfWeek>
            {
                DaysOfWeek.Monday, DaysOfWeek.Tuesday, DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday
            }, DaysInAdvance = GetDaysInAdvance(ProductType.Temporary)},

            new() { ProductId = 6, Name = "iPhone", ProductType = ProductType.Normal, DeliveryDays = new List<DaysOfWeek>
            {
                DaysOfWeek.Monday, DaysOfWeek.Tuesday, DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday
            }, DaysInAdvance = GetDaysInAdvance(ProductType.Normal) }
        };
    }

    public int GetDaysInAdvance(ProductType productType)
    {
        switch (productType)
        {
            case ProductType.External:
                return 5;
            case ProductType.Temporary:
                return 1;
            case ProductType.Normal:
                return 2;
            default:
                throw new Exception("Invalid product type");
        }
    }
}
