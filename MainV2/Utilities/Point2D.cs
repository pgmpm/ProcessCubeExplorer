
namespace pgmpm.MainV2.Utilities
{
    /// <summary>
    /// A 2D point
    /// 
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    public class Point2D
    {
        public double x { get; set; }
        public double y { get; set; }

        public Point2D(double _x, double _y)
        {
            x = _x;
            y = _y;
        }

        public bool Equals(Point2D p)
        {
            if (p == null) return false;
            if(p == this) return true;

            // Return true if the fields match:
            return (x == p.x) && (y == p.y);
        }
    }
}
