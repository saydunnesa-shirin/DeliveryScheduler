using DeliveryScheduler.Repository.Entities;
using Microsoft.AspNetCore.Mvc;
using DeliveryScheduler.Repository.Requests;
using DeliveryScheduler.Service.Services;

namespace DeliveryScheduler.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class SchedulerController : ControllerBase
{
    private readonly ILogger<SchedulerController> _logger;
    private readonly IScheduler _scheduler;

    public SchedulerController(ILogger<SchedulerController> logger, IScheduler scheduler)
    {
        _logger = logger;
        _scheduler = scheduler;
    }

    [HttpPost(Name = "GetAvailableDeliveryDates")]
    public IEnumerable<Availability> GetAvailableDeliveryDates(OrderRequestParams input)
    {
        try
        {
            var result = _scheduler.GetAvailableDeliveryDates(input);

            if (string.IsNullOrEmpty(result.errMessage))
                return result.availabilities;

            _logger.LogError(result.errMessage);
            return new List<Availability>();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }

        return new List<Availability>();
    }
}
