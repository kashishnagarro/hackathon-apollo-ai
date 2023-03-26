using TrafficLightDirector.Domain.Enum;

namespace TrafficLightDirector.Domain.Model
{
    public class State
    {
        public string LiveFeed { get; set; } = "https://stghackathonapollo.blob.core.windows.net/images/1.jpg";
        public LightState CurrentState { get; set; }
        public TrafficLightSide TrafficLightId { get; set; }
    }
}
