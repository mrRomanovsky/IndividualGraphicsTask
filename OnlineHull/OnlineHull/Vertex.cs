using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace OnlineHull
{

    public class Vertex
    {
        public Vertex(double x, double y)
        {
            this.Point = new PointF((float)x, (float)y);
            this.Next = null;
            this.Prev = null;
        }

        public Vertex(PointF p)
        {
            this.Point = p;
            this.Next = this;
            this.Prev = this;
        }

        public Vertex Insert(Vertex other)
        {
            other.Prev = this;
            other.Next = this.Next;
            this.Next.Prev = other;
            this.Next = other;
            if (this.Prev == this)
                this.Prev = other;
            return this.Next;
        }

        public Vertex Remove()
        {
            if (this.Prev != null)
                this.Prev.Next = this.Next;
            if (this.Next != null)
                this.Next.Prev = this.Prev;
            return this.Prev == this? null : this.Next;
        }

        public Vertex Neighbour(Rotation rotation)
        {
            return rotation == Rotation.Clockwise?
                Next :
                Prev;
        }

        public Vertex Split(Vertex b)
        {
            while (true)
            {
                Vertex v = Neighbour(Rotation.Counterclockwise);
                if (v != b)
                    v.Remove();
                else
                    break;
            }

            return this;
        }

        public PointF Point { get; private set; }
        public Vertex Next { get; private set; }
        public Vertex Prev { get; private set; }
        private double x;
        private double y;

    }

    public enum Rotation
    {
        Clockwise,
        Counterclockwise
    };
}
