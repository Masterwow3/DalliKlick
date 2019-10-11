using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace DalliKlick.Views.Game
{
    public class GameItem
    {
        public GameItem(string imagePath)
        {
            ImagePath = imagePath;
            if(!File.Exists(imagePath))
                throw new Exception("Image not found");

            Bitmap = new BitmapImage(new Uri(ImagePath, UriKind.Absolute));
            Bitmap.Freeze();
        }

        public BitmapImage Bitmap { get; }

        public string ImagePath { get; }
    }
}