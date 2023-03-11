using DeliveryScheduler.Repository.Helpers;

namespace DeliveryScheduler.Service.Tests;
public class SchedulerTests
{
    private readonly Mock<IProduct> _productRepository;
    private readonly Mock<ICustomer> _customerRepository;
    private readonly Mock<IPeriod> _periodRepository;
    private readonly IScheduler _schedulerService;

    public SchedulerTests()
    {
        _productRepository = new Mock<IProduct>();
        _customerRepository = new Mock<ICustomer>();
        _periodRepository = new Mock<IPeriod>();
        _schedulerService = new Scheduler(_customerRepository.Object, _productRepository.Object, _periodRepository.Object);
    }

    [SetUp]
    public void Setup()
    {
        _productRepository.Reset();
        _customerRepository.Reset();
        _periodRepository.Reset();
    }

    [Test]
    public void GetAvailableDeliveryDates_Pass()
    {
        //Arrange
        var command = SetUpGetAvailableDeliveryDatesRequest();
        var newMockProduct = new MockProduct();
        _productRepository.Setup(x => x.GetProducts()).Returns(newMockProduct.GetProducts);

        //Act
        var result = _schedulerService.GetAvailableDeliveryDates(command);

        //Arrange
        Assert.IsEmpty(result.errMessage);
        _productRepository.Verify(x => x.GetProducts(), Times.Once);
        _productRepository.VerifyNoOtherCalls();
    }

    [Test]
    public void GetAvailableDeliveryDates_NoProducts_Fail()
    {
        //Arrange
        var command = SetUpGetAvailableDeliveryDatesRequest();
        command.Products = null;
        var newMockProduct = new MockProduct();
        _productRepository.Setup(x => x.GetProducts()).Returns(newMockProduct.GetProducts);

        //Act
        var result = _schedulerService.GetAvailableDeliveryDates(command);

        //Arrange
        Assert.AreEqual(result.errMessage, "Invalid product list");
    }

    [Test]
    public void GetAvailableDeliveryDates_HasGreenDelivery_Pass()
    {
        //Arrange

        var products = new List<Product>
        {
            new()
            {
                ProductId = 2, Name = "LG TV", ProductType = ProductType.Normal, DeliveryDays = new List<DaysOfWeek>
                {
                    DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday, DaysOfWeek.Monday, DaysOfWeek.Tuesday, DaysOfWeek.Wednesday
                },
                DaysInAdvance = 2
            }
        };

        var command = SetUpGetAvailableDeliveryDatesRequest(products);

        var newMockProduct = new MockProduct();
        _productRepository.Setup(x => x.GetProducts()).Returns(products);

        _periodRepository.Setup(x => x.GetOrderDate()).Returns(new DateTime(2023, 03, 10));
        _periodRepository.Setup(x => x.GetOrderLength()).Returns(10);

        //Act
        var result = _schedulerService.GetAvailableDeliveryDates(command);

        //Arrange
        Assert.IsEmpty(result.errMessage);
        Assert.NotNull(result.availabilities);
        Assert.AreEqual(8, result.availabilities.Count());
        Assert.IsNotEmpty(result.availabilities.Where(a => a.IsGreenDelivery));
    }

    [Test]
    public void GetAvailableDeliveryDates_NoGreenDelivery_Pass()
    {
        //Arrange

        var products = new List<Product>
        {
            new()
            {
                ProductId = 2, Name = "LG TV", ProductType = ProductType.Normal, DeliveryDays = new List<DaysOfWeek>
                {
                   DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday, DaysOfWeek.Monday, DaysOfWeek.Tuesday
                },
                DaysInAdvance = 2
            }
        };

        var command = SetUpGetAvailableDeliveryDatesRequest(products);
        
        var newMockProduct = new MockProduct();
        _productRepository.Setup(x => x.GetProducts()).Returns(products);

        _periodRepository.Setup(x => x.GetOrderDate()).Returns(new DateTime(2023, 03, 10));
        _periodRepository.Setup(x => x.GetOrderLength()).Returns(4);

        //Act
        var result = _schedulerService.GetAvailableDeliveryDates(command);

        //Arrange
        Assert.IsEmpty(result.errMessage);
        Assert.NotNull(result.availabilities);
        Assert.AreEqual(3, result.availabilities.Count());
        Assert.IsEmpty(result.availabilities.Where(a=>a.IsGreenDelivery));
    }

    private static AvailiabilitySearch SetUpGetAvailableDeliveryDatesRequest(IEnumerable<Product> productList = null)
    {
        var postalCode = "1258";
        var products = new List<Product>();
        
        if (productList.NotNullAndAny())
        {
            products = productList.ToList();
            return new AvailiabilitySearch(postalCode, products);
        }

        products = new List<Product>
        {
            new()
            {
                ProductId = 1, Name = "LG TV", ProductType = ProductType.Normal, DeliveryDays = new List<DaysOfWeek>
                {
                    DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday
                },
                DaysInAdvance = 4
            },

            new()
            {
                ProductId = 2, Name = "AC", ProductType = ProductType.External, DeliveryDays = new List<DaysOfWeek>
                {
                    DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday
                },
                DaysInAdvance = 5
            },

            new()
            {
                ProductId = 3, Name = "Dell Laptop", ProductType = ProductType.External, DeliveryDays =
                    new List<DaysOfWeek>
                    {
                        DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday,
                        DaysOfWeek.Sunday
                    },
                DaysInAdvance = 5,
            }
        };
        return new AvailiabilitySearch(postalCode, products);
    }
}
