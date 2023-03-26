namespace Nagarro.Hackathon.TrafficManagerFunction
{
    public class CommonValues
    {
        public static TrafficLightSide CurrentDirection = TrafficLightSide.First;
        public static DateTime LightChangedAt = DateTime.Now;
        //If current lane has no traffic set this value to true, so that timer can change light accordingly
        public static bool ChangeLight = false;

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
