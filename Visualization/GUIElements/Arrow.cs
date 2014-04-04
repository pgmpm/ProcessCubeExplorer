using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace pgmpm.Visualization.GUIElements
{
    /// <summary>
    /// Class for an arrow that connects two Thumbs (e.g. a Place and a Transition in a PetriNet).
    /// Source: http://www.codeproject.com/Articles/23116/WPF-Arrow-and-Custom-Shapes
    /// </summary>
    /// <author>Thomas Meents, Krystian Zielonka</author>
    public sealed class Arrow : Shape
    {
        #region initialize

        public static readonly DependencyProperty X1Property = DependencyProperty.Register("X1", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty HeadWidthProperty = DependencyProperty.Register("HeadWidth", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty HeadHeightProperty = DependencyProperty.Register("HeadHeight", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        Thumb StartElement, DestinationElement;
        Double _arrowX1, _arrowY1, _arrowX2, _arrowY2;

        public Arrow(Thumb startElement, Thumb destinationElement)
        {
            StartElement = startElement;
            DestinationElement = destinationElement;
        }

        public double X1
        {
            get { return (double)base.GetValue(X1Property); }
            set { base.SetValue(X1Property, value); }
        }

        public double Y1
        {
            get { return (double)base.GetValue(Y1Property); }
            set { base.SetValue(Y1Property, value); }
        }

        public double X2
        {
            get { return (double)base.GetValue(X2Property); }
            set { base.SetValue(X2Property, value); }
        }

        public double Y2
        {
            get { return (double)base.GetValue(Y2Property); }
            set { base.SetValue(Y2Property, value); }
        }

        public double HeadWidth
        {
            get { return (double)base.GetValue(HeadWidthProperty); }
            set { base.SetValue(HeadWidthProperty, value); }
        }

        public double HeadHeight
        {
            get { return (double)base.GetValue(HeadHeightProperty); }
            set { base.SetValue(HeadHeightProperty, value); }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                // Create a StreamGeometry for describing the shape
                StreamGeometry geometry = new StreamGeometry {FillRule = FillRule.EvenOdd};
                Fill = Brushes.Black;

                using (StreamGeometryContext context = geometry.Open())
                {
                    InternalDrawArrowGeometry(context);
                }

                // Freeze the geometry for performance benefits
                geometry.Freeze();

                return geometry;
            }
        }

        #endregion initialize

        private void InternalDrawArrowGeometry(StreamGeometryContext context)
        {
            double endOfX1 = 0, endOfX2 = 0, endOfY1 = 0, endOfY2 = 0;

            if (StartElement.Name == "Transition")
            {
                double g_m = (Y1 - Y2) / (X1 - X2);
                double g_b = Y1 - (g_m * X1);

                // Borders
                double leftBorder = X1 - (StartElement.Width / 2);
                double rightBorder = X1 + (StartElement.Width / 2);
                double topBorder = Y1 - (StartElement.Height / 2);
                double bottomBorder = Y1 + (StartElement.Height / 2);

                double tmp_x1, tmp_y1, tmp_x2, tmp_y2;

                if (X1 > X2)
                {
                    tmp_x1 = -(StartElement.Width / 2);
                    tmp_y1 = Math.Abs(Y1 - (g_m * leftBorder + g_b));

                    tmp_y2 = (StartElement.Height / 2);

                    if (Y1 >= Y2)
                    {
                        tmp_y1 *= -1;
                        tmp_x2 = ((topBorder - g_b) / g_m) - X1;
                        tmp_y2 *= -1;
                    }
                    else
                    {
                        tmp_x2 = ((bottomBorder - g_b) / g_m) - X1;
                    }

                    if ((g_m * leftBorder + g_b) >= topBorder && (g_m * leftBorder + g_b) <= bottomBorder)
                    {
                        endOfX1 = tmp_x1;
                        endOfY1 = tmp_y1;
                    }
                    else
                    {
                        endOfX1 = tmp_x2;
                        endOfY1 = tmp_y2;
                    }
                }
                else if (X1 < X2)
                {
                    tmp_x1 = (StartElement.Width / 2);
                    tmp_y1 = Math.Abs(Y1 - (g_m * rightBorder + g_b));

                    tmp_y2 = (StartElement.Height / 2);

                    if (Y1 >= Y2)
                    {
                        tmp_y1 *= -1;
                        tmp_x2 = Math.Abs(X1 - (topBorder - g_b) / g_m);
                        tmp_y2 *= -1;
                    }
                    else
                    {
                        tmp_x2 = Math.Abs(X1 - (bottomBorder - g_b) / g_m);
                    }

                    if ((g_m * leftBorder + g_b) >= topBorder && (g_m * leftBorder + g_b) <= bottomBorder)
                    {
                        endOfX1 = tmp_x1;
                        endOfY1 = tmp_y1;
                    }
                    else
                    {
                        endOfX1 = tmp_x2;
                        endOfY1 = tmp_y2;
                    }
                }
                else
                {
                    if (Y1 > Y2)
                    {
                        endOfY1 = -(StartElement.Height / 2);
                    }
                    else
                    {
                        endOfY1 = (StartElement.Height / 2);
                    }
                }
            }
            else if (DestinationElement.Name == "Transition")
            {
                double g_m = (Y1 - Y2) / (X1 - X2); // m -> Slope (Steigung)
                double g_b = Y2 - (g_m * X2);       // b

                // Borders
                double leftBorder = X2 - (DestinationElement.Width / 2);
                double rightBorder = X2 + (DestinationElement.Width / 2);
                double topBorder = Y2 - (DestinationElement.Height / 2);
                double bottomBorder = Y2 + (DestinationElement.Height / 2);

                double tmp_x1, tmp_y1, tmp_x2, tmp_y2;

                if (X2 > X1)
                {
                    tmp_x1 = -(DestinationElement.Width / 2);
                    tmp_y1 = Math.Abs(Y2 - (g_m * leftBorder + g_b));

                    tmp_y2 = (DestinationElement.Height / 2);

                    if (Y2 >= Y1)
                    {
                        tmp_y1 *= -1;
                        tmp_x2 = ((topBorder - g_b) / g_m) - X2;
                        tmp_y2 *= -1;
                    }
                    else
                    {
                        tmp_x2 = ((bottomBorder - g_b) / g_m) - X2;
                    }

                    if ((g_m * leftBorder + g_b) >= topBorder && (g_m * leftBorder + g_b) <= bottomBorder)
                    {
                        endOfX2 = tmp_x1;
                        endOfY2 = tmp_y1;
                    }
                    else
                    {
                        endOfX2 = tmp_x2;
                        endOfY2 = tmp_y2;
                    }
                }
                else if (X2 < X1)
                {
                    tmp_x1 = (DestinationElement.Width / 2);
                    tmp_y1 = Math.Abs(Y2 - (g_m * rightBorder + g_b));

                    tmp_y2 = (DestinationElement.Height / 2);

                    if (Y2 >= Y1)
                    {
                        tmp_y1 *= -1;
                        tmp_x2 = Math.Abs(X2 - (topBorder - g_b) / g_m);
                        tmp_y2 *= -1;
                    }
                    else
                    {
                        tmp_x2 = Math.Abs(X2 - (bottomBorder - g_b) / g_m);
                    }

                    if ((g_m * leftBorder + g_b) >= topBorder && (g_m * leftBorder + g_b) <= bottomBorder)
                    {
                        endOfX2 = tmp_x1;
                        endOfY2 = tmp_y1;
                    }
                    else
                    {
                        endOfX2 = tmp_x2;
                        endOfY2 = tmp_y2;
                    }
                }
                else
                {
                    if (Y1 > Y2)
                    {
                        endOfY2 = (DestinationElement.Height / 2);
                    }
                    else
                    {
                        endOfY2 = -(DestinationElement.Height / 2);
                    }
                }
            }
            else
            {
                throw new Exception("Exception: No name for the start- and destination element");
            }

            if (StartElement.Name == "Place")
            {
                double radius = StartElement.Width / 2;  //Radius Place

                double g_m = (Y1 - Y2) / (X1 - X2); // Slope (Steigung)
                double g_b = Y1 - (g_m * X1);       // b

                double a = 1 + Math.Pow(g_m, 2);
                double b = (2 * g_m * g_b) - (2 * g_m * Y1) - (2 * X1);
                double c = (g_b * g_b) - (2 * g_b * Y1) + Math.Pow(Y1, 2) - Math.Pow(radius, 2) + Math.Pow(X1, 2);

                double schnittX1 = (-b + Math.Sqrt(Math.Pow(b, 2) - (4 * a * c))) / (2 * a);
                double schnittY1 = (g_m * schnittX1) + g_b;

                endOfX1 = Math.Abs(X1 - schnittX1);
                endOfY1 = Math.Abs(Y1 - schnittY1);
                
                if (X1 > X2)
                {
                    endOfX1 *= -1;
                }
                else if (X1 == X2) 
                {
                    endOfX1 = 0;
                    endOfY1 = radius;
                }

                if (Y1 > Y2)
                {
                    endOfY1 *= -1;
                }
            }
            else if (DestinationElement.Name == "Place")
            {
                double radius = DestinationElement.Width / 2;  // Radius Place

                double g_m = (Y1 - Y2) / (X1 - X2); // m -> Slope (Steigung)
                double g_b = Y1 - (g_m * X1);       // b

                double a = 1 + Math.Pow(g_m, 2);
                double b = (2 * g_m * g_b) - (2 * g_m * Y2) - (2 * X2);
                double c = (g_b * g_b) - (2 * g_b * Y2) + Math.Pow(Y2, 2) - Math.Pow(radius, 2) + Math.Pow(X2, 2);

                double schnittX1 = (-b + Math.Sqrt(Math.Pow(b, 2) - (4 * a * c))) / (2 * a);
                double schnittY1 = (g_m * schnittX1) + g_b;

                endOfX2 = Math.Abs(X2 - schnittX1);
                endOfY2 = Math.Abs(Y2 - schnittY1);

                if (X1 < X2)
                {
                    endOfX2 *= -1;
                }
                else if (X1 == X2)
                {
                    endOfX2 = 0;
                    endOfY2 = radius;
                }

                if (Y1 < Y2)
                {
                    endOfY2 *= -1;
                }
            }
            else
            {
                throw new Exception("Exception: No name for the start- and destination element");
            }

            DrawLine(context, endOfX1, endOfY1, endOfX2, endOfY2);
            DrawArrow(context, endOfX1, endOfY1, endOfX2, endOfY2);
        }

        private void DrawLine(StreamGeometryContext context, double eX_1, double eY_1, double eX_2, double eY_2)
        {
            _arrowX1 = X1 + eX_1; _arrowY1 = Y1 + eY_1; _arrowX2 = X2 + eX_2; _arrowY2 = Y2 + eY_2;

            Point pt1 = new Point(_arrowX1, _arrowY1);
            Point pt2 = new Point(_arrowX2, _arrowY2);

            context.BeginFigure(pt1, false, false);
            context.LineTo(pt2, true, true);
        }

        /// <summary>
        /// Add an arrowhead at the end of the line.
        /// </summary>
        private void DrawArrow(StreamGeometryContext context, double eX_1, double eY_1, double eX_2, double eY_2)
        {
            _arrowX1 = X1 + eX_1; _arrowY1 = Y1 + eY_1; _arrowX2 = X2 + eX_2; _arrowY2 = Y2 + eY_2;

            double theta = Math.Atan2(_arrowY1 - _arrowY2, _arrowX1 - _arrowX2);
            double sint = Math.Sin(theta);
            double cost = Math.Cos(theta);

            Point pt3 = new Point(_arrowX2, _arrowY2);

            Point pt4 = new Point(
                _arrowX2 + (HeadWidth * cost - HeadHeight * sint),
                _arrowY2 + (HeadWidth * sint + HeadHeight * cost));

            Point pt5 = new Point(
                _arrowX2 + (HeadWidth * cost + HeadHeight * sint),
                _arrowY2 - (HeadHeight * cost - HeadWidth * sint));

            context.BeginFigure(pt3, true, false);
            context.LineTo(pt4, true, true);
            context.LineTo(pt3, true, true);
            context.LineTo(pt5, true, true);
            context.LineTo(pt4, true, true);
        }
    }
}