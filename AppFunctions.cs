using System.Security.Cryptography.Pkcs;

namespace lab1
{
    partial class NeoGebra
    {
        private void CheckForCursorChange()
        {
            bool noObjectHoveredOver = true;
            intPoint? prevPoint;

            foreach (Polygon poly in Polygons)
            {
                prevPoint = poly.vertices.Last();
                foreach (intPoint p in poly.vertices)
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
                state != States.AddingEdgeConstraintV &&
                state != States.AddingEdgeConstraintH)
                Cursor = Cursors.Default;
        }

        private void DeleteVertexOrConstraint(intPoint point)
        {
            Polygon? polyToUpdate = null;
            int polyIndex;
            int pointIndex = 0;

            for (polyIndex = Polygons.Count - 1; polyIndex >= 0; polyIndex--)
            {
                Polygon poly = Polygons[polyIndex];

                for (pointIndex = poly.vertices.Count - 1; pointIndex >= 0; pointIndex--)
                {
                    intPoint p = poly.vertices[pointIndex];
                    intPoint pPrev = poly.vertices[(pointIndex + 1) % poly.vertices.Count];

                    if (point.IsCloseToPoint(p))
                    {
                        polyToUpdate = poly;
                        p.Farewell();
                        break;
                    }

                    if (point.IsCloseToLine(p, pPrev))
                    {
                        intPoint.DeleteConstraint(p, pPrev);
                        break;
                    }
                }

                if (polyToUpdate != null)
                    break;
            }

            if (polyToUpdate != null)
            {
                polyToUpdate.vertices.RemoveAt(pointIndex);
                if (polyToUpdate.vertices.Count == 0)
                    Polygons.RemoveAt(polyIndex);
            }
        }

        private void BuildPolygon(intPoint point)
        {
            if (Points == null)
                return;

            if (Points.Count > 0)
            {
                // Undo last point addition
                intPoint lastPoint = Points.Last();
                if (lastPoint.IsCloseToPoint(point))
                {
                    Points.RemoveAt(Points.Count - 1);
                    if (Points.Count == 0)
                        state = States.Idle;

                    return;
                }

                // Close polygon
                intPoint initialPoint = Points.First();
                if (initialPoint.IsCloseToPoint(point))
                {
                    state = States.Idle;
                    Polygons.Add(new Polygon(Points));
                    Points.Clear();

                    return;
                }
            }

            Points.Add(point);
        }

        private bool TryToGrabPoint(intPoint point)
        {
            for (int i = Polygons.Count - 1; i >= 0; i--)
            {
                Polygon poly = Polygons[i];

                for (int j = poly.vertices.Count - 1; j >= 0; j--)
                {
                    intPoint vertex = poly.vertices[j];

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

        private bool TryToGrabLine(intPoint point)
        {
            for (int i = Polygons.Count - 1; i >= 0; i--)
            {
                Polygon poly = Polygons[i];

                for (int j = poly.vertices.Count - 1; j >= 0; j--)
                {
                    intPoint vertex = poly.vertices[j];
                    intPoint prevVertex = poly.vertices[(j + 1) % poly.vertices.Count];

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

        private bool TryToGrabPolygon(intPoint point)
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
            intPoint? h = grabbedPoint.GetHorizontallyConstrainedNeighbor();
            intPoint? v = grabbedPoint.GetVerticallyConstrainedNeighbor();

            if (h != null)
            {
                h.y = MousePos.y;
            }
            if (v != null)
            {
                v.x = MousePos.x;
            }

            grabbedPoint.y = MousePos.y;
            grabbedPoint.x = MousePos.x;
        }

        private void MoveEdge()
        {
            intPoint? h1 = grabbedLine.p1.GetHorizontallyConstrainedNeighbor();
            intPoint? v1 = grabbedLine.p1.GetVerticallyConstrainedNeighbor();
            intPoint? h2 = grabbedLine.p2.GetHorizontallyConstrainedNeighbor();
            intPoint? v2 = grabbedLine.p2.GetVerticallyConstrainedNeighbor();

            if (h1 != null && h1 != grabbedLine.p2)
                h1.y += MousePos.y - clickedPoint.y;
            else if (v1 != null && v1 != grabbedLine.p2)
                v1.x += MousePos.x - clickedPoint.x;
            if (h2 != null && h2 != grabbedLine.p1)
                h2.y += MousePos.y - clickedPoint.y;
            else if (v2 != null && v2 != grabbedLine.p1)
                v2.x += MousePos.x - clickedPoint.x;

            grabbedLine.p1.y += MousePos.y - clickedPoint.y;
            grabbedLine.p2.y += MousePos.y - clickedPoint.y;
            grabbedLine.p1.x += MousePos.x - clickedPoint.x;
            grabbedLine.p2.x += MousePos.x - clickedPoint.x;
        }

        private void MovePolygon()
        {
            foreach (intPoint p in grabbedPolygon.vertices)
            {
                p.x += MousePos.x - clickedPoint.x;
                p.y += MousePos.y - clickedPoint.y;
            }
        }

        private void AddMidpoint()
        {
            if (grabbedPolygon == null)
            {
                state = States.Idle;
                return;
            }

            intPoint.DeleteConstraint(grabbedLine);

            intPoint midPoint = intPoint.middlePoint(grabbedLine.p1, grabbedLine.p2);
            int i = 0;

            intPoint prevVertex = grabbedPolygon.vertices.Last();
            foreach (intPoint vertex in grabbedPolygon.vertices)
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

        private void AddHorizontalConstraint()
        {
            intPoint midPoint = intPoint.middlePoint(grabbedLine.p1, grabbedLine.p2);

            intPoint min = grabbedLine.p1.x < grabbedLine.p2.x ? grabbedLine.p1 : grabbedLine.p2;
            intPoint max = grabbedLine.p1.x < grabbedLine.p2.x ? grabbedLine.p2 : grabbedLine.p1;

            if (min.SetConstraint(max, intPoint.Constraints.Horizontal))
            {
                grabbedLine.p1.y = midPoint.y;
                grabbedLine.p2.y = midPoint.y;
            }

            state = States.Idle;
        }

        private void AddVerticalConstraint()
        {
            intPoint midPoint = intPoint.middlePoint(grabbedLine.p1, grabbedLine.p2);

            intPoint min = grabbedLine.p1.y < grabbedLine.p2.y ? grabbedLine.p1 : grabbedLine.p2;
            intPoint max = grabbedLine.p1.y < grabbedLine.p2.y ? grabbedLine.p2 : grabbedLine.p1;

            if (min.SetConstraint(max, intPoint.Constraints.Vertical))
            {
                grabbedLine.p1.x = midPoint.x;
                grabbedLine.p2.x = midPoint.x;
            }

            state = States.Idle;
        }
    }
}
