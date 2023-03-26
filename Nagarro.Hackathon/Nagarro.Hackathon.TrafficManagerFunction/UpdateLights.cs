using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Nagarro.Hackathon.TrafficManagerFunction
{
    public class UpdateLights
    {
        private readonly ILogger _logger;

        public UpdateLights(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UpdateLights>();
        }

        [Function("UpdateLights")]
        public void Run([TimerTrigger("*/5 * * * * *")] MyInfo myTimer)
        {
            var diff = DateTime.Now - CommonValues.LightChangedAt;
            //set state to ready for current direction and for next direction
            int curr = (int)CommonValues.CurrentDirection - 1;
            int next = (int)CommonValues.CurrentDirection >= 4 ? 0 : (int)CommonValues.CurrentDirection;

            if (CommonValues.ChangeLight)
            {
                CommonValues.ChangeLight = false;
                ChangeLightDirection(curr, next);
            }
            else if (diff.TotalSeconds >= 59)
            {
                ChangeLightDirection(curr, next);
            }
            else if (diff.TotalSeconds >= 54)
            {
                CommonValues.State[curr].LightState = LightState.Ready;
                //CommonValues.State[next].LightState = LightState.Ready;
            }

            //Console.WriteLine(JsonConvert.SerializeObject(CommonValues.State, Formatting.Indented));
        }

        /// <summary>
        /// Change Light direction
        /// </summary>
        /// <param name="curr"></param>
        /// <param name="next"></param>
        public void ChangeLightDirection(int curr, int next)
        {
            //change light to stop and go for next light
            CommonValues.State[curr].LightState = LightState.Stop;
            CommonValues.State[next].LightState = LightState.Go;
            //reset timer and change current direction
            var nextDirection = (int)CommonValues.CurrentDirection + 1;
            nextDirection = nextDirection > 4 ? 1 : nextDirection;
            CommonValues.CurrentDirection = (TrafficLightSide)(nextDirection);
            CommonValues.LightChangedAt = DateTime.Now;
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
