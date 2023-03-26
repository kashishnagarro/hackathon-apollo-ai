using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace TrafficLightDirector.Infrastrucure
{
    public interface IComputerVision
    {
        Task<List<DetectedObject>> AnalyzeImageObjects(string imageUrl);
    }
}