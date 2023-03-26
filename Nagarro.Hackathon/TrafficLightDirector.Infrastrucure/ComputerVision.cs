namespace TrafficLightDirector.Infrastrucure
{
    using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
    using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class ComputerVision : IComputerVision
    {
        private readonly ComputerVisionClient client;

        public ComputerVision(IConfiguration configuration)
        {
            client = BuildComputerVisionClient(configuration["ComputerVisionEndPoint"] ?? "",
                configuration["ComputerVisionAuthKey"] ?? "");
        }

        private ComputerVisionClient BuildComputerVisionClient(string endpoint, string key)
        {
            return new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
            {
                Endpoint = endpoint
            };
        }

        public async Task<List<DetectedObject>> AnalyzeImageObjects(string imageUrl)
        {
            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Objects
            };
            ImageAnalysis results = await client.AnalyzeImageAsync(imageUrl, visualFeatures: features);

            return results.Objects.ToList();
        }
    }
}
