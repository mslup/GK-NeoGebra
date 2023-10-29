using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Security.Policy;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using System.Diagnostics.Eventing.Reader;
using System.Xml;

namespace lab1
{
    public class Polygon
    {
        public List<intPoint> vertices;
        private List<intPoint> offsetVertices;
        private Dictionary<intPoint, intPoint> nexts;
        private Dictionary<int, HashSet<(int, intPoint)>> intersections;
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

        public (Polygon, List<intPoint>) CalculateOffsetPolygon(int offset)
        {
            offsetVertices = new List<intPoint>();
            int n = vertices.Count;
            int outer_ccw = GetClockwiseness();

            // Find offset vertices
            for (int i = 0; i < n; ++i)
            {
                int prev = (i + n - 1) % n;
                int next = (i + 1) % n;

                double vnX = vertices[next].x - vertices[i].x;
                double vnY = vertices[next].y - vertices[i].y;
                Vector2 vnn = Vector2.Normalize(new Vector2((float)vnX, (float)vnY));
                double nnnX = vnn.Y;
                double nnnY = -vnn.X;

                double vpX = vertices[i].x - vertices[prev].x;
                double vpY = vertices[i].y - vertices[prev].y;
                Vector2 vpn = Vector2.Normalize(new Vector2((float)vpX, (float)vpY));
                double npnX = vpn.Y;
                double npnY = -vpn.X;

                double bisX = (nnnX + npnX) * outer_ccw;
                double bisY = (nnnY + npnY) * outer_ccw;

                Vector2 bisn = Vector2.Normalize(new Vector2((float)bisX, (float)bisY));
                double bislen = offset / (double)Math.Sqrt((1 + nnnX * npnX + nnnY * npnY) / 2);

                int threshold = (int)1e6;
                int x = (int)(vertices[i].x + bislen * bisn.X);
                if (x >= int.MaxValue - threshold || x <= int.MinValue + threshold)
                    x /= 8;
                int y = (int)(vertices[i].y + bislen * bisn.Y);
                if (y >= int.MaxValue - threshold || y <= int.MinValue + threshold)
                    y /= 8;

                offsetVertices.Add(new intPoint(x, y));
            }

            var inters = GetIntersections(offsetVertices);
            var ret = new List<intPoint>();

            int index = 0;
            intPoint p = offsetVertices[index];
            intPoint first = p;
            ret.Add(p);

            while (p != first || ret.Count < 2)
            {
                if (intersections.ContainsKey(index))
                {
                    (int newIndex, intPoint newP) = intersections[index].MinBy(((int idx, intPoint pp) val) => p.Distance(val.pp));
                    if (newP != p)
                        (index, p) = (newIndex, newP);
                    else
                    {
                        index = (index + 1) % n;
                        p = offsetVertices[index];
                    }
                }
                else
                {
                    index = (index + 1) % n;
                    p = offsetVertices[index];
                }
                ret.Add(p);
            }

            return (new Polygon(ret), new());
        }


        private List<intPoint> GetIntersections(List<intPoint> points)
        {
            List<intPoint> ret = new();
            intersections = new();

            int n = points.Count;
            for (int i = 0; i < n; ++i)
            {
                int iNext = (i + 1) % n;
                var iSegment = (points[i], points[iNext]);
                for (int j = 0; j < n; ++j)
                {
                    int jNext = (j + 1) % n;
                    var jSegment = (points[j], points[jNext]);

                    //if (i == j || i == jNext || j == iNext)
                    //    continue;

                    intPoint? intersection = GetIntersection(iSegment, jSegment);
                    if (intersection != null)
                    {
                        if (!intersections.ContainsKey(i))
                            intersections[i] = new();
                        if (!intersections.ContainsKey(j))
                            intersections[j] = new();

                        intersections[i].Add((j, intersection));
                        intersections[j].Add((i, intersection));

                        ret.Add(intersection);
                    }
                }
            }

            return ret;
        }

        private intPoint? GetIntersection((intPoint p1, intPoint p2) seg1, (intPoint p1, intPoint p2) seg2)
        {
            double x1 = seg1.p1.x;
            double y1 = seg1.p1.y;
            double x2 = seg1.p2.x;
            double y2 = seg1.p2.y;

            double x3 = seg2.p1.x;
            double y3 = seg2.p1.y;
            double x4 = seg2.p2.x;
            double y4 = seg2.p2.y;

            double denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            if (Math.Abs(denominator) < double.Epsilon)
                return null;

            double px = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / denominator;
            double py = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / denominator;

            if (px >= Math.Min(x1, x2) && px <= Math.Max(x1, x2) && px >= Math.Min(x3, x4) && px <= Math.Max(x3, x4) &&
                py >= Math.Min(y1, y2) && py <= Math.Max(y1, y2) && py >= Math.Min(y3, y4) && py <= Math.Max(y3, y4) &&
                (px, py) != (x1, y1) && (px, py) != (x2, y2) && (px, py) != (x3, y3) && (px, py) != (x4, y4))
            {
                return new intPoint((int)px, (int)py);
            }

            return null;
        }

        private int GetClockwiseness()
        {
            int n = vertices.Count;
            int rightTurns = 0;
            int leftTurns = 0;

            for (int i = 0; i < n; ++i)
            {
                int prev = (i + n - 1) % n;
                int next = (i + 1) % n;

                if (intPoint.IsRightTurn(vertices[prev], vertices[i], vertices[next]))
                    rightTurns++;
                else
                    leftTurns++;
            }

            return rightTurns >= leftTurns ? 1 : -1;
        }
    }
}
