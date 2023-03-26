namespace TrafficLightDirector.Domain
{
    using System.Timers;
    using TrafficLightDirector.Domain.Enum;
    using TrafficLightDirector.Domain.Model;

    public class TrafficLight : ITrafficLight
    {
        private readonly ITrafficHelper trafficHelper;

        private readonly string[] TrafficFeeds = {
            "https://stghackathonapollo.blob.core.windows.net/images/1.jpg",
            "https://stghackathonapollo.blob.core.windows.net/images/2.jpg",
            "https://stghackathonapollo.blob.core.windows.net/images/3.jpg",
            "https://stghackathonapollo.blob.core.windows.net/images/4.jpg",
            "https://stghackathonapollo.blob.core.windows.net/images/5.jpg",
            "https://stghackathonapollo.blob.core.windows.net/images/6.jpg",
            "https://stghackathonapollo.blob.core.windows.net/images/7.jpg",
            "https://stghackathonapollo.blob.core.windows.net/images/8.jpg",
            "https://stghackathonapollo.blob.core.windows.net/images/9.jpg",
            "https://stghackathonapollo.blob.core.windows.net/images/10.jpg"
        };

        private System.Timers.Timer ResetTimer;

        public TrafficLight(ITrafficHelper trafficHelper)
        {
            this.trafficHelper = trafficHelper;
        }

        public void TrafficLightInit()
        {
            if (ResetTimer == null)
            {
                StartGreenLightTimer();
            }
        }

        private void StartGreenLightTimer()
        {
            ResetTimer = new System.Timers.Timer();
            ResetTimer.Elapsed += GreenLightTimer_Elapsed;
            ResetTimer.Interval = 1000 * 10;
            ResetTimer.Enabled = true;
        }

        public async Task UpdateLightIfTrafficSignalHaveTraffic()
        {
            // Analyze an image to get features and other properties.
            Random rnd = new Random();
            string feed = TrafficFeeds[rnd.Next(TrafficFeeds.Length)];

            bool hasTraffic = await trafficHelper.DoesTrafficSignalHaveTraffic(feed);

            int curr = (int)TrafficSignal.CurrentDirection - 1;
            TrafficSignal.State[curr].LiveFeed = feed;

            if (!hasTraffic && CanChangeLight())
            {
                UpdateLights(feed);
            }
        }

        private void GreenLightTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {

            var diff = DateTime.Now - TrafficSignal.LightChangedAt;
            //set state to ready for current direction and for next direction
            int curr = (int)TrafficSignal.CurrentDirection - 1;
            int next = (int)TrafficSignal.CurrentDirection >= 4 ? 0 : (int)TrafficSignal.CurrentDirection;

            if (TrafficSignal.ChangeLight)
            {
                TrafficSignal.ChangeLight = false;
                ChangeLightDirection(curr, next);
            }
            else if (diff.TotalSeconds >= 59)
            {
                ChangeLightDirection(curr, next);
            }
            else if (diff.TotalSeconds >= 54)
            {
                TrafficSignal.State[curr].CurrentState = LightState.Ready;
                //CommonValues.State[next].LightState = LightState.Ready;
            }
        }
        private void UpdateLights(string feed)
        {
            int curr = (int)TrafficSignal.CurrentDirection - 1;

            TrafficSignal.State[curr].CurrentState = LightState.Ready;
           
            TrafficSignal.ChangeLight = true;
        }

        /// <summary>
        /// Change Light direction
        /// </summary>
        /// <param name="curr"></param>
        /// <param name="next"></param>
        public void ChangeLightDirection(int curr, int next)
        {
            //change light to stop and go for next light
            TrafficSignal.State[curr].CurrentState = LightState.Stop;
            TrafficSignal.State[next].CurrentState = LightState.Go;
            //reset timer and change current direction
            var nextDirection = (int)TrafficSignal.CurrentDirection + 1;
            nextDirection = nextDirection > 4 ? 1 : nextDirection;
            TrafficSignal.CurrentDirection = (TrafficLightSide)(nextDirection);
            TrafficSignal.LightChangedAt = DateTime.Now;
        }

        private bool CanChangeLight()
        {
            var diff = DateTime.Now - TrafficSignal.LightChangedAt;
            return !TrafficSignal.ChangeLight && diff.TotalSeconds > 5;
        }
    }
}