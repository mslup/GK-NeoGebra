# Polygon Editor

## Overview

This C# application, developed with WinForms, is a Polygon Editor created as part of the Computer Graphics course at the Faculty of Mathematics and Information Science at WUT. The editor provides a user-friendly interface for creating, deleting, and editing polygons, with advanced features such as vertex manipulation, edge movement, and the addition of constraints.

## Features

### Polygon Manipulation
- **Adding, deleting, and editing polygons:** Users can add new polygons, delete existing ones, and perform various editing operations.
- **Vertex manipulation:** Move, remove, and add vertices to polygons.
- **Edge manipulation:** Move entire edges, add vertices in the middle of edges, and smoothly update polygons.

### Constraints and Relations
- **Adding constraints:** Users can add constraints to selected edges, including horizontal and vertical constraints.
- **Constraint visualization:** Constraints are visually represented as icons at the middle of the corresponding edges.
- **Constraint removal:** Users can easily remove constraints.

### Offset Polygon
- **Offset polygon creation:** A user can enable the creation of offset polygons, ensuring that the resulting polygon is closed and free of self-intersections.
- **Smooth offset modification:** The offset polygon is smoothly updated during the modification of the original polygon.

### Drawing Segments
- **Algorithm selection:** Users can choose between library-based and custom (Bresenham's algorithm) implementations for drawing segments using radio buttons.

### Predefined Scene
- **Default scene:** The application provides a predefined scene with a minimum of 2 polygons, including constraints.
