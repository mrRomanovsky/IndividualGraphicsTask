using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
            return this.Prev == this? null : this.Prev;
        }

        public Vertex Neighbour(Rotation rotation)
        {
            return rotation == Rotation.Clockwise?
                Next :
                Prev;
        }

        public Vertex Split(Vertex b)
        {
            Vertex bp = b.Prev.Insert(new Vertex(b.Point));
            Insert(new Vertex(Point));
            Splice(bp);
            return bp;
        }

        public void Splice(Vertex b)
        {
            Vertex a = this;
            Vertex aNext = a.Next;
            Vertex bNext = b.Next;
            a.Next = bNext;
            b.Next = aNext;
            aNext.Prev = b;
            bNext.Prev = a;
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
