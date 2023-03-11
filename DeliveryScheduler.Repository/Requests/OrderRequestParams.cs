namespace DeliveryScheduler.Repository.Requests
{
    public class OrderRequestParams
    {
        public OrderRequestParams()
        {
        }
        public OrderRequestParams(string postalCode, IEnumerable<int> productIds)
        {
            PostalCode = postalCode;
            ProductIds = (List<int>)productIds;
        }
        public string PostalCode { get; set; }
        public List<int> ProductIds { get; set; }
    }
}
