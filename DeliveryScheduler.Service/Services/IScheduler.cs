using DeliveryScheduler.Repository.Entities;
using DeliveryScheduler.Repository.Requests;

namespace DeliveryScheduler.Service.Services;
public interface IScheduler
{
    public (IEnumerable<Availability> availabilities, string errMessage) GetAvailableDeliveryDates(AvailiabilitySearch input);
}
