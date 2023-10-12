using System.Security.Cryptography.Pkcs;

namespace lab1
{
    partial class NeoGebra
    {
        private void DeleteVertex(intPoint point)
        {
            Polygon? polyToUpdate = null;
            int polyIndex = 0;
            int pointIndex = 0;

            for (polyIndex = Polygons.Count - 1; polyIndex >= 0; polyIndex--) 
            {
                Polygon poly = Polygons[polyIndex];

                for (pointIndex = poly.vertices.Count - 1; pointIndex >= 0; pointIndex--)
                {
                    intPoint p = poly.vertices[pointIndex];

                    if (p.IsCloseToPoint(point))
                    {
                        polyToUpdate = poly;
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

        private void CheckForCursorChange()
        {
            bool noObjectHoveredOver = true;
            intPoint? lastPoint;

            foreach (Polygon poly in Polygons)
            {
                lastPoint = poly.vertices.Last();
                foreach (intPoint p in poly.vertices)
                {
                    if (!MousePos.IsCloseToPoint(p) && 
                        !MousePos.IsCloseToLine(lastPoint, p) &&
                        !MousePos.IsInsidePolygon(poly))
                    {
                        lastPoint = p;
                        continue;
                    }

                    if (state == States.Idle)
                        Cursor = Cursors.SizeAll;
                    noObjectHoveredOver = false;

                    lastPoint = p;
                }
            }

            if (noObjectHoveredOver && state != States.DeletingVertex)
                Cursor = Cursors.Default;
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
            intPoint prevVertex;
            for (int i = Polygons.Count - 1; i >= 0; i--)
            {
                Polygon poly = Polygons[i];
                prevVertex = poly.vertices.Last();

                for (int j = poly.vertices.Count - 1; j >= 0; j--)
                {
                    intPoint vertex = poly.vertices[j];

                    if (point.IsCloseToLine(prevVertex, vertex))
                    {
                        grabbedLine = (prevVertex, vertex);
                        grabbedPolygon = poly;
                        canvas.Invalidate();
                        return true;
                    }
                    prevVertex = vertex;
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

        private void AddMidpoint()
        {
            if (grabbedPolygon == null)
            {
                state = States.Idle;
                return;
            }

            intPoint midPoint = new intPoint(
                (grabbedLine.p1.x + grabbedLine.p2.x) / 2,
                (grabbedLine.p1.y + grabbedLine.p2.y) / 2);
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
    }
}
