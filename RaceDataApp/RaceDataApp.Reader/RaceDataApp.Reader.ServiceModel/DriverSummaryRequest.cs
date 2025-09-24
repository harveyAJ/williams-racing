using RaceDataApp.Reader.Domain.Entities;
using ServiceStack;

namespace RaceDataApp.Reader.ServiceModel;

[Route("/driver-summary")]
public class DriverSummaryRequest : IGet, IReturn<List<DriverSummary>>;