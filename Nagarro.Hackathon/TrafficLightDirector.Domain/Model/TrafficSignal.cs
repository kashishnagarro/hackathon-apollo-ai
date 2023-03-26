namespace TrafficLightDirector.Domain.Model
{
    using TrafficLightDirector.Domain.Enum;


    public class TrafficSignal
    {
        public static TrafficLightSide CurrentDirection = TrafficLightSide.First;
        public static DateTime LightChangedAt = DateTime.Now;
        //If current lane has no traffic set this value to true, so that timer can change light accordingly
        public static bool ChangeLight = false;

        public static List<State> State = new List<State>
        {
            new State
            {
                TrafficLightId= TrafficLightSide.First,
                CurrentState = LightState.Go,
                LiveFeed=""
            },
            new State
            {
                TrafficLightId= TrafficLightSide.Second,
                CurrentState = LightState.Stop
            },
            new State
            {
                TrafficLightId= TrafficLightSide.Third,
                CurrentState = LightState.Stop
            },
            new State
            {
                TrafficLightId= TrafficLightSide.Fourth,
                CurrentState = LightState.Stop
            }
        };
    }
}
