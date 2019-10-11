using System.Windows;
using DalliKlick.Implementations.Interfaces;

namespace DalliKlick.Implementations.Window
{
    public class MessageManager : IMessageManager
    {
        public MessageBoxResult Show(string message, string title = "", MessageBoxButton buttons = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.None)
        {
            return MessageBox.Show(message, title, buttons, image);
        }


        public MessageBoxResult ShowError(string message, string title = "Fehler", MessageBoxButton buttons = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.Hand)
        {
            return MessageBox.Show(message, title, buttons, image);
        }
    }
}