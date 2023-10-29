using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public class intPoint
    {
        public int x; public int y;
        public bool isRemoved;

        private intPoint?[] constraints;

        public enum Constraints
        {
            Horizontal = 0, Vertical = 1
        };


        public intPoint()
        {
            x = 0;
            y = 0;
            isRemoved = false;
            constraints = new intPoint?[2];
            constraints[0] = constraints[1] = null;
        }

        public intPoint(int xx, int yy) : this()
        {
            x = xx;
            y = yy;
        }

        public static intPoint middlePoint(intPoint p1, intPoint p2)
        {
            return new intPoint((p1.x + p2.x) / 2, (p1.y + p2.y) / 2);
        }

        public double Distance(intPoint other)
        {
            return Math.Sqrt(Math.Pow(x - other.x, 2) + Math.Pow(y - other.y, 2));
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
               (minX.x <= x && x <= maxX.x && minY.y <= y && y <= maxY.y) ||
               // If the line is vertical, is Y coordinate close
               (Math.Abs(p2.x - p1.x) < NeoGebra.eps && minY.y <= y && y <= maxY.y) ||
               // If horizontal, is X coordinate close
               (Math.Abs(p2.y - p1.y) < NeoGebra.eps && minX.x <= x && x <= maxX.x)
               ))
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

                if (IsRightTurn(min, max, this))
                    above++;

            }

            return above != 0 && above % 2 != 0;
        }

        public static bool IsRightTurn(intPoint P, intPoint Q, intPoint R)
        {
            return (Q.y - P.y) * (R.x - Q.x) -
                (Q.x - P.x) * (R.y - Q.y) < 0;
        }

        public static bool SetConstraint(intPoint P, intPoint Q, Constraints croissant)
        {
            if (P.constraints[(int)croissant] != null ||
                Q.constraints[(int)croissant] != null)
                return false; //throw?

            P.constraints[(int)croissant] = Q;
            Q.constraints[(int)croissant] = P;
            return true;
        }

        public bool SetConstraint(intPoint other, Constraints croissant)
        {
            return SetConstraint(this, other, croissant);
        }

        public static void DeleteConstraint(intPoint P, intPoint Q)
        {
            if (P.constraints[0] == Q)
            {
                P.constraints[0] = null;
                Q.constraints[0] = null;
            }
            else if (P.constraints[1] == Q)
            {
                P.constraints[1] = null;
                Q.constraints[1] = null;
            }
        }

        public static void DeleteConstraint((intPoint p1, intPoint p2) line)
        {
            DeleteConstraint(line.p1, line.p2);
        }

        public void DeleteConstraint(intPoint other)
        {
            DeleteConstraint(this, other);
        }

        public bool IsConstrained()
        {
            return constraints[0] != null || constraints[1] != null;
        }

        public bool IsConstrainedHorizontally()
        {
            return constraints[(int)Constraints.Horizontal] != null;
        }

        public bool IsConstrainedVertically()
        {
            return constraints[(int)Constraints.Vertical] != null;
        }

        public intPoint? GetHorizontallyConstrainedNeighbor()
        {
            return constraints[(int)Constraints.Horizontal];
        }

        public intPoint? GetVerticallyConstrainedNeighbor()
        {
            return constraints[(int)Constraints.Vertical];
        }

        public static bool IsNeighborEdgeConstrainedHorizontally
            ((intPoint p1, intPoint p2) line)
        {
            if (line.p1.constraints[(int)Constraints.Horizontal] == line.p2)
                return false;

            if (line.p1.constraints[(int)Constraints.Horizontal] != null)
                return true;

            if (line.p2.constraints[(int)Constraints.Horizontal] != null)
                return true;

            return false;
        }
        
        public static bool IsNeighborEdgeConstrainedVertically
            ((intPoint p1, intPoint p2) line)
        {
            if (line.p1.constraints[(int)Constraints.Vertical] == line.p2)
                return false;

            if (line.p1.constraints[(int)Constraints.Vertical] != null)
                return true;

            if (line.p2.constraints[(int)Constraints.Vertical] != null)
                return true;

            return false;
        }

        public static char? GetConstraintChar(intPoint p1, intPoint p2)
        {
            if (p1.constraints[(int)Constraints.Vertical] == p2)
                return 'V';
            if (p1.constraints[(int)Constraints.Horizontal] == p2)
                return 'H';

            return null;
        }

        public void Farewell()
        {
            if (constraints[0] != null)
            {
                constraints[0].constraints[0] = null;
                constraints[0] = null;
            }
            if (constraints[1] != null)
            {
                constraints[1].constraints[1] = null;
                constraints[1] = null;
            }
        }
    }
}
