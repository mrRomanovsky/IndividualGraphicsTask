using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineHull
{
    public enum PointToEdgeRelation
    {
        Left,
        Right,
        Behind,
        Beyond,
        Origin,
        Destination,
        Between
    };

    public static class PointExtensions
    {

        public static PointToEdgeRelation Classify(this PointF p2, PointF p0, PointF p1)
        {
            PointF a = new PointF(p1.X - p0.X, p1.Y - p0.Y);
            PointF b = new PointF(p2.X - p0.X, p2.Y - p0.Y);
            float sa = a.X * b.Y - b.X * a.Y;
            if (sa > 0)
                return PointToEdgeRelation.Left;
            if (sa < 0)
                return PointToEdgeRelation.Right;
            if ((a.X * b.X < 0) || (a.Y * b.Y < 0))
                return PointToEdgeRelation.Behind;
            if (Math.Sqrt(a.X * a.X + a.Y * a.Y) < Math.Sqrt(b.X * b.X + b.Y * b.Y))
                return PointToEdgeRelation.Beyond;
            if (p0 == p2)
                return PointToEdgeRelation.Origin;
            if (p1 == p2)
                return PointToEdgeRelation.Destination;
            return PointToEdgeRelation.Between;
        }
    }
}
