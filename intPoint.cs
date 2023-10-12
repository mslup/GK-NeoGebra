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

        public bool IsCloseToPoint(intPoint other)
        {
            return Math.Abs(this.x - other.x) < NeoGebra.eps &&
                Math.Abs(this.y - other.y) < NeoGebra.eps;
        }

        public bool IsCloseToLine(intPoint p1, intPoint p2)
        {
            double a = Math.Abs((p2.x - p1.x) * (p1.y - y) - (p1.x - x) * (p2.y - p1.y));
            double b = Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2));

            return a / b < NeoGebra.eps &&
                ((p1.x <= x && x <= p2.x) || (p2.x <= x && x <= p1.x)) &&
                ((p1.y <= y && y <= p2.y) || (p2.y <= y && y <= p1.y));
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
