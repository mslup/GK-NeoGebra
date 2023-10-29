using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
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
        private static SolidBrush brhmlineBrush = new SolidBrush(Color.Black);
        private static Pen hlLinePen = new Pen(Color.FromArgb(127, 255, 235, 205), hlLineWidth);
        private static SolidBrush polygonBrush = new SolidBrush(Color.FromArgb(127, 216, 232, 242));
        private static SolidBrush hlPolygonBrush = new SolidBrush(Color.FromArgb(70, 216, 232, 242));
        private static Pen iconPen1 = new Pen(Color.FromArgb(200, 200, 200), iconWidth1);
        private static Pen iconPen2 = new Pen(Color.FromArgb(240, 240, 240), iconWidth2);
        private static SolidBrush iconBrush = new SolidBrush(Color.FromArgb(94, 143, 143));
        private static Font iconFont = new Font("Arial Black", iconWidth1);

        private static Pen debugPen = new Pen(Color.Red, pointWidth);
        private static SolidBrush debugBrush = new SolidBrush(Color.Red);

        public static void DrawPoint(this Graphics g, Vertex p, bool highlight = true)
        {
            //if (pen == null)
            var pen = pointPen;

            if (highlight && NeoGebra.MousePos.IsCloseToPoint(p))
                g.DrawEllipse(hlPointPen,
                    p.X - hlPointWidth / 2,
                    p.Y - hlPointWidth / 2,
                    hlPointWidth, hlPointWidth);

            g.DrawEllipse(pen,
                p.X - pointWidth / 2,
                p.Y - pointWidth / 2,
                pointWidth, pointWidth);

            if (NeoGebra.Debug)
                g.DrawString($"{p.X}, {p.Y}",
                    new Font("Arial", 10),
                    new SolidBrush(Color.Black),
                    new Point((int)p.X + 5, (int)p.Y + 5));
        }

        public static void DrawLine(this Graphics g, Vertex p1, Vertex p2, bool highlight = true)
        {
            Point P1 = new Point(p1.X, p1.Y);
            Point P2 = new Point(p2.X, p2.Y);

            if (highlight && NeoGebra.MousePos.IsCloseToLine(p1, p2))
                g.DrawLine(hlLinePen, P1, P2);

            switch (NeoGebra.linemode)
            {
            case NeoGebra.LineModes.WinForms:
                g.DrawLine(linePen, P1, P2);
                break;
            case NeoGebra.LineModes.Bresenham:
                g.DrawBresenham(brhmlineBrush, P1, P2);
                break;
            }

            char? constraint = Vertex.GetConstraintChar(p1, p2);
            if (constraint != null)
            {
                Vertex mid = Vertex.middlePoint(p1, p2);
                g.DrawConstraintIcon(mid, (char)constraint);
            }
        }

        private static void DrawBresenham(this Graphics g, SolidBrush brush, Point p1, Point p2)
        {
            int x1, x2, y1, y2;
            x1 = p1.X;
            y1 = p1.Y;
            x2 = p2.X;
            y2 = p2.Y;

            int dx = x2 - x1;
            int dy = y2 - y1;

            int sgndx = (dx == 0) ? 0 : (dx > 0) ? 1 : -1;
            int sgndy = (dy == 0) ? 0 : (dy > 0) ? 1 : -1;

            int h;
            int xDiag = 0;
            int yDiag = 0;

            if (sgndx * dx >= sgndy * dy)
            {
                h = 2;
                xDiag = 1;
            }
            else
            {
                h = 1;
                yDiag = 1;
            }

            int dStr = 2 * (dy * sgndx * (h - 1) - dx * sgndy * (2 - h));
            int dDiag = 2 * (dy * sgndx - dx * sgndy);

            int d = dy * sgndx * h - dx * sgndy * (3 - h);

            int x = x1;
            int y = y1;

            g.PutPixel(brush, x, y);
            while ((xDiag == 1 && ((x1 < x2 && x < x2) || (x1 > x2 && x > x2))) ||
                (yDiag == 1 && ((y1 < y2 && y < y2) || (y1 > y2 && y > y2))))
            {
                if ((xDiag == 1 && d * sgndx * sgndy < 0) || (yDiag == 1 && d * sgndx * sgndy > 0))
                {
                    d += dStr;
                    x += sgndx * xDiag;
                    y += sgndy * yDiag;
                }
                else
                {
                    d += dDiag;
                    x += sgndx;
                    y += sgndy;
                }
                g.PutPixel(brush, x, y);
            }
        }

        private static void PutPixel(this Graphics g, SolidBrush brush, int x, int y)
        {
            g.FillRectangle(brush, x, y, 1, 1);
        }

        private static void DrawConstraintIcon(this Graphics g, Vertex p, char constraint)
        {
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.DrawEllipse(iconPen1,
                p.X - iconWidth1 / 2,
                p.Y - iconWidth1 / 2,
                iconWidth1, iconWidth1);
            g.DrawEllipse(iconPen2,
                p.X - iconWidth2 / 2,
                p.Y - iconWidth2 / 2,
                iconWidth2, iconWidth2);
            g.DrawString($"{constraint}", iconFont,
                iconBrush, new PointF(p.X - iconWidth1 / 2 - 2, p.Y - iconWidth1 / 2 - 4));
        }

        public static void FillPolygon(this Graphics g, Polygon poly)
        {
            if (NeoGebra.MousePos.IsInsidePolygon(poly))
                g.FillPath(hlPolygonBrush, poly.GetGraphicsPath());
            else
                g.FillPath(polygonBrush, poly.GetGraphicsPath());

        }

        public static void DrawPolygons(this Graphics g, List<Polygon> Polygons, int offset)
        {
            foreach (Polygon poly in Polygons)
            {
                g.DrawOffsetPolygon(poly, offset);
                g.DrawPolygon(poly);
            }
        }

        private static void DrawOffsetPolygon(this Graphics g, Polygon poly, int offset)
        {
            if (offset == 0)
                return;

            var offsetPolygon = poly.CalculateOffsetPolygon(offset);

            g.DrawPolygon(offsetPolygon, false, false);
        }

        private static void DrawPolygon(this Graphics g, Polygon poly, bool fill = true, bool highlight = true)
        {
            Vertex lastPoint = new Vertex();
            if (fill)
                g.FillPolygon(poly);

            bool start = true;
            foreach (Vertex p in poly.vertices)
            {
                if (!start)
                {
                    g.DrawLine(lastPoint, p, highlight);
                    g.DrawPoint(lastPoint, highlight);
                }
                start = false;
                lastPoint = p;
            }

            g.DrawLine(lastPoint, poly.vertices.First(), highlight);
            g.DrawPoint(poly.vertices.First(), highlight);
            g.DrawPoint(lastPoint, highlight);
        }

        public static void DrawPolygonInProgress(this Graphics g, List<Vertex> Points)
        {
            bool start = true;
            Vertex lastPoint = new Vertex();

            foreach (Vertex p in Points)
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
