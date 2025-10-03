using FluentAssertions;
using Funq;
using ServiceStack;
using NUnit.Framework;
using RaceDataApp.Loader.ServiceInterface;
using RaceDataApp.Loader.ServiceModel;

namespace RaceDataApp.Loader.Tests;

public class IntegrationTest
{
    const string BaseUri = "http://localhost:2000/";
    private readonly ServiceStackHost _appHost;

    class AppHost : AppSelfHostBase
    {
        public AppHost() : base(nameof(IntegrationTest), typeof(RaceDataLoaderService).Assembly) { }

        public override void Configure(Container container)
        {
        }
    }

    public IntegrationTest()
    {
        _appHost = new AppHost()
            .Init()
            .Start(BaseUri);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() => _appHost.Dispose();

    public IServiceClient CreateClient() => new JsonServiceClient(BaseUri);

    [Test]
    public void Can_call_SaveDriver_Service()
    {
        var client = CreateClient();

        var response = client.Post(new SaveDriver { Forename = "Valentin" });

        response.DriverId.Should().Be(123);
    }
}