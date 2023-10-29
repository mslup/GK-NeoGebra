using System.Diagnostics.CodeAnalysis;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace lab1
{
    public partial class NeoGebra : Form
    {
        public const bool Debug = false;

        public const int eps = 8;
        private int offset = 10;
        public static intPoint MousePos
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
            SettingEdgeConstraintH,
            SettingEdgeConstraintV
        };
        private States state;

        public enum LineModes
        {
            WinForms, Bresenham
        };
        public static LineModes linemode { get; private set; }

        private List<intPoint> Points = new();
        private List<Polygon> Polygons = new();
        private List<Polygon> OffsetPolygons = new();
        private intPoint grabbedPoint = new();
        private (intPoint p1, intPoint p2) grabbedLine = new();
        private Polygon? grabbedPolygon = null;
        private intPoint? clickedPoint;

        public NeoGebra()
        {
            InitializeComponent();
            InitializeScene();
            MousePos = new();

            state = States.Idle;
            linemode = LineModes.WinForms;
            winformsLineButton.Checked = true;
            offsetSlider.Value = offset;
        }

        private void InitializeScene()
        {

            Polygons.Add(new Polygon(new List<intPoint>()
            {
                new intPoint(160, 81),
                new intPoint(213, 208),
                new intPoint(174, 261),
                new intPoint(184, 172),
                new intPoint(130, 311),
                new intPoint(429, 293)
            }));

            //canvas = new PictureBox();

            //Polygons.Add(new Polygon(new List<intPoint>()
            //{
            //    new intPoint(88, 56),
            //    new intPoint(197, 60),
            //    new intPoint(230, 151),
            //    new intPoint(143, 151),
            //    new intPoint(60, 235)
            //}));

            //TryToGrabLine(new intPoint(200, 151));
            //SetHorizontalConstraint();

            //Polygons.Add(new Polygon(new List<intPoint>()
            //{
            //    new intPoint(351, 198),
            //    new intPoint(351, 109),
            //    new intPoint(415, 40),
            //    new intPoint(497, 100),
            //    new intPoint(464, 197)
            //}));

            //TryToGrabLine(new intPoint(351, 150));
            //SetVerticalConstraint();

            //Polygons.Add(new Polygon(new List<intPoint>()
            //{
            //    new intPoint(195, 379),
            //    new intPoint(165, 292),
            //    new intPoint(250, 225),
            //    new intPoint(250, 333),
            //    new intPoint(387, 275),
            //    new intPoint(387, 379)
            //}));

            //TryToGrabLine(new intPoint(250, 300));
            //SetVerticalConstraint();
            //TryToGrabLine(new intPoint(387, 300));
            //SetVerticalConstraint();
            //TryToGrabLine(new intPoint(200, 379));
            //SetHorizontalConstraint();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.DrawPolygons(Polygons, offset);
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

            clickedPoint = MousePos;

            switch (state)
            {
            case States.BuildingPolygon:
                BuildPolygon();
                break;
            case States.Deleting:
                Delete();
                break;
            case States.SettingEdgeConstraintH:
                SetHorizontalConstraint();
                break;
            case States.SettingEdgeConstraintV:
                SetVerticalConstraint();
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
                state == States.SettingEdgeConstraintH ||
                state == States.SettingEdgeConstraintV)
            {
                if (Polygons.Count == 0)
                    return;

                if (TryToGrabPoint(clickedPoint))
                {
                    state = States.MovingVertex;
                }
                else if (TryToGrabLine(clickedPoint))
                {
                    if (state != States.SettingEdgeConstraintH &&
                        state != States.SettingEdgeConstraintV)
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
                state = States.SettingEdgeConstraintH;
                break;
            case Keys.V:
                Cursor = Cursors.VSplit;
                state = States.SettingEdgeConstraintV;
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

        private void winformsLineButton_CheckedChanged(object sender, EventArgs e)
        {
            linemode = LineModes.WinForms;

            splitContainer.Refresh();
        }

        private void bresenhamButton_CheckedChanged(object sender, EventArgs e)
        {
            linemode = LineModes.Bresenham;

            splitContainer.Refresh();
        }

        private void offsetSlider_Scroll(object sender, EventArgs e)
        {
            offset = offsetSlider.Value;
            
            splitContainer.Refresh();
        }
    }
}
