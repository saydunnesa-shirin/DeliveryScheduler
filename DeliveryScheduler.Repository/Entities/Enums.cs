namespace DeliveryScheduler.Repository.Entities;

public enum ProductType
{
    Normal = 1,
    External,
    Temporary
}

public enum GreenDeliveryDates
{
    Weekly = DaysOfWeek.Wednesday
}

public enum DaysOfWeek
{
    Monday = 1,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}

