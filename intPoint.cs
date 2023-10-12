using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public class intPoint
    {
        public int x; public int y;
        public bool isRemoved;

        public intPoint() { x = 0; y = 0; }
        public intPoint(int xx, int yy)
        {
            x = xx;
            y = yy;
            isRemoved = false;
        }

        public static intPoint middlePoint(intPoint p1, intPoint p2)
        {
            return new intPoint((p1.x + p2.x) / 2, (p1.y + p2.y) / 2);
        }

        public bool IsCloseToPoint(intPoint other)
        {
            return Math.Abs(this.x - other.x) < NeoGebra.eps &&
                Math.Abs(this.y - other.y) < NeoGebra.eps;
        }

        public bool IsCloseToLine(intPoint p1, intPoint p2)
        {
            intPoint minX = p1.x < p2.x ? p1 : p2;
            intPoint maxX = p1.x < p2.x ? p2 : p1;
            intPoint minY = p1.y < p2.y ? p1 : p2;
            intPoint maxY = p1.y < p2.y ? p2 : p1;

            // Check if point has the chance to be close to a segment
            if (!(
               // Is the point inside the segment's bounding rectangle 
               minX.x <= x && x <= maxX.x && minY.y <= y && y <= maxY.y) ||
               // If the line is vertical, is Y coordinate close
               (Math.Abs(p2.x - p1.x) < NeoGebra.eps && Math.Abs(p1.y - y) < NeoGebra.eps && Math.Abs(p2.y - y) < NeoGebra.eps) ||
               // If horizontal, is X coordinate close
               (Math.Abs(p2.y - p1.y) < NeoGebra.eps && Math.Abs(p1.x - x) < NeoGebra.eps && Math.Abs(p2.x - x) < NeoGebra.eps)
               )
                return false;

            // Calculate distance
            double dist = Math.Abs((p2.x - p1.x) * (p1.y - y) - (p1.x - x) * (p2.y - p1.y)) /
                 Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2));

            return dist < NeoGebra.eps;
        }

        public bool IsInsidePolygon(Polygon poly)
        {
            if (poly == null) return false;

            intPoint prevVertex = poly.vertices.Last();
            int above = 0;

            foreach (intPoint vertex in poly.vertices)
            {
                intPoint min = prevVertex.x < vertex.x ? prevVertex : vertex;
                intPoint max = prevVertex.x < vertex.x ? vertex : prevVertex;
                prevVertex = vertex;

                if (min.x > x || x > max.x)
                    continue;

                if (IsRightTurn(min, max))
                    above++;

            }

            return above != 0 && above % 2 != 0;
        }

        public bool IsRightTurn(intPoint P, intPoint Q)
        {
            return (Q.y - P.y) * (x - Q.x) -
                (Q.x - P.x) * (y - Q.y) < 0;
        }
    }
}
