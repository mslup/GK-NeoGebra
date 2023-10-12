using System.Diagnostics.CodeAnalysis;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace lab1
{
    public partial class NeoGebra : Form
    {
        private enum States
        {
            Idle,
            BuildingPolygon,
            MovingVertex,
            MovingEdge,
            MovingPolygon,
            AddingMidpoint,
            DeletingVertex
        };
        private States state;

        public const int eps = 8;

        public static intPoint MousePos
        {
            get;
            private set;
        }

        private List<intPoint> Points = new();
        private List<Polygon> Polygons = new();
        private intPoint grabbedPoint = new();
        private (intPoint p1, intPoint p2) grabbedLine = new();
        private Polygon? grabbedPolygon = null;
        private intPoint? clickedPoint;

        public NeoGebra()
        {
            InitializeComponent();
            InitializeCanvas();
            MousePos = new();
            state = States.Idle;
        }

        private void InitializeCanvas()
        {
            canvas = new PictureBox();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.DrawPolygons(Polygons);
            g.DrawPolygonInProgress(Points);

            if (state == States.BuildingPolygon)
            {
                if (Points.Count == 0)
                    return;

                intPoint lastPoint = Points.Last();

                g.DrawLine(lastPoint, MousePos);
                g.DrawPoint(lastPoint);
            }
        }

        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (state == States.Idle)
                state = States.BuildingPolygon;

            if (state == States.DeletingVertex)
            {
                DeleteVertex(new intPoint(e.X, e.Y));
                (sender as Control)!.Invalidate();
                return;
            }

            if (state == States.BuildingPolygon)
            {
                BuildPolygon(new intPoint(e.X, e.Y));
                (sender as Control)!.Invalidate();
                return;
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            MousePos = new intPoint(e.X, e.Y);
            CheckForCursorChange();

            if (state == States.MovingVertex)
            {
                grabbedPoint.x = MousePos.x;
                grabbedPoint.y = MousePos.y;
            }
            if (state == States.MovingEdge)
            {
                grabbedLine.p1.x += MousePos.x - clickedPoint.x;
                grabbedLine.p1.y += MousePos.y - clickedPoint.y;
                grabbedLine.p2.x += MousePos.x - clickedPoint.x;
                grabbedLine.p2.y += MousePos.y - clickedPoint.y;
            }
            if (state == States.MovingPolygon)
            {
                foreach (intPoint p in grabbedPolygon.vertices)
                {
                    p.x += MousePos.x - clickedPoint.x;
                    p.y += MousePos.y - clickedPoint.y;
                }
            }

            clickedPoint = MousePos;
            (sender as Control).Invalidate();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Points.Clear();
            Polygons.Clear();
            state = States.Idle;
            canvas.Invalidate();
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            clickedPoint = new intPoint(e.X, e.Y);

            if (state == States.Idle)
            {
                if (TryToGrabPoint(clickedPoint))
                {
                    state = States.MovingVertex;
                }
                else if (TryToGrabLine(clickedPoint))
                {
                    state = States.MovingEdge;
                }
                else if (TryToGrabPolygon(clickedPoint))
                {
                    state = States.MovingPolygon;
                }
            }
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (state == States.MovingVertex ||
                state == States.MovingEdge ||
                state == States.MovingPolygon)
                state = States.Idle;

            clickedPoint = null;
        }

        private void NeoGebra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                Cursor = Cursors.No;
                state = States.DeletingVertex;
            }
        }

        private void NeoGebra_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                Cursor = Cursors.Default;
                state = States.Idle;
            }
        }

        private void canvas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            clickedPoint = new intPoint(e.X, e.Y);

            if (TryToGrabLine(clickedPoint))
            {
                state = States.AddingMidpoint;
                AddMidpoint();
            }

            (sender as Control).Invalidate();
        }
    }
}
