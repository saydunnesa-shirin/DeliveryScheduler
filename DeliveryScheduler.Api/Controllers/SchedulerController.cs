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
    public IEnumerable<Availability> GetAvailableDeliveryDates(string postalCode, IEnumerable<Product> products)
    {
        var input = new AvailiabilitySearch(postalCode, products);
        var response = _scheduler.GetAvailableDeliveryDates(input);

        if (string.IsNullOrEmpty(response.errMessage)) 
            return response.availabilities;

        _logger.LogError(response.errMessage);
        return new List<Availability>();

    }
}
