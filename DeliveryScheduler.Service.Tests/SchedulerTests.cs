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
        command.ProductIds = null;
        var newMockProduct = new MockProduct();
        _productRepository.Setup(x => x.GetProducts()).Returns(newMockProduct.GetProducts);

        //Act
        var result = _schedulerService.GetAvailableDeliveryDates(command);

        //Arrange
        Assert.AreEqual(result.errMessage, "Invalid requested product list");
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

        var command = SetUpGetAvailableDeliveryDatesRequest(new List<int> { 1, 2 });

        var newMockProduct = new MockProduct();
        _productRepository.Setup(x => x.GetProducts()).Returns(products);

        _periodRepository.Setup(x => x.GetOrderDate()).Returns(new DateTime(2023, 03, 10));
        _periodRepository.Setup(x => x.GetOrderLength()).Returns(10);
        _periodRepository.Setup(x => x.GetGreenDeliveryDates()).Returns(DaysOfWeek.Wednesday);

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

        var command = SetUpGetAvailableDeliveryDatesRequest(new List<int> { 1, 2 });
        
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

    [Test]
    public void GetAvailableDeliveryDates_HasTwoGreenDelivery()
    {
        //Arrange

        var products = new List<Product>
        {
            new()
            {
                ProductId = 1, Name = "LG TV", ProductType = ProductType.Normal, DeliveryDays = new List<DaysOfWeek>
                {
                    DaysOfWeek.Monday, DaysOfWeek.Tuesday, DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday
                },
                DaysInAdvance = 2
            },
            new()
            {
                ProductId = 2, Name = "DELL Laptop", ProductType = ProductType.Normal, DeliveryDays = new List<DaysOfWeek>
                {
                    DaysOfWeek.Monday, DaysOfWeek.Tuesday, DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday
                },
                DaysInAdvance = 1
            }
        };

        var command = SetUpGetAvailableDeliveryDatesRequest(new List<int>{1,2});

        var newMockProduct = new MockProduct();
        _productRepository.Setup(x => x.GetProducts()).Returns(products);

        _periodRepository.Setup(x => x.GetOrderDate()).Returns(new DateTime(2023, 03, 11));
        _periodRepository.Setup(x => x.GetOrderLength()).Returns(14);
        _periodRepository.Setup(x => x.GetGreenDeliveryDates()).Returns(DaysOfWeek.Monday);

        //Act
        var result = _schedulerService.GetAvailableDeliveryDates(command);

        //Arrange
        Assert.IsEmpty(result.errMessage);
        Assert.IsNotEmpty(result.availabilities);
        Assert.AreEqual(13, result.availabilities.Count());
        Assert.AreEqual(2, result.availabilities.Where(d => d.IsGreenDelivery).Count());
        Assert.AreEqual(true, result.availabilities.First().IsGreenDelivery);
    }

    [Test]
    public void GetAvailableDeliveryDates_DeliveryDaysNotMatch_Pass()
    {
        //Arrange

        var products = new List<Product>
        {
            new()
            {
                ProductId = 2, Name = "LG TV", ProductType = ProductType.Normal, DeliveryDays = new List<DaysOfWeek>
                {
                    DaysOfWeek.Monday, DaysOfWeek.Tuesday, DaysOfWeek.Wednesday
                },
                DaysInAdvance = 1
            }
        };

        var command = SetUpGetAvailableDeliveryDatesRequest(new List<int> { 2 });

        var newMockProduct = new MockProduct();
        _productRepository.Setup(x => x.GetProducts()).Returns(products);

        _periodRepository.Setup(x => x.GetOrderDate()).Returns(new DateTime(2023, 03, 09));
        _periodRepository.Setup(x => x.GetOrderLength()).Returns(4);

        //Act
        var result = _schedulerService.GetAvailableDeliveryDates(command);

        //Arrange
        Assert.IsEmpty(result.errMessage);
        Assert.IsEmpty(result.availabilities);
    }

    [Test]
    public void GetAvailableDeliveryDates_CheckForDaysInAdvance()
    {
        //Arrange

        var products = new List<Product>
        {
            new()
            {
                ProductId = 1, Name = "LG TV", ProductType = ProductType.Normal, DeliveryDays = new List<DaysOfWeek>
                {
                    DaysOfWeek.Monday, DaysOfWeek.Tuesday, DaysOfWeek.Wednesday
                },
                DaysInAdvance = 2
            },
            new()
            {
                ProductId = 2, Name = "DELL Laptop", ProductType = ProductType.External, DeliveryDays = new List<DaysOfWeek>
                {
                    DaysOfWeek.Monday, DaysOfWeek.Tuesday, DaysOfWeek.Wednesday
                },
                DaysInAdvance = 5
            }
        };

        var command = SetUpGetAvailableDeliveryDatesRequest(new List<int> { 1, 2 });

        var newMockProduct = new MockProduct();
        _productRepository.Setup(x => x.GetProducts()).Returns(products);

        _periodRepository.Setup(x => x.GetOrderDate()).Returns(new DateTime(2023, 03, 06));
        _periodRepository.Setup(x => x.GetOrderLength()).Returns(5);

        //Act
        var result = _schedulerService.GetAvailableDeliveryDates(command);

        //Arrange
        Assert.IsEmpty(result.errMessage);
        Assert.IsNotEmpty(result.availabilities);
        Assert.AreEqual(1, result.availabilities.Count());
    }

    [Test]
    public void GetAvailableDeliveryDates_CheckForTemporaryProductDeliverableWeek()
    {
        //Arrange

        var products = new List<Product>
        {
            new()
            {
                ProductId = 1, Name = "LG TV", ProductType = ProductType.Normal, DeliveryDays = new List<DaysOfWeek>
                {
                    DaysOfWeek.Monday, DaysOfWeek.Tuesday, DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday
                },
                DaysInAdvance = 2
            },
            new()
            {
                ProductId = 2, Name = "DELL Laptop", ProductType = ProductType.Temporary, DeliveryDays = new List<DaysOfWeek>
                {
                    DaysOfWeek.Monday, DaysOfWeek.Tuesday, DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday, DaysOfWeek.Sunday
                },
                DaysInAdvance = 1
            }
        };

        var command = SetUpGetAvailableDeliveryDatesRequest(new List<int> { 1, 2 });

        var newMockProduct = new MockProduct();
        _productRepository.Setup(x => x.GetProducts()).Returns(products);

        _periodRepository.Setup(x => x.GetOrderDate()).Returns(new DateTime(2023, 03, 07));
        _periodRepository.Setup(x => x.GetOrderLength()).Returns(14);

        //Act
        var result = _schedulerService.GetAvailableDeliveryDates(command);

        //Arrange
        Assert.IsEmpty(result.errMessage);
        Assert.IsNotEmpty(result.availabilities);
        Assert.AreEqual(6, result.availabilities.Count());
    }

    private static OrderRequestParams SetUpGetAvailableDeliveryDatesRequest(IEnumerable<int> productList = null)
    {
        var postalCode = "1258";
        var products = new List<int> {1, 2, 3, 4, 5};

        if (!productList.NotNullAndAny()) return new OrderRequestParams(postalCode, products);
        products = productList.ToList();
        return new OrderRequestParams(postalCode, products);
    }
}
