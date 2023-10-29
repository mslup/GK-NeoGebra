using System.Security.Cryptography.Pkcs;

namespace lab1
{
    partial class NeoGebra
    {
        private void CheckForCursorChange()
        {
            bool noObjectHoveredOver = true;
            Vertex? prevPoint;

            foreach (Polygon poly in Polygons)
            {
                prevPoint = poly.vertices.Last();
                foreach (Vertex p in poly.vertices)
                {
                    if (!MousePos.IsCloseToPoint(p) &&
                        !MousePos.IsCloseToLine(prevPoint, p) &&
                        !MousePos.IsInsidePolygon(poly))
                    {
                        prevPoint = p;
                        continue;
                    }

                    if (state == States.Idle)
                        Cursor = Cursors.SizeAll;
                    noObjectHoveredOver = false;

                    prevPoint = p;
                }
            }

            if (noObjectHoveredOver &&
                state != States.Deleting &&
                state != States.SettingEdgeConstraintV &&
                state != States.SettingEdgeConstraintH)
                Cursor = Cursors.Default;
        }

        private void Delete()
        {
            Polygon? polyToUpdate = null;
            Polygon? polyToDelete = null;
            int polyIndex;
            int pointIndex = 0;

            for (polyIndex = Polygons.Count - 1; polyIndex >= 0; polyIndex--)
            {
                Polygon poly = Polygons[polyIndex];

                for (pointIndex = poly.vertices.Count - 1; pointIndex >= 0; pointIndex--)
                {
                    Vertex p = poly.vertices[pointIndex];
                    Vertex pPrev = poly.vertices[(pointIndex + 1) % poly.vertices.Count];

                    if (clickedPoint.IsCloseToPoint(p))
                    {
                        polyToUpdate = poly;
                        p.Farewell();
                        break;
                    }

                    if (clickedPoint.IsCloseToLine(p, pPrev))
                    {
                        Vertex.DeleteConstraint(p, pPrev);
                        break;
                    }

                    if (clickedPoint.IsInsidePolygon(poly))
                    {
                        polyToDelete = poly;
                        break;
                    }
                }

                if (polyToUpdate != null || polyToDelete != null)
                    break;
            }

            if (polyToUpdate != null)
            {
                polyToUpdate.vertices.RemoveAt(pointIndex);
                if (polyToUpdate.vertices.Count == 0)
                    Polygons.RemoveAt(polyIndex);
            }
            else if (polyToDelete != null)
            {
                Polygons.RemoveAt(polyIndex);
            }
        }

        private void BuildPolygon()
        {
            if (Points == null)
                return;

            if (Points.Count > 0)
            {
                // Undo last point addition
                Vertex lastPoint = Points.Last();
                if (lastPoint.IsCloseToPoint(clickedPoint))
                {
                    Points.RemoveAt(Points.Count - 1);
                    if (Points.Count == 0)
                        state = States.Idle;

                    return;
                }

                // Close polygon
                Vertex initialPoint = Points.First();
                if (initialPoint.IsCloseToPoint(clickedPoint))
                {
                    state = States.Idle;
                    Polygons.Add(new Polygon(Points));
                    Points.Clear();

                    return;
                }
            }

            Points.Add(clickedPoint);
        }

        private bool TryToGrabPoint(Vertex point)
        {
            for (int i = Polygons.Count - 1; i >= 0; i--)
            {
                Polygon poly = Polygons[i];

                for (int j = poly.vertices.Count - 1; j >= 0; j--)
                {
                    Vertex vertex = poly.vertices[j];

                    if (vertex.IsCloseToPoint(point))
                    {
                        grabbedPoint = vertex;
                        canvas.Invalidate();
                        return true;
                    }
                }
            }

            return false;
        }

        private bool TryToGrabLine(Vertex point)
        {
            for (int i = Polygons.Count - 1; i >= 0; i--)
            {
                Polygon poly = Polygons[i];

                for (int j = poly.vertices.Count - 1; j >= 0; j--)
                {
                    Vertex vertex = poly.vertices[j];
                    Vertex prevVertex = poly.vertices[(j + 1) % poly.vertices.Count];

                    if (point.IsCloseToLine(prevVertex, vertex))
                    {
                        grabbedLine = (prevVertex, vertex);
                        grabbedPolygon = poly;
                        canvas.Invalidate();
                        return true;
                    }
                }
            }

            return false;
        }

