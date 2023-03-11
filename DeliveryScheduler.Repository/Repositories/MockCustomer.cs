namespace DeliveryScheduler.Repository.Repositories;

public class MockCustomer : ICustomer
{
    public IEnumerable<Customer> GetCustomers()
    {
        return new List<Customer>
        {
            new() { Id = 1, Name = "Karim", Address = "Flat-15, House-10, Road-4, Tallinn, Estonia", PostalCode = "1024" },
            new() { Id = 2, Name = "Reza", Address = "Flat-12, House-10, Road-5, Tallinn, Estonia", PostalCode = "1024" },
            new() { Id = 3, Name = "Irem", Address = "Flat-1, House-12, Road-45, Tallinn, Estonia", PostalCode = "1025" },
            new() { Id = 4, Name = "Shirin", Address = "Flat-10, House-13, Road-4, Tallinn, Estonia", PostalCode = "1026" },
            new() { Id = 5, Name = "Anju", Address = "Flat-5, House-10, Road-8, Tallinn, Estonia", PostalCode = "1027" }
        };
    }
}
