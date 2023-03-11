namespace DeliveryScheduler.Repository.Requests
{
    public class AvailiabilitySearch
    {
        public AvailiabilitySearch(string postalCode, IEnumerable<Product> products)
        {
            PostalCode = postalCode;
            Products = (List<Product>?)products;
        }
        public string PostalCode { get; set; }
        public List<Product>? Products { get; set; }
    }
}
