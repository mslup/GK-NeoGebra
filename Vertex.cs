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
    public class Vertex
    {
        public double x; public double y;
        public int X
        {
            get => (int)x; set => x = value;
        }
        public int Y
        {
            get => (int)y; set => y = value;
        }

        //public bool isRemoved;
        private Vertex?[] constraints;

        public enum Constraints
        {
            Horizontal = 0, Vertical = 1
        };


        public Vertex()
        {
            X = 0;
            Y = 0;
            //isRemoved = false;
            constraints = new Vertex?[2];
            constraints[0] = constraints[1] = null;
        }

        public Vertex(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public Vertex(double x, double y) : this()
        {
            this.x = x;
            this.y = y;
        }

        public static Vertex middlePoint(Vertex p1, Vertex p2)
        {
            return new Vertex((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }

        public double Distance(Vertex other)
        {
            return Math.Sqrt(Math.Pow(x - other.x, 2) + Math.Pow(y - other.y, 2));
        }

        public bool IsCloseToPoint(Vertex other)
        {
            return Math.Abs(this.X - other.X) < NeoGebra.eps &&
                Math.Abs(this.Y - other.Y) < NeoGebra.eps;
        }

        public bool IsCloseToLine(Vertex p1, Vertex p2)
        {
            Vertex minX = p1.X < p2.X ? p1 : p2;
            Vertex maxX = p1.X < p2.X ? p2 : p1;
            Vertex minY = p1.Y < p2.Y ? p1 : p2;
            Vertex maxY = p1.Y < p2.Y ? p2 : p1;

            // Check if point has the chance to be close to a segment
            if (!(
               // Is the point inside the segment's bounding rectangle 
               (minX.X <= X && X <= maxX.X && minY.Y <= Y && Y <= maxY.Y) ||
               // If the line is vertical, is Y coordinate close
               (Math.Abs(p2.X - p1.X) < NeoGebra.eps && minY.Y <= Y && Y <= maxY.Y) ||
               // If horizontal, is X coordinate close
               (Math.Abs(p2.Y - p1.Y) < NeoGebra.eps && minX.X <= X && X <= maxX.X)
               ))
                return false;

            // Calculate distance
            double dist = Math.Abs((p2.X - p1.X) * (p1.Y - Y) - (p1.X - X) * (p2.Y - p1.Y)) /
                 Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));

            return dist < NeoGebra.eps;
        }

        public bool IsInsidePolygon(Polygon poly)
        {
            if (poly == null) return false;

            Vertex prevVertex = poly.vertices.Last();
            int above = 0;

            foreach (Vertex vertex in poly.vertices)
            {
                Vertex min = prevVertex.X < vertex.X ? prevVertex : vertex;
                Vertex max = prevVertex.X < vertex.X ? vertex : prevVertex;
                prevVertex = vertex;

                if (min.X >= X || X > max.X)
                    continue;

                if (IsRightTurn(min, max, this))
                    above++;

            }

            return above != 0 && above % 2 != 0;
        }

        public static bool IsRightTurn(Vertex P, Vertex Q, Vertex R)
        {
            return (Q.Y - P.Y) * (R.X - Q.X) -
                (Q.X - P.X) * (R.Y - Q.Y) < 0;
        }

        public static bool SetConstraint(Vertex P, Vertex Q, Constraints croissant)
        {
            if (P.constraints[(int)croissant] != null ||
                Q.constraints[(int)croissant] != null)
                return false; //throw?

            P.constraints[(int)croissant] = Q;
            Q.constraints[(int)croissant] = P;
            return true;
        }

        public bool SetConstraint(Vertex other, Constraints croissant)
        {
            return SetConstraint(this, other, croissant);
        }

        public static void DeleteConstraint(Vertex P, Vertex Q)
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

        public static void DeleteConstraint((Vertex p1, Vertex p2) line)
        {
            DeleteConstraint(line.p1, line.p2);
        }

        public void DeleteConstraint(Vertex other)
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

        public Vertex? GetHorizontallyConstrainedNeighbor()
        {
            return constraints[(int)Constraints.Horizontal];
        }

        public Vertex? GetVerticallyConstrainedNeighbor()
        {
            return constraints[(int)Constraints.Vertical];
        }

        public static bool IsNeighborEdgeConstrainedHorizontally
            ((Vertex p1, Vertex p2) line)
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
            ((Vertex p1, Vertex p2) line)
        {
            if (line.p1.constraints[(int)Constraints.Vertical] == line.p2)
                return false;

            if (line.p1.constraints[(int)Constraints.Vertical] != null)
                return true;

            if (line.p2.constraints[(int)Constraints.Vertical] != null)
                return true;

            return false;
        }

        public static char? GetConstraintChar(Vertex p1, Vertex p2)
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

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}
