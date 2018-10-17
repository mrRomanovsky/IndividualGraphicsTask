using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineHull
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<PointF> points = new List<PointF>();

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            points.Add(e.Location);
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            points.Clear();
        }


        Polygon InsertionHull(List<PointF> points)
        {
            Polygon p = new Polygon();
            if (points.Count > 0)
                p.Insert(points[0]);
            foreach (var point in points)
            {
                if (!PointInConvexPolygon(point, p))
                {
                    LeastVertex(p, ClosestToPolygon(point));
                    SupportingLine(point, p, PointToEdgeRelation.Left);
                    Vertex l = p.V;
                    SupportingLine(point, p, PointToEdgeRelation.Right);
                    p.Split(l);
                    p.Insert(point);
                }
            }
            return p;
        }

        void SupportingLine(PointF s, Polygon p, PointToEdgeRelation side)
        {
            Rotation rotation = side == PointToEdgeRelation.Left ? Rotation.Clockwise : Rotation.Counterclockwise;
            Vertex a = p.V;
            Vertex b = p.Neighbour(rotation);
            PointToEdgeRelation rel = b.Point.Classify(s, a.Point);
            while (rel == side || rel == PointToEdgeRelation.Beyond || rel == PointToEdgeRelation.Between)
            {
                p.Advance(rotation);
                a = p.V;
                b = p.Neighbour(rotation);
                rel = b.Point.Classify(s, a.Point);
            }
        }

        Func<Vertex, Vertex, int> ClosestToPolygon(PointF p) =>
            (a, b) =>
            {
                PointF aDiff = new PointF(p.X - a.Point.X, p.Y - a.Point.Y);
                double aDist = Math.Sqrt(aDiff.X * aDiff.X + aDiff.Y * aDiff.Y);
                PointF bDiff = new PointF(p.X - b.Point.X, p.Y - b.Point.Y);
                double bDist = Math.Sqrt(bDiff.X * bDiff.X + bDiff.Y * bDiff.Y);
                if (aDist - bDist < 0.001) // aDist < bDist
                    return -1;
                if (aDist - bDist > 0.001) // aDist > bDist
                    return 1;
                return 0;
            };


        Vertex LeastVertex(Polygon p, Func<Vertex, Vertex, int> cmp)
        {
            Vertex res = p.V;
            p.Advance(Rotation.Clockwise);
            for (int i = 1; i < p.Size; ++i)
            {
                if (cmp(p.V, res) < 0)
                    res = p.V;
            }

            p.SetV(res);
            return res;
        }

        bool PointInConvexPolygon(PointF s, Polygon p)
        {
            if (p.Size == 1)
                return (Math.Abs(s.X - p.Point.X) < 0.0001) && (Math.Abs(s.Y - p.Point.Y) < 0.0001);
            if (p.Size == 2)
            {
                PointToEdgeRelation rel = s.Classify(p.Point, p.Next.Point);
                return rel == PointToEdgeRelation.Between ||
                       rel == PointToEdgeRelation.Origin ||
                       rel == PointToEdgeRelation.Destination;
            }

            Vertex start = p.V;
            for (int i = 0; i < p.Size; ++i)
            {
                if (s.Classify(p.Point, p.Next.Point) == PointToEdgeRelation.Left)
                {
                    p.SetV(start);
                    return false;
                }
                p.Advance(Rotation.Clockwise);
            }
            return true;
        }
    }
}
