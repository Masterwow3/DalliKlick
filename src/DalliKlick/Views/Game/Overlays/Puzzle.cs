using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DalliKlick.Views.Game.Overlays
{
    public class Puzzle : IOverlay
    {
        public Puzzle()
        {
        }

        private int BoolToTab(bool tab, bool inverseTab = false)
        {
            return tab ? (inverseTab ? -1 : 1) : 0;
        }
        private PathGeometry GetGeometry(PiecePosition position)
        {
            var tabCoords = new double[]
            {
                0, 0, 35, 15, 37, 5,
                37, 5, 40, 0, 38, -5,
                38, -5, 20, -20, 50, -20,
                50, -20, 80, -20, 62, -5,
                62, -5, 60, 0, 63, 5,
                63, 5, 65, 15, 100, 0
            };

            int upperConnection = BoolToTab(!position.HasFlag(PiecePosition.Top)),
                rightConnection = BoolToTab(!position.HasFlag(PiecePosition.Right), true),
                bottomConnection = BoolToTab(!position.HasFlag(PiecePosition.Bottom), true),
                leftConnection = BoolToTab(!position.HasFlag(PiecePosition.Left));

            var upperPoints = new List<Point>();
            var rightPoints = new List<Point>();
            var bottomPoints = new List<Point>();
            var leftPoints = new List<Point>();

            for (var i = 0; i < (tabCoords.Length / 2); i++)
            {
                double[] upperCoords = tabCoords;
                double[] rightCoords = tabCoords;
                double[] bottomCoords = tabCoords;
                double[] leftCoords = tabCoords;

                upperPoints.Add(new Point(upperCoords[i * 2], 0 + upperCoords[i * 2 + 1] * upperConnection));
                rightPoints.Add(new Point(100 - rightCoords[i * 2 + 1] * rightConnection, rightCoords[i * 2]));
                bottomPoints.Add(new Point(100 - bottomCoords[i * 2], 100 - bottomCoords[i * 2 + 1] * bottomConnection));
                leftPoints.Add(new Point(0 + leftCoords[i * 2 + 1] * leftConnection, 100 - leftCoords[i * 2]));
            }
            var upperSegment = new PolyBezierSegment(upperPoints, true);
            var rightSegment = new PolyBezierSegment(rightPoints, true);
            var bottomSegment = new PolyBezierSegment(bottomPoints, true);
            var leftSegment = new PolyBezierSegment(leftPoints, true);

            var pathFigure = new PathFigure()
            {
                IsClosed = false,
                StartPoint = new Point(0, 0)
            };
            pathFigure.Segments.Add(upperSegment);
            pathFigure.Segments.Add(rightSegment);
            pathFigure.Segments.Add(bottomSegment);
            pathFigure.Segments.Add(leftSegment);

            var pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);
            return pathGeometry;
        }

        public Path GetPath(PiecePosition position)
        {
            var path = new Path()
            {
                Stroke = new SolidColorBrush(Colors.Gray),
                StrokeThickness = 1,
            };
            
            path.Fill = new SolidColorBrush(Colors.Yellow);

            path.Data = GetGeometry(position);

            var tt1 = new TranslateTransform()
            {
                X = 0,
                Y = 0
            };
            var rt = new RotateTransform()
            {
                CenterX = 50,
                CenterY = 50,
                Angle = 0
            };


            TransformGroup tg1 = new TransformGroup();

            tg1.Children.Add(tt1);
            tg1.Children.Add(rt);
            path.RenderTransform = tg1;

            return path;
        }
    }
}