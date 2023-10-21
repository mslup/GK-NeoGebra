using System.Diagnostics.CodeAnalysis;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace lab1
{
    public partial class NeoGebra : Form
    {
        public const int eps = 8;
        public static intPoint MousePos
        {
            get;
            private set;
        }
        public static Dictionary<(intPoint, intPoint), char> EdgeConstraints
        {
            get;
            private set;
        }

        private enum States
        {
            Idle,
            BuildingPolygon,
            MovingVertex,
            MovingEdge,
            MovingPolygon,
            AddingMidpoint,
            Deleting,
            AddingEdgeConstraintH,
            AddingEdgeConstraintV
        };
        private States state;

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
            EdgeConstraints = new();
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

            switch (state)
            {
            case States.BuildingPolygon:
                BuildPolygon(new intPoint(e.X, e.Y));
                break;
            case States.Deleting:
                DeleteVertexOrConstraint(new intPoint(e.X, e.Y));
                break;
            case States.AddingEdgeConstraintH:
                AddHorizontalConstraint();
                break;
            case States.AddingEdgeConstraintV:
                AddVerticalConstraint();
                break;
            }

            (sender as Control)!.Invalidate();
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            MousePos = new intPoint(e.X, e.Y);
            CheckForCursorChange();

            if (state == States.MovingVertex)
            {
                MoveVertex();
            }
            if (state == States.MovingEdge)
            {
                MoveEdge();
            }
            if (state == States.MovingPolygon)
            {
                MovePolygon();
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

            if (state == States.Idle ||
                state == States.AddingEdgeConstraintH ||
                state == States.AddingEdgeConstraintV)
            {
                if (Polygons.Count == 0)
                    return;

                if (TryToGrabPoint(clickedPoint))
                {
                    state = States.MovingVertex;
                }
                else if (TryToGrabLine(clickedPoint))
                {
                    if (state != States.AddingEdgeConstraintH &&
                        state != States.AddingEdgeConstraintV)
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
            switch (e.KeyCode)
            {
            case Keys.ControlKey:
                Cursor = Cursors.No;
                state = States.Deleting;
                break;
            case Keys.H:
                Cursor = Cursors.HSplit;
                state = States.AddingEdgeConstraintH;
                break;
            case Keys.V:
                Cursor = Cursors.VSplit;
                state = States.AddingEdgeConstraintV;
                break;
            }
        }

        private void NeoGebra_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
            case Keys.ControlKey:
            case Keys.H:
            case Keys.V:
                Cursor = Cursors.Default;
                state = States.Idle;
                break;
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
