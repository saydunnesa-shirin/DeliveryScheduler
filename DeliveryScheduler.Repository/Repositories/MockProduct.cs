namespace DeliveryScheduler.Repository.Repositories;

public class MockProduct : IProduct
{
    public IEnumerable<Product> GetProducts()
    {
        return new List<Product>
        {
            new() { ProductId = 1, Name = "LG TV", ProductType = ProductType.Normal, DeliveryDays = new List<DaysOfWeek>
            {
                DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday
            },DaysInAdvance = 4 },

            new() { ProductId = 2, Name = "AC", ProductType = ProductType.External, DeliveryDays = new List<DaysOfWeek>
            {
                DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday
            }, DaysInAdvance = 5},

            new() { ProductId = 3, Name = "Dell Laptop", ProductType = ProductType.External, DeliveryDays = new List<DaysOfWeek>
            {
                DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday
            }, DaysInAdvance = 5, },

            new() { ProductId = 4, Name = "Dell Mouse", ProductType = ProductType.Temporary, DeliveryDays = new List<DaysOfWeek>
                {
                    DaysOfWeek.Monday, DaysOfWeek.Tuesday, DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday
                }, DaysInAdvance = 4},

            new() { ProductId = 5, Name = "Keyboard", ProductType = ProductType.Temporary,  DeliveryDays = new List<DaysOfWeek>
                {
                    DaysOfWeek.Monday, DaysOfWeek.Tuesday, DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday
                }, DaysInAdvance = 4},
            new() { ProductId = 6, Name = "iPhone", ProductType = ProductType.Normal, DaysInAdvance = 4 }
        };
    }
}
