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
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        //List<PointF> points = new List<PointF>();
        Pen bluePen = new Pen(Color.Blue);
        private Polygon p = new Polygon();

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //points.Add(e.Location);
            InsertionHull(e.Location);
            DrawPolygon();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            //points.Clear();
            p = new Polygon();
        }

        void DrawPolygon()
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Image.Width, pictureBox1.Image.Height);
            Vertex curr = p.V;
            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
            {
                /*foreach (var point in points)
                    g.DrawRectangle(bluePen, point.X, point.Y, 1, 1);*/
                for (int i = 0; i < p.Size; ++i)
                {
                    g.DrawRectangle(bluePen, curr.Point.X, curr.Point.Y, 1, 1);
                    g.DrawLine(bluePen, curr.Point, curr.Next.Point);
                    curr = curr.Next;
                }
            }
        }

        void InsertionHull(PointF point)//List<PointF> points)
        {
            if (p.Size == 0)
                p.Insert(point);
            else if (!PointInConvexPolygon(point, p))
            {
                LeastVertex(p, ClosestToPolygon(point));
                SupportingLine(point, p, PointToEdgeRelation.Left);
                Vertex l = p.V;
                SupportingLine(point, p, PointToEdgeRelation.Right);
                p.Split(l);
                p.SetV(l);
                p.Insert(point);
            }
        }

        void SupportingLine(PointF s, Polygon p, PointToEdgeRelation side)
        {
            Rotation rotation = side == PointToEdgeRelation.Right ? Rotation.Clockwise : Rotation.Counterclockwise;
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
                if (aDist < bDist)
                    return -1;
                if (aDist > bDist)
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
                p.Advance(Rotation.Clockwise);
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
            int intersectsCnt = 0;
            for (int i = 0; i < p.Size; ++i)
            {
                var classification = s.Classify(p.Point, p.Next.Point);
                if (classification == PointToEdgeRelation.Destination || classification == PointToEdgeRelation.Origin
                                                                      || classification == PointToEdgeRelation.Between)
                {
                    p.SetV(start);
                    return true;
                }
                if (LinesIntersect(p.Point, p.Next.Point, s, new PointF(pictureBox1.Width - 1, s.Y)))
                    ++intersectsCnt;
                p.Advance(Rotation.Clockwise);
            }

            p.SetV(start);
            return intersectsCnt % 2 != 0;
        }

        bool LinesIntersect(PointF a, PointF b, PointF c, PointF d)
        {
            return PointsOnDifferentSides(a, b, c, d) && PointsOnDifferentSides(c, d, a, b);
        }

        bool PointsOnDifferentSides(PointF a, PointF b, PointF c, PointF d)
        {
            return c.Classify(a, b) != d.Classify(a, b);
        }
    }
}
