namespace DeliveryScheduler.Repository.Entities;
public class Availability
{
    public string PostalCode { get; set; }
    public DateTime DeliveryDate { get; set; }
    public bool IsGreenDelivery { get; set; }
}
