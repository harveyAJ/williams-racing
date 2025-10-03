using FluentAssertions;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using RaceDataApp.Loader.ServiceInterface;
using RaceDataApp.Loader.ServiceModel;

namespace RaceDataApp.Loader.Tests;

public class UnitTest
{
    private readonly ServiceStackHost _appHost;

    public UnitTest()
    {
        _appHost = new BasicAppHost().Init();
        _appHost.Container.AddTransient<RaceDataLoaderService>();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() => _appHost.Dispose();

    [Test]
    public void Can_call_RaceDataLoaderService()
    {
        var service = _appHost.Container.Resolve<RaceDataLoaderService>();

        var response = (SaveDriverResponse)service.Post(new SaveDriver { Forename = "Valentin" });

        response.DriverId.Should().Be(123);
    }
}