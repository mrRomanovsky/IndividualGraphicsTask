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
            /*if (sa > 0.001)
                return PointToEdgeRelation.Left;
            if (sa < -0.001)
                return PointToEdgeRelation.Right;
            if ((a.X * b.X < -0.001) || (a.Y * b.Y < -0.001))
                return PointToEdgeRelation.Behind;
            if (Math.Sqrt(a.X * a.X + a.Y * a.Y) < Math.Sqrt(b.X * b.X + b.Y * b.Y))
                return PointToEdgeRelation.Beyond;*/
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

        /*
         * function classify(point, line) {
    var p0 = line.first;
    var p1 = line.second;
    var p2 = point;

    var a = {
        x: p1.x - p0.x,
        y: p1.y - p0.y
    );

    var b = {
        x: p2.x - p0.x,
        y: p2.y - p0.y
    );

    var sa = a.x * b.y - b.x * a.y;
    if (sa > 0.0) return "left";
    if (sa < 0.0) return "right";
    if ((a.x * b.x < 0.0) || (a.y * b.y < 0.0)) return "behind";
    if (a.length < b.length) return "beyond";
    if (p0.equals(p2))return "origin";
    if (p1 == p2)return "destination";
    return "between";
}
         */
    }
}
