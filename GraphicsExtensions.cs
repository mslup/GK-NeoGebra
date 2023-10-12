using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace lab1
{
    public static class GraphicsExtensions
    {
        private static int pointWidth = 2;
        private static int hlPointWidth = pointWidth + NeoGebra.eps;
        private static int lineWidth = 2;
        private static int hlLineWidth = lineWidth + NeoGebra.eps;
        private static int iconWidth1 = 10;
        private static int iconWidth2 = iconWidth1 - 2;

        private static Pen pointPen = new Pen(Color.LightSlateGray, pointWidth);
        private static Pen hlPointPen = new Pen(Color.FromArgb(127, 255, 235, 205), hlPointWidth);
        private static Pen linePen = new Pen(Color.PeachPuff, lineWidth);
        private static Pen hlLinePen = new Pen(Color.FromArgb(127, 255, 235, 205), hlLineWidth);
        private static SolidBrush polygonBrush = new SolidBrush(Color.FromArgb(127, 216, 232, 242));
        private static SolidBrush hlPolygonBrush = new SolidBrush(Color.FromArgb(70, 216, 232, 242));
        private static Pen iconPen1 = new Pen(Color.FromArgb(200, 200, 200), iconWidth1);
        private static Pen iconPen2 = new Pen(Color.FromArgb(240, 240, 240), iconWidth2);
        private static SolidBrush iconBrush = new SolidBrush(Color.FromArgb(94, 143, 143));
        private static Font iconFont = new Font("Arial Black", iconWidth1);

        public static void DrawPoint(this Graphics g, intPoint p)
        {
            if (NeoGebra.MousePos.IsCloseToPoint(p))
                g.DrawEllipse(hlPointPen,
                    p.x - hlPointWidth / 2,
                    p.y - hlPointWidth / 2,
                    hlPointWidth, hlPointWidth);

            g.DrawEllipse(pointPen,
                p.x - pointWidth / 2,
                p.y - pointWidth / 2,
                pointWidth, pointWidth);

            //g.DrawString($"{p.x}, {p.y}",
            //    new Font("Arial", 10),
            //    new SolidBrush(Color.Black),
            //    new Point(p.x + 5, p.y + 5));
        }

        public static void DrawLine(this Graphics g, intPoint p1, intPoint p2)
        {
            Point P1 = new Point(p1.x, p1.y);
            Point P2 = new Point(p2.x, p2.y);

            if (NeoGebra.MousePos.IsCloseToLine(p1, p2))
                g.DrawLine(hlLinePen, P1, P2);

            g.DrawLine(linePen, P1, P2);

            intPoint min, max;
            if (p1.x == p2.x)
            {
                min = p1.y < p2.y ? p1 : p2;
                max = p1.y < p2.y ? p2 : p1;
            }
            else if (p1.y == p2.y)
            {
                min = p1.x < p2.x ? p1 : p2;
                max = p1.x < p2.x ? p2 : p1;
            }
            else
                return;

            // change?
            // mozna w sumie nie przechowywac typu constraint,
            // tylko sam fakt istnienia
            char constraint;
            if (NeoGebra.EdgeConstraints.
                TryGetValue((min, max), out constraint))
            {
                intPoint mid = intPoint.middlePoint(p1, p2);
                g.DrawConstraintIcon(mid, constraint);
            }
        }

        private static void DrawConstraintIcon(this Graphics g, intPoint p, char constraint)
        {
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.DrawEllipse(iconPen1,
                p.x - iconWidth1 / 2,
                p.y - iconWidth1 / 2,
                iconWidth1, iconWidth1);
            g.DrawEllipse(iconPen2,
                p.x - iconWidth2 / 2,
                p.y - iconWidth2 / 2,
                iconWidth2, iconWidth2);
            g.DrawString($"{constraint}", iconFont, 
                iconBrush, new PointF(p.x - iconWidth1 / 2 - 2, p.y - iconWidth1 / 2 - 4));
        }

        public static void FillPolygon(this Graphics g, Polygon poly)
        {
            if (NeoGebra.MousePos.IsInsidePolygon(poly))
                g.FillPath(hlPolygonBrush, poly.GetGraphicsPath());
            else
                g.FillPath(polygonBrush, poly.GetGraphicsPath());

        }

        public static void DrawPolygons(this Graphics g, List<Polygon> Polygons)
        {
            bool start = true;
            intPoint lastPoint = new intPoint();

            foreach (Polygon poly in Polygons)
            {
                g.FillPolygon(poly);

                foreach (intPoint p in poly.vertices)
                {
                    if (!start)
                    {
                        g.DrawLine(lastPoint, p);
                        g.DrawPoint(lastPoint);
                    }
                    start = false;
                    lastPoint = p;
                }

                g.DrawLine(lastPoint, poly.vertices.First());
                g.DrawPoint(poly.vertices.First());
                g.DrawPoint(lastPoint);
                start = true;
            }


        }
        public static void DrawPolygonInProgress(this Graphics g, List<intPoint> Points)
        {
            bool start = true;
            intPoint lastPoint = new intPoint();

            foreach (intPoint p in Points)
            {
                if (!start)
                {
                    g.DrawLine(lastPoint, p);
                    g.DrawPoint(lastPoint);
                }
                start = false;

                lastPoint = p;
            }
            if (Points != null && Points.Count > 0)
                g.DrawPoint(lastPoint);
        }

    }
}
