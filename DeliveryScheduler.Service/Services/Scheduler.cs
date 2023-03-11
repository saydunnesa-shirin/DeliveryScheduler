using DeliveryScheduler.Repository.Entities;
using DeliveryScheduler.Repository.Repositories;
using DeliveryScheduler.Repository.Requests;
using System.Globalization;
using DeliveryScheduler.Repository.Helpers;

namespace DeliveryScheduler.Service.Services;
public class Scheduler : IScheduler
{
    private readonly ICustomer _customer;
    private readonly IProduct _product;
    private readonly IPeriod _period;
    public Scheduler(ICustomer customer, IProduct product, IPeriod period)
    {
        _customer = customer;
        _product = product;
        _period = period;
    }
    public (IEnumerable<Availability> availabilities, string errMessage) GetAvailableDeliveryDates(OrderRequestParams input)
    {
        try
        {
            var errorMessage = "";
            //Get existing Products
            var productList = GetProducts();
            //Validate Input
            var validationResult = ValidateInput(input, productList);

            if (!validationResult.isValid)
            {
                errorMessage = validationResult.errMessage;
                return (new List<Availability>(), errorMessage);
            }

            productList = (from e in productList where input.ProductIds.Contains(e.ProductId) select e).ToList();

            if (!productList.NotNullAndAny())
            {
                errorMessage = "Invalid product list";
                return (new List<Availability>(), errorMessage);
            }

            var result = new List<Availability>();
            var orderDate = _period.GetOrderDate();
            var weekNo = GetWeekOfYear(orderDate.Date);
            var lastDate = orderDate.AddDays(_period.GetOrderLength());

            while (orderDate <= lastDate)
            {
                var productDic = new Dictionary<int, bool>();
                var day = GetDay(orderDate);

                foreach (var product in productList)
                {
                    var deliverable = true;
                    //Check for DeliveryDays
                    if (product.DeliveryDays.NotNullAndAny())
                        deliverable = product.DeliveryDays.Contains(day);

                    if(deliverable)
                        deliverable = orderDate.AddDays(product.DaysInAdvance) <= lastDate; //Check for DaysInAdvance

                    if (product.ProductType == ProductType.Temporary && deliverable)
                    {
                        deliverable = GetWeekOfYear(orderDate.Date) == weekNo; //Check for Temporary product week number
                    }

                    productDic.Add(product.ProductId, deliverable);
                }

                if (productDic.Values.All(q => q.Equals(true)))
                {
                    result.Add(new Availability
                    {
                        PostalCode = input.PostalCode,
                        DeliveryDate = orderDate,
                        IsGreenDelivery = day == (DaysOfWeek)GreenDeliveryDates.Weekly
                    });
                }
                orderDate = orderDate.AddDays(1);
            }

            if (!result.NotNullAndAny()) return (result, errorMessage);

            result = result.OrderBy(o => o.DeliveryDate).ToList();

            if (result.Any(o => o.IsGreenDelivery) && result.Any(d => d.DeliveryDate < DateTime.Now.AddDays(3)))
            {
                result = result.OrderByDescending(o => o.IsGreenDelivery).ToList();
            }

            return (result, errorMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private IEnumerable<Customer> GetCustomers()
    {
        return _customer.GetCustomers();
    }

    private IEnumerable<Product> GetProducts()
    {
        return _product.GetProducts();
    }

    private static (bool isValid, string errMessage) ValidateInput(OrderRequestParams input, IEnumerable<Product> productList)
    {
        var errorMessage = "";
        if (string.IsNullOrEmpty(input.PostalCode))
        {
            errorMessage = "Invalid postal code";
            return (false, errorMessage);
        }

        if (!input.ProductIds.NotNullAndAny())
        {
            errorMessage = "Invalid requested product list";
            return (false, errorMessage);
        }

        if (!productList.NotNullAndAny())
        {
            errorMessage = "Invalid product list";
            return (false, errorMessage);
        }
        return (true, errorMessage);
    }

    private static int GetWeekOfYear(DateTime time)
    {
        var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
        if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
        {
            time = time.AddDays(3);
        }

        // Return the week of our adjusted day
        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }

    private static DaysOfWeek GetDay(DateTime orderDate)
    {
        var day = (DaysOfWeek)(orderDate.DayOfWeek == DayOfWeek.Sunday
            ? 7
            : (int)orderDate.DayOfWeek);

        return day;
    }
}
