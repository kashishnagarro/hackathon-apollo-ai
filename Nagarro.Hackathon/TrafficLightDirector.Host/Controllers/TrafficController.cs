using Microsoft.AspNetCore.Mvc;
using TrafficLightDirector.Domain;
using TrafficLightDirector.Domain.Model;

namespace TrafficLightDirector.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrafficController : ControllerBase
    {


        private readonly ILogger<TrafficController> _logger;
        private readonly ITrafficLight trafficLight;

        public TrafficController(ILogger<TrafficController> logger, ITrafficLight trafficLight)
        {
            _logger = logger;
            this.trafficLight = trafficLight;
            trafficLight.TrafficLightInit();
        }

        [HttpGet(Name = "trafficsignal")]
        public async Task<dynamic> Get()
        {
            await trafficLight.UpdateLightIfTrafficSignalHaveTraffic();
            return TrafficSignal.State;
        }
    }
}