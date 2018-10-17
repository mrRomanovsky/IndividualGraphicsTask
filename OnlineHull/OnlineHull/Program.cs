using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineHull
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

/*
 * bool pointlnConvexPolygon (Point &s , Polygon &p)
{
  if (p. size() == 1)
    return (s == p.point());
  if (p. size () == 2) {
    int c = s .classify (p. edge ());
    return ( (c==BETWEEN) || (c==ORIGIN) | || (c==DESTINATION) );
  }
  Vertex *org = p.v();
  for (int i = 0; i < p.size(); i++, p.advance(CLOCKWISE))
    if (s.classify (p.edge()) == LEFT) {
      p.setV(org);
      return FALSE;
    }
  return TRUE;
}
 */
