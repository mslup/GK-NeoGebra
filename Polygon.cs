using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Security.Policy;

namespace lab1
{
    public class Polygon
    {
        public List<intPoint> vertices;
        private GraphicsPath? path;

        public Polygon(List<intPoint> vertices)
        {
            this.vertices = new List<intPoint>();
            this.vertices.AddRange(vertices);
            path = null;
        }

        public GraphicsPath GetGraphicsPath()
        {   
            // optimization possible

            Point[] points = new Point[vertices.Count];
            byte[] types = new byte[vertices.Count];
            int it = 0;
            foreach (intPoint point in vertices) 
            {
                points[it] = new Point(point.x, point.y);
                types[it] = 1;
                it++;
            }

            path = new GraphicsPath(points, types);//, FillMode.Winding);
            return path;
        }
    }
}
