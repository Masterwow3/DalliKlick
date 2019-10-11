using System.Windows;

namespace DalliKlick.Implementations.Interfaces
{
    public interface IMessageManager
    {
        MessageBoxResult Show(string message, string title = "", MessageBoxButton buttons = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.None);

        MessageBoxResult ShowError(string message, string title = "Fehler",
            MessageBoxButton buttons = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.Hand);
    }
}