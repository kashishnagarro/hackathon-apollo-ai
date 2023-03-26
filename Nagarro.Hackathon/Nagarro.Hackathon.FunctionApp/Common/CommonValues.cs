namespace Nagarro.Hackathon.FunctionApp
{
    public class CommonValues
    {
        public static TrafficLightSide CurrentDirection = TrafficLightSide.First;
        public static DateTime LightChangedAt = DateTime.Now;

        public static List<State> State = new List<State>
        {
            new State
            {
                Side= TrafficLightSide.First,
                LightState = LightState.Go
            },
            new State
            {
                Side= TrafficLightSide.Second,
                LightState = LightState.Stop
            },
            new State
            {
                Side= TrafficLightSide.Third,
                LightState = LightState.Stop
            },
            new State
            {
                Side= TrafficLightSide.Fourth,
                LightState = LightState.Stop
            }
        };
    }
}
