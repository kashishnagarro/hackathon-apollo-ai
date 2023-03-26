
namespace TrafficLightDirector.Domain
{
    public interface ITrafficHelper
    {
        Task<bool> DoesTrafficSignalHaveTraffic(string currentFeed);
    }
}