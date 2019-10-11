using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DalliKlick.Views.Game.Overlays
{
    public class OverlayManager : Canvas
    {
        public OverlayManager()
        {
            SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Initialize(CurrentOverlay);
        }

        // Dependency Property
        public static readonly DependencyProperty CurrentOverlayProperty =
            DependencyProperty.Register("CurrentOverlay", typeof(IOverlay),
                typeof(OverlayManager), new FrameworkPropertyMetadata(OnCurrentOverlayPropertyChanged)
                {
                    BindsTwoWayByDefault = true,
                    
                });
        
        private static void OnCurrentOverlayPropertyChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            OverlayManager control = source as OverlayManager;
            control.CurrentOverlay = (IOverlay)e.NewValue;
        }
        
        // .NET Property wrapper
        public IOverlay CurrentOverlay
        {
            get { return (IOverlay)GetValue(CurrentOverlayProperty); }
            set
            {
                SetValue(CurrentOverlayProperty, value);
                Initialize(value);
            }
        }

        


        private void Initialize(IOverlay overlay)
        {
            if(overlay == null)
                return;
            if (Math.Abs(ActualWidth) < Double.Epsilon || Math.Abs(ActualHeight) < Double.Epsilon)
                return;
            foreach (UIElement child in this.Children)
            {
                child.MouseEnter -= PathOnMouseEnter;
                child.PreviewMouseDown -= ChildOnPreviewMouseDown;
            }
            this.Children.Clear();


            var puzzle = overlay as Puzzle;

            var piceSize = new Point(100,100);
            var amountX = ActualWidth / piceSize.X;
            var amountY = ActualHeight / piceSize.Y;
            for (double y = 0; y < amountY * piceSize.Y; y += piceSize.Y)
            {
                for (double x = 0; x < amountX*piceSize.X; x+=piceSize.X)
                {
                    var position = PiecePosition.None;
                    if (Math.Abs(y) < Double.Epsilon)
                        position = position | PiecePosition.Top;
                    if (Math.Abs(x) < Double.Epsilon)
                        position = position | PiecePosition.Left;

                    var path = puzzle.GetPath(position);
                    path.MouseEnter += PathOnMouseEnter;
                    path.PreviewMouseDown += ChildOnPreviewMouseDown;
                    path.RenderTransform = new TransformGroup()
                    {
                        Children = new TransformCollection()
                        {
                            new TranslateTransform(x,y),
                            new ScaleTransform(1,1)
                        }
                    };
                    this.Children.Add(path);
                }
            }

        }

        private void ChildOnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Path path))
                return;
            RemoveChild(path);
        }

        private void PathOnMouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;
            if (!(sender is Path path))
                return;
            RemoveChild(path);
        }

        private void RemoveChild(Path path)
        {
            path.MouseEnter -= PathOnMouseEnter;
            path.PreviewMouseDown -= ChildOnPreviewMouseDown;
            this.Children.Remove(path);
        }
    }
}