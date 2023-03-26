using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using System.Net;

namespace Nagarro.Hackathon.TrafficManagerFunction
{
    public class GetTrafficState
    {
        static string subscriptionKey = "3a1b070218a344328e93f272cfe446b8";  //ConfigManager.GetAppSettings(AppSettings.UMAuthority);
        static string endpoint = "https://hackathon-apollo.cognitiveservices.azure.com/";
        
        private readonly string[] images = {
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

        //if any of these items exists then we consider road with traffic
        private static readonly string[] trafficItems = { "land vehicle", "mode of transport", "bus", "traffic congestion", "car", "crowded", "vehicle" };

        [Function("GetTrafficState")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            // Create a client
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

            // Analyze an image to get features and other properties.
            Random rnd = new Random();
            string img = images[rnd.Next(images.Length)];
            bool hasTraffic = await CheckTraffic(client, img);

            //When there is no traffic in current lane and change lights hasn't been turned on
            if (!hasTraffic && CanChangeLight())
            {
                UpdateLights();
            }

            var response = req.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString(JsonConvert.SerializeObject(CommonValues.State));

            return response;
        }

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        public static async Task<bool> CheckTraffic(ComputerVisionClient client, string imageUrl)
        {
            // Creating a list that defines the features to be extracted from the image. 
            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Tags
            };

            Console.WriteLine($"Analyzing the image {Path.GetFileName(imageUrl)}...");
            Console.WriteLine();
            // Analyze the URL image 
            ImageAnalysis results = await client.AnalyzeImageAsync(imageUrl, visualFeatures: features);

            bool hasTraffic = results.Tags.Any(x => trafficItems.Contains(x.Name) && x.Confidence > 0.5);
            return hasTraffic;
        }

        public static void UpdateLights()
        {
            int curr = (int)CommonValues.CurrentDirection - 1;

            CommonValues.State[curr].LightState = LightState.Ready;
            CommonValues.ChangeLight = true;
        }

        public static bool CanChangeLight()
        {
            var diff = DateTime.Now - CommonValues.LightChangedAt;
            return !CommonValues.ChangeLight && diff.TotalSeconds > 5;
        }
    }
}