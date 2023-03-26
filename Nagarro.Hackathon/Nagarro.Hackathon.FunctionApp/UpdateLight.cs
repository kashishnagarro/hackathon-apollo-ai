using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Nagarro.Hackathon.FunctionApp
{
    public class UpdateLight
    {
        private readonly ILogger _logger;

        public UpdateLight(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UpdateLight>();
        }

        [Function("UpdateLight")]
        public void Run([TimerTrigger("0 */5 * * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

            var diff = DateTime.Now - CommonValues.LightChangedAt;
            if (diff.TotalSeconds >= 54)
            {
                //set state to ready for current direction and for next direction
                CommonValues.State[(int)CommonValues.CurrentDirection - 1].LightState = LightState.Ready;
                CommonValues.State[(int)CommonValues.CurrentDirection].LightState = LightState.Ready;
            }
            else if (diff.TotalSeconds >= 59)
            {
                //change light to stop and go for next light
                CommonValues.State[(int)CommonValues.CurrentDirection - 1].LightState = LightState.Stop;
                CommonValues.State[(int)CommonValues.CurrentDirection].LightState = LightState.Go;
                //reset timer and change current direction
                CommonValues.CurrentDirection = (TrafficLightSide)((int)CommonValues.CurrentDirection + 1);
                CommonValues.LightChangedAt = DateTime.Now;
            }

            Console.WriteLine(JsonConvert.SerializeObject(CommonValues.State, Formatting.Indented));
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
