namespace DeliveryScheduler.Repository.Repositories;

public interface IPeriod
{
    public DateTime GetOrderDate();
    public int GetOrderLength();
    public DaysOfWeek GetGreenDeliveryDates();
}

