namespace DeliveryScheduler.Repository.Entities;
public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public List<DaysOfWeek> DeliveryDays { get; set; } = null;
    public ProductType ProductType { get; set; }
    public int DaysInAdvance { get; set; }
}
