using System.IO;
using System.Reflection;
using DalliKlick.Implementations.Interfaces.Services;

namespace DalliKlick.Implementations.Services
{
    public class GameImageService : IGameImageService
    {
        public string[] GetImagePaths()
        {
            var gameDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var imageDirectory = Path.Combine(gameDirectory ?? "", "Bilder");
            if (!Directory.Exists(imageDirectory))
                Directory.CreateDirectory(imageDirectory);

            return Directory.GetFiles(imageDirectory);
        }
    }
}