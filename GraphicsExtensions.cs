using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace lab1
{
    public static class GraphicsExtensions
    {
        private static int pointWidth = 2;
        private static int highlightPointWidth = pointWidth + NeoGebra.eps;
        private static int lineWidth = 2;
        private static int highlightLineWidth = lineWidth + NeoGebra.eps;

        private static Pen pointPen = new Pen(Color.LightSlateGray, pointWidth);
        private static Pen highlightPointPen = new Pen(Color.FromArgb(127, 255, 235, 205), highlightPointWidth);
        private static Pen linePen = new Pen(Color.PeachPuff, lineWidth);
        private static Pen highlightLinePen = new Pen(Color.FromArgb(127, 255, 235, 205), highlightLineWidth);
        private static SolidBrush polygonBrush = new SolidBrush(Color.FromArgb(127, 216, 232, 242));

        public static void DrawPoint(this Graphics g, intPoint p)
        {
            if (NeoGebra.MousePos.IsCloseToPoint(p))
                g.DrawEllipse(highlightPointPen,
                    p.x - highlightPointWidth / 2,
                    p.y - highlightPointWidth / 2,
                    highlightPointWidth, highlightPointWidth);

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
                g.DrawLine(highlightLinePen, P1, P2);

            g.DrawLine(linePen, P1, P2);
        }
        public static void FillPolygon(this Graphics g, Polygon poly)
        {
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
