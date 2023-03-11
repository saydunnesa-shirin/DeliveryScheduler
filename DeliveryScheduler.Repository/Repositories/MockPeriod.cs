namespace DeliveryScheduler.Repository.Repositories;

public class MockPeriod : IPeriod
{
    private const int HowManyDays = 14;
    public DateTime GetOrderDate()
    {
        return DateTime.Now.Date;
    }

    public int GetOrderLength()
    {
        return HowManyDays;
    }

    public DaysOfWeek GetGreenDeliveryDates()
    {
        return DaysOfWeek.Monday;
    }
}
