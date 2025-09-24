using Funq;
using ServiceStack;
using NUnit.Framework;
using RaceDataApp.Reader.ServiceInterface;
using RaceDataApp.Reader.ServiceModel;

namespace RaceDataApp.Reader.Tests;

public class IntegrationTest
{
    const string BaseUri = "http://localhost:2000/";
    private readonly ServiceStackHost appHost;

    class AppHost : AppSelfHostBase
    {
        public AppHost() : base(nameof(IntegrationTest), typeof(RaceDataQueryService).Assembly) { }

        public override void Configure(Container container)
        {
        }
    }

    public IntegrationTest()
    {
        appHost = new AppHost()
            .Init()
            .Start(BaseUri);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() => appHost.Dispose();

    public IServiceClient CreateClient() => new JsonServiceClient(BaseUri);

    [Test]
    public void Can_call_CircuitSummaryRequest_Service()
    {
        var client = CreateClient();

        var response = client.Get(new CircuitSummaryRequest());

        //Assert.That(response.Result, Is.EqualTo("Hello, World!"));
    }
}