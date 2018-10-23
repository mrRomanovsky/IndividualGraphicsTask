using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineHull
{
    public class Polygon
    {
        public Polygon()
        {
            Size = 0;
            V = null;
        }

        public Polygon(Vertex v)
        {
            V = v;
            Resize();
        }

        public PointF Point
        {
            get { return V.Point; }
        }

        public Vertex Next
        {
            get { return V.Next; }
        }

        public Vertex Prev
        {
            get { return V.Prev; }
        }

        public Vertex Neighbour(Rotation rotation)
        {
            return V.Neighbour(rotation);
        }

        public Vertex Advance(Rotation rotation)
        {
            V = V.Neighbour(rotation);
            return V;
        }

        public Vertex SetV(Vertex v)
        {
            V = v;
            return V;
        }

        public Vertex Insert(PointF p)
        {
            if (Size == 0)
                V = new Vertex(p);
            else
                V.Insert(new Vertex(p));
            ++Size;
            return V;
        }

        public void Remove()
        {
            V.Remove();
            --Size;
        }

        public Vertex V { get; private set; }

        public int Size{ get; private set; }

        public Polygon Split(Vertex b)
        {
            Vertex bp = V.Split(b);
            Resize();
            return new Polygon(bp);
        }

        private void Resize()
        {
            /*
            if (V == null)
                Size = 0;
            else
            {
                
                Vertex curr = V;
                while (curr != V)
                {
                    ++Size;
                    curr = curr.Next;
                }
            }*/
            Size = 0;
            if (V != null)
            {
                Vertex curr = V.Next;
                ++Size;
                while (curr != V)
                {
                    ++Size;
                    curr = curr.Next;
                }
            }
        }
    }
}
