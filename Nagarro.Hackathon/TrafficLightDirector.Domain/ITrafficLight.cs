namespace TrafficLightDirector.Domain
{
    public interface ITrafficLight
    {
        void TrafficLightInit();
        Task UpdateLightIfTrafficSignalHaveTraffic();
    }
}