using System.Collections.Generic;
using System.Windows;

namespace DalliKlick.Implementations.Window
{
    public class WindowSettings : Dictionary<string, object>
    {
        protected WindowSettings() { }


        #region Methods

        public static WindowSettings With()
        {
            return new WindowSettings();
        }


        public WindowSettings AutoSize()
        {
            this[System.Windows.Window.SizeToContentProperty.Name] = SizeToContent.WidthAndHeight;
            return this;
        }


        public WindowSettings FixedSize(int width, int height, int? minWidth = null, int? minHeight = null)
        {
            this[FrameworkElement.WidthProperty.Name] = width;
            this[FrameworkElement.HeightProperty.Name] = height;
            this[FrameworkElement.MinWidthProperty.Name] = minWidth;
            this[FrameworkElement.MinHeightProperty.Name] = minHeight;
            return this;
        }


        public WindowSettings Resize()
        {
            this[System.Windows.Window.ResizeModeProperty.Name] = ResizeMode.CanResizeWithGrip;
            return this;
        }


        public WindowSettings NoResize()
        {
            this[System.Windows.Window.ResizeModeProperty.Name] = ResizeMode.NoResize;
            return this;
        }

        #endregion Methods
    }
}