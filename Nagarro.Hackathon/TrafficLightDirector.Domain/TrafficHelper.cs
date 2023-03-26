namespace TrafficLightDirector.Domain
{
    using System.Threading.Tasks;
    using TrafficLightDirector.Infrastrucure;

    public class TrafficHelper : ITrafficHelper
    {
        private readonly IComputerVision computerVision;

        public TrafficHelper(IComputerVision computerVision)
        {
            this.computerVision = computerVision;
        }

        public async Task<bool> DoesTrafficSignalHaveTraffic(string currentFeed)
        {
            var result = false;
            var detectedObjects = await computerVision.AnalyzeImageObjects(currentFeed);

            foreach (var detectedObject in detectedObjects)
            {
                if (detectedObject.ObjectProperty.ToLower() == "vehicle")
                {
                    result = true;
                    break;
                }
                var detectedObjectParent = detectedObject.Parent;
                while (detectedObjectParent != null)
                {
                    if (detectedObjectParent.ObjectProperty.ToLower() == "vehicle")
                    {
                        result = true;
                        break;
                    }
                    detectedObjectParent = detectedObjectParent.Parent;
                }
            }
            return result;
        }
    }
}
