using FluentAssertions;
using Moq;
using NUnit.Framework;
using RaceDataApp.Reader.Domain.Entities;
using ServiceStack;
using ServiceStack.Testing;
using RaceDataApp.Reader.ServiceInterface;
using RaceDataApp.Reader.ServiceModel;

namespace RaceDataApp.Reader.Tests;

public class UnitTest
{
    private readonly ServiceStackHost _appHost;
    private readonly Mock<ICommandExecutor> _commandExecutorMock = new();
    private readonly Mock<IAsyncCommand<CircuitSummaryRequest, List<CircuitSummary>>> _circuitSummaryCommandMock = new();
    private readonly Mock<IAsyncCommand<DriverSummaryRequest, List<DriverSummary>>> _driverSummaryCommandMock = new();
    private readonly List<CircuitSummary> _expectedSummaries = [new() { CircuitId = 123 }];
    
    public UnitTest()
    {
        _appHost = new BasicAppHost().Init();
        _circuitSummaryCommandMock.Setup(x => x.Result)
            .Returns(_expectedSummaries);
        _commandExecutorMock.Setup(x => x.Command<IAsyncCommand<CircuitSummaryRequest, List<CircuitSummary>>>())
            .Returns(_circuitSummaryCommandMock.Object);
        _appHost.Container.AddSingleton(_commandExecutorMock.Object);
        _appHost.Container.AddTransient<RaceDataQueryService>();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() => _appHost.Dispose();

    [Test]
    public async Task Can_call_RaceDataQueryServices()
    {
        var service = _appHost.Container.Resolve<RaceDataQueryService>();
        
        var response = await service.GetAsync(new CircuitSummaryRequest());
        
        response.Should().BeEquivalentTo(_expectedSummaries);
    }
}