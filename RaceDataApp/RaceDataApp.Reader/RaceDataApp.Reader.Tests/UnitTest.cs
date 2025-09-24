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

public class DataSetTests
{
    [Test]
    public void LapTimes()
    {
        using var reader = new StreamReader("./dataset/lap_times.csv");
        var headerLine = reader.ReadLine(); // skip the header
        if (headerLine == null) return; //probably throw?

        //var entities = new List<LapTime>();
        HashSet<string> hashSet = new HashSet<string>();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var parts = line.Split(',');

            try
            {
                if (hashSet.Contains($"{parts[0].Trim()}-{parts[1].Trim()}-{parts[2].Trim()}"))
                {
                    Console.WriteLine(line);
                }

                hashSet.Add($"{parts[0].Trim()}-{parts[1].Trim()}-{parts[2].Trim()}");
                // entities.Add(new LapTime
                // {
                //     RaceId       = int.Parse(parts[0]),
                //     DriverId     = int.Parse(parts[1]),
                //     Lap          = int.Parse(parts[2]),
                //     Position     = int.Parse(parts[3]),
                //     Time         = parts[4].Trim('"'),
                //     Milliseconds = int.Parse(parts[5])
                // });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // if (entities.Count >= BatchSize)
            // {
            //     entities.Clear();
            // }
        }
    }
}