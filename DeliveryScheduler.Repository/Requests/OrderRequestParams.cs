namespace DeliveryScheduler.Repository.Requests
{
    public class OrderRequestParams
    {
        public string PostalCode { get; set; }
        public List<int> ProductIds { get; set; }
    }
}
