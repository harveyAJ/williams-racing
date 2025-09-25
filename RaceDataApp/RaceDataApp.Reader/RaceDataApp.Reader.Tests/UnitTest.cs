using NUnit.Framework;
using RaceDataApp.Loader.Entities;
using ServiceStack;
using ServiceStack.Testing;
using RaceDataApp.Reader.ServiceInterface;
using RaceDataApp.Reader.ServiceModel;

namespace RaceDataApp.Reader.Tests;

public class UnitTest
{
    private readonly ServiceStackHost appHost;

    public UnitTest()
    {
        appHost = new BasicAppHost().Init();
        appHost.Container.AddTransient<RaceDataQueryService>();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() => appHost.Dispose();

    [Test]
    public void Can_call_RaceDataQueryServices()
    {
        // var service = appHost.Container.Resolve<MyServices>();
        //
        // var response = (HelloResponse)service.Any(new Hello { Name = "World" });
        //
        // Assert.That(response.Result, Is.EqualTo("Hello, World!"));
    }
}