using FluentAssertions;
using Funq;
using Moq;
using ServiceStack;
using NUnit.Framework;
using RaceDataApp.Reader.Domain.Entities;
using RaceDataApp.Reader.ServiceInterface;
using RaceDataApp.Reader.ServiceModel;

namespace RaceDataApp.Reader.Tests;

public class IntegrationTest
{
    const string BaseUri = "http://localhost:2000/";
    private readonly ServiceStackHost _appHost;
    private static readonly List<CircuitSummary> ExpectedSummaries = [new() { CircuitId = 123 }];
    
    class AppHost : AppSelfHostBase
    {
        private readonly Mock<ICommandExecutor> _commandExecutorMock = new();
        private readonly Mock<IAsyncCommand<CircuitSummaryRequest, List<CircuitSummary>>> _circuitSummaryCommandMock = new();
        
        public AppHost() : base(nameof(IntegrationTest), typeof(RaceDataQueryService).Assembly) { }

        public override void Configure(Container container)
        {
            _circuitSummaryCommandMock.Setup(x => x.Result)
                .Returns(ExpectedSummaries);
            _commandExecutorMock.Setup(x => x.Command<IAsyncCommand<CircuitSummaryRequest, List<CircuitSummary>>>())
                .Returns(_circuitSummaryCommandMock.Object);
            container.AddSingleton(_commandExecutorMock.Object);
            container.AddTransient<RaceDataQueryService>();
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
    public void Can_call_CircuitSummaryRequest_Service()
    {
        var client = CreateClient();

        var response = client.Get(new CircuitSummaryRequest());

        response.Should().BeEquivalentTo(ExpectedSummaries);
    }
}