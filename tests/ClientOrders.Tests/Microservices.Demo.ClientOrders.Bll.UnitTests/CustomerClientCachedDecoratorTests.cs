namespace Microservices.Demo.ClientOrders.Bll.UnitTests;

public class CustomerClientCachedDecoratorTests
{
    [Fact]
    public void Query_Should_ReturnCustomerFromCache_When_CacheHasCustomerId()
    {

    }

    [Fact]
    public void Query_Should_ReturnCustomerFromClient_When_CacheNotHaveCustomerId()
    {

    }

    [Fact]
    public void Query_Should_ThrowCustomerNotFound_WhenCacheAndClientNotHaveCustomerId()
    {

    }

    [Fact]
    public void Query_Should_PutCustomerIntoCache_WhenCacheNotHaveCustomer_AndClientHas()
    { }
}
