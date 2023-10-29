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
        public static Vertex MousePos
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

        private List<Vertex> Points = new();
        private List<Polygon> Polygons = new();
        private List<Polygon> OffsetPolygons = new();
        private Vertex grabbedPoint = new();
        private (Vertex p1, Vertex p2) grabbedLine = new();
        private Polygon? grabbedPolygon = null;
        private Vertex? clickedPoint;

        public NeoGebra()
        {
            InitializeComponent();
            InitializeScene();
            MousePos = new();

            state = States.Idle;
            linemode = LineModes.WinForms;
            winformsLineButton.Checked = true;
            offsetSlider.Value = offset;
            offsetCheckBox.Checked = true;
        }

        private void InitializeScene()
        {

            //Polygons.Add(new Polygon(new List<Vertex>()
            //{
            //    new Vertex(132, 295),
            //    new Vertex(440, 316),
            //    new Vertex(316, 103),
            //    new Vertex(330, 236),
            //    new Vertex(232, 228),
            //    new Vertex(272, 118)
            //}));


            Polygons.Add(new Polygon(new List<Vertex>()
            {
                new Vertex(88, 56),
                new Vertex(197, 60),
                new Vertex(230, 151),
                new Vertex(143, 151),
                new Vertex(60, 235)
            }));

            TryToGrabLine(new Vertex(200, 151));
            SetHorizontalConstraint();

            Polygons.Add(new Polygon(new List<Vertex>()
            {
                new Vertex(351, 198),
                new Vertex(351, 109),
                new Vertex(415, 40),
                new Vertex(497, 100),
                new Vertex(464, 197)
            }));

            TryToGrabLine(new Vertex(351, 150));
            SetVerticalConstraint();

            Polygons.Add(new Polygon(new List<Vertex>()
            {
                new Vertex(195, 379),
                new Vertex(165, 292),
                new Vertex(250, 225),
                new Vertex(250, 333),
                new Vertex(387, 275),
                new Vertex(387, 379)
            }));

            TryToGrabLine(new Vertex(250, 300));
            SetVerticalConstraint();
            TryToGrabLine(new Vertex(387, 300));
            SetVerticalConstraint();
            TryToGrabLine(new Vertex(200, 379));
            SetHorizontalConstraint();
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

                Vertex lastPoint = Points.Last();

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
            MousePos = new Vertex(e.X, e.Y);
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
            clickedPoint = new Vertex(e.X, e.Y);

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
            clickedPoint = new Vertex(e.X, e.Y);

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

        private void offsetCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (offsetCheckBox.Checked)
            {
                offset = offsetSlider.Value; 
                offsetSlider.Enabled = true;
            }
            else
            {
                offset = 0; 
                offsetSlider.Enabled = false;
            }

            splitContainer.Refresh();
        }
    }
}