        private bool TryToGrabPolygon(Vertex point)
        {
            for (int i = Polygons.Count - 1; i >= 0; i--)
            {
                if (point.IsInsidePolygon(Polygons[i]))
                {
                    grabbedPolygon = Polygons[i];
                    canvas.Invalidate();
                    return true;
                }
            }
            return false;
        }

        private void MoveVertex()
        {
            Vertex? h = grabbedPoint.GetHorizontallyConstrainedNeighbor();
            Vertex? v = grabbedPoint.GetVerticallyConstrainedNeighbor();

            if (h != null)
                h.Y = MousePos.Y;
            if (v != null)
                v.X = MousePos.X;

            grabbedPoint.Y = MousePos.Y;
            grabbedPoint.X = MousePos.X;
        }

        private void MoveEdge()
        {
            Vertex? h1 = grabbedLine.p1.GetHorizontallyConstrainedNeighbor();
            Vertex? v1 = grabbedLine.p1.GetVerticallyConstrainedNeighbor();
            Vertex? h2 = grabbedLine.p2.GetHorizontallyConstrainedNeighbor();
            Vertex? v2 = grabbedLine.p2.GetVerticallyConstrainedNeighbor();

            if (h1 != null && h1 != grabbedLine.p2)
                h1.Y += MousePos.Y - clickedPoint.Y;
            else if (v1 != null && v1 != grabbedLine.p2)
                v1.X += MousePos.X - clickedPoint.X;
            if (h2 != null && h2 != grabbedLine.p1)
                h2.Y += MousePos.Y - clickedPoint.Y;
            else if (v2 != null && v2 != grabbedLine.p1)
                v2.X += MousePos.X - clickedPoint.X;

            grabbedLine.p1.Y += MousePos.Y - clickedPoint.Y;
            grabbedLine.p2.Y += MousePos.Y - clickedPoint.Y;
            grabbedLine.p1.X += MousePos.X - clickedPoint.X;
            grabbedLine.p2.X += MousePos.X - clickedPoint.X;
        }

        private void MovePolygon()
        {
            foreach (Vertex p in grabbedPolygon.vertices)
            {
                p.X += MousePos.X - clickedPoint.X;
                p.Y += MousePos.Y - clickedPoint.Y;
            }
        }

        private void AddMidpoint()
        {
            if (grabbedPolygon == null)
            {
                state = States.Idle;
                return;
            }

            Vertex.DeleteConstraint(grabbedLine);

            Vertex midPoint = Vertex.middlePoint(grabbedLine.p1, grabbedLine.p2);
            int i = 0;

            Vertex prevVertex = grabbedPolygon.vertices.Last();
            foreach (Vertex vertex in grabbedPolygon.vertices)
            {
                if ((prevVertex == grabbedLine.p1 && vertex == grabbedLine.p2) ||
                    (prevVertex == grabbedLine.p2 && vertex == grabbedLine.p1))
                {
                    break;
                }

                prevVertex = vertex;
                i++;
            }

            grabbedPolygon.vertices.Insert(i, midPoint);

            state = States.Idle;
        }

        private void SetHorizontalConstraint()
        {
            Vertex midPoint = Vertex.middlePoint(grabbedLine.p1, grabbedLine.p2);

            if (grabbedLine.p1.SetConstraint(grabbedLine.p2, Vertex.Constraints.Horizontal))
            {
                grabbedLine.p1.Y = midPoint.Y;
                grabbedLine.p2.Y = midPoint.Y;
            }

            state = States.Idle;
        }

        private void SetVerticalConstraint()
        {
            Vertex midPoint = Vertex.middlePoint(grabbedLine.p1, grabbedLine.p2);

            if (grabbedLine.p1.SetConstraint(grabbedLine.p2, Vertex.Constraints.Vertical))
            {
                grabbedLine.p1.X = midPoint.X;
                grabbedLine.p2.X = midPoint.X;
            }

            state = States.Idle;
        }
    }
}
