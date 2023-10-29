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
        public List<Vertex> vertices;
        private List<Vertex> offsetVertices;
        private Dictionary<int, HashSet<(int idx, Vertex v)>> intersections;
        private GraphicsPath? path;

        public Polygon(List<Vertex> vertices)
        {
            this.vertices = new List<Vertex>();
            this.vertices.AddRange(vertices);
            path = null;
        }

        public GraphicsPath GetGraphicsPath()
        {
            // optimization possible

            Point[] points = new Point[vertices.Count];
            byte[] types = new byte[vertices.Count];
            int it = 0;
            foreach (Vertex point in vertices)
            {
                points[it] = new Point(point.X, point.Y);
                types[it] = 1;
                it++;
            }

            path = new GraphicsPath(points, types);//, FillMode.Winding);
            return path;
        }

        public Polygon CalculateOffsetPolygon(int offset)
        {
            offsetVertices = new List<Vertex>();
            int n = vertices.Count;
            int outer_ccw = GetClockwiseness();

            // Find offset vertices
            for (int i = 0; i < n; ++i)
            {
                int prev = (i + n - 1) % n;
                int next = (i + 1) % n;

                double vnX = vertices[next].X - vertices[i].X;
                double vnY = vertices[next].Y - vertices[i].Y;
                Vector2 vnn = Vector2.Normalize(new Vector2((float)vnX, (float)vnY));
                double nnnX = vnn.Y;
                double nnnY = -vnn.X;

                double vpX = vertices[i].X - vertices[prev].X;
                double vpY = vertices[i].Y - vertices[prev].Y;
                Vector2 vpn = Vector2.Normalize(new Vector2((float)vpX, (float)vpY));
                double npnX = vpn.Y;
                double npnY = -vpn.X;

                double bisX = (nnnX + npnX) * outer_ccw;
                double bisY = (nnnY + npnY) * outer_ccw;

                Vector2 bisn = Vector2.Normalize(new Vector2((float)bisX, (float)bisY));
                double bislen = offset / (double)Math.Sqrt((1 + nnnX * npnX + nnnY * npnY) / 2);

                int threshold = (int)1e6;
                int x = (int)(vertices[i].X + bislen * bisn.X);
                if (x >= int.MaxValue - threshold || x <= int.MinValue + threshold)
                    x /= 8;
                int y = (int)(vertices[i].Y + bislen * bisn.Y);
                if (y >= int.MaxValue - threshold || y <= int.MinValue + threshold)
                    y /= 8;

                offsetVertices.Add(new Vertex(x, y));
            }

            GetIntersections(offsetVertices);
            var ret = new List<Vertex>();

            // Find vertices of the offset polygon
            int index = 0;
            Vertex p = offsetVertices[index];
            Vertex first = p;
            ret.Add(p);

            bool goToNextNotIntersection = false;

            while (p != first || ret.Count < 2)
            {
                goToNextNotIntersection = false;
                if (intersections.ContainsKey(index))
                {
                    var filtered = intersections[index].
                        Where(val => p.Distance(offsetVertices[index]) < val.v.Distance(offsetVertices[index]));
                    if (filtered.Any())
                    {
                        (int newIndex, Vertex newP) = filtered.MinBy(val => p.Distance(val.v));
                        if (newP != p)
                            (index, p) = (newIndex, newP);
                        else
                            goToNextNotIntersection = true;
                    }
                    else
                        goToNextNotIntersection = true;
                }
                else
                    goToNextNotIntersection = true;

                if (goToNextNotIntersection)
                {
                    index = (index + 1) % n;
                    p = offsetVertices[index];
                }

                ret.Add(p);
            }

            return new Polygon(ret);
        }


        private List<Vertex> GetIntersections(List<Vertex> points)
        {
            List<Vertex> ret = new();
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

                    Vertex? intersection = GetIntersection(iSegment, jSegment);
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

        private Vertex? GetIntersection((Vertex p1, Vertex p2) seg1, (Vertex p1, Vertex p2) seg2)
        {
            double x1 = seg1.p1.X;
            double y1 = seg1.p1.Y;
            double x2 = seg1.p2.X;
            double y2 = seg1.p2.Y;

            double x3 = seg2.p1.X;
            double y3 = seg2.p1.Y;
            double x4 = seg2.p2.X;
            double y4 = seg2.p2.Y;

            double denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            if (Math.Abs(denominator) < double.Epsilon)
                return null;

            double px = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / denominator;
            double py = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / denominator;

            if (px >= Math.Min(x1, x2) && px <= Math.Max(x1, x2) && px >= Math.Min(x3, x4) && px <= Math.Max(x3, x4) &&
                py >= Math.Min(y1, y2) && py <= Math.Max(y1, y2) && py >= Math.Min(y3, y4) && py <= Math.Max(y3, y4) &&
                (px, py) != (x1, y1) && (px, py) != (x2, y2) && (px, py) != (x3, y3) && (px, py) != (x4, y4))
            {
                return new Vertex(px, py);
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

                if (Vertex.IsRightTurn(vertices[prev], vertices[i], vertices[next]))
                    rightTurns++;
                else
                    leftTurns++;
            }

            return rightTurns >= leftTurns ? 1 : -1;
        }
    }
}
