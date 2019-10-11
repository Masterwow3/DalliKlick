using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;

namespace DalliKlick.Implementations.Window
{
    public class CustomWindowManager : WindowManager
    {
        #region Overrides of WindowManager
        

        protected override System.Windows.Window EnsureWindow(object model, object view, bool isDialog)
        {
            if (!(view is System.Windows.Window window))
            {
                window = CreateWindow(model, view, isDialog);
                window.Content = view;
                window.SetValue(View.IsGeneratedProperty, true);
                var owner = InferOwnerOf(window);
                if (owner != null && isDialog)
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    window.Owner = owner;
                }
                else
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
                window.SizeToContent = SizeToContent.Manual;
            }
            else
            {
                var owner = InferOwnerOf(window);
                if (owner != null && isDialog)
                {
                    window.Owner = owner;
                }
            }
            return window;
        }

        #endregion Overrides of WindowManager

        #region Methods

        protected virtual System.Windows.Window CreateWindow(object model, object view, bool isDialog)
        {
            var window = new System.Windows.Window
            {
                //Icon = new BitmapImage(TODO),
                UseLayoutRounding = true,
                SnapsToDevicePixels = true
            };
            if (view is UserControl userControl)
            {
                if (Math.Abs(userControl.MinHeight) > double.Epsilon)
                {
                    window.MinHeight = userControl.MinHeight + 1;
                }
                if (Math.Abs(userControl.MinWidth) > double.Epsilon)
                {
                    window.MinWidth = userControl.MinWidth + 1;
                }
                if (!double.IsPositiveInfinity(userControl.MaxHeight))
                {
                    window.MaxHeight = userControl.MaxHeight;
                }
                if (!double.IsPositiveInfinity(userControl.MaxWidth))
                {
                    window.MaxWidth = userControl.MaxWidth;
                }
            }
            return window;
        }

        #endregion Methods
    }
}