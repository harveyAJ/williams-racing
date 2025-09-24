using RaceDataApp.Reader.Domain.Entities;
using ServiceStack;

namespace RaceDataApp.Reader.ServiceModel;

[Route("/circuit-summary")]
public class CircuitSummaryRequest : IGet, IReturn<List<CircuitSummary>>;