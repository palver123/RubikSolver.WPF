﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using RubikSolver.Utils;

namespace RubikSolver.CubeComponents
{
    internal class Cubicle
    {
        /// <summary>
        /// Size of a cubicle.
        /// </summary>
        internal const double SIZE = 10;

        private readonly Point3D[] vertices;
        public Vector3D _center;
        public Vector3D virtualCenter;
        public Facet[] facets;
        public bool inPlace;

        public Cubicle(double size, Vector3D center, int i, int j, int k)
        {
            _center = virtualCenter = center;
            inPlace = true;
            var halfSize = size / 2;
            vertices = new[]{
                new Point3D(-halfSize, -halfSize, -halfSize) + center,
                new Point3D(halfSize, -halfSize, -halfSize) + center,
                new Point3D(halfSize, -halfSize, halfSize) + center,
                new Point3D(-halfSize, -halfSize, halfSize) + center,
                new Point3D(-halfSize, halfSize, -halfSize) + center,
                new Point3D(halfSize, halfSize, -halfSize) + center,
                new Point3D(halfSize, halfSize, halfSize) + center,
                new Point3D(-halfSize, halfSize, halfSize) + center
            };
            facets = new[]{
                new Facet(new Vector3D(-1, 0, 0)),
                new Facet(new Vector3D(0, -1, 0)),
                new Facet(new Vector3D(0, 0, 1)),
                new Facet(new Vector3D(0, 1, 0)),
                new Facet(new Vector3D(1, 0, 0)),
                new Facet(new Vector3D(0, 0, -1))
            };
            if (j == 0) facets[0].color = MainWindow._cubicleFaceColors[0, i * 3 + k];
            if (k == 0) facets[1].color = MainWindow._cubicleFaceColors[1, j * 3 + i];
            if (i == 2) facets[2].color = MainWindow._cubicleFaceColors[2, j * 3 + k];
            if (k == 2) facets[3].color = MainWindow._cubicleFaceColors[3, j * 3 + (2 - i)];
            if (j == 2) facets[4].color = MainWindow._cubicleFaceColors[4, (2 - i) * 3 + k];
            if (i == 0) facets[5].color = MainWindow._cubicleFaceColors[5, (2 - j) * 3 + k];
        }

        private static Model3D CreateTriangleModel(Point3D p0, Point3D p1, Point3D p2, Brush color)
        {
            var mesh = new MeshGeometry3D();
            mesh.Positions.Add(p0);
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            var normal = CalculateNormal(p0, p1, p2);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);

            Material material = new DiffuseMaterial(color);
            return new GeometryModel3D(mesh, material);
        }

        private static Vector3D CalculateNormal(Point3D p0, Point3D p1, Point3D p2)
        {
            var v0 = new Vector3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            var v1 = new Vector3D(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
            return Vector3D.CrossProduct(v0, v1);
        }

        public Model3DGroup GetGeometry(int i, int j, int k)
        {
            var geometry = new Model3DGroup();
            var defaultColor = new SolidColorBrush(Color.FromRgb(10, 10, 10));

            // top side triangles
            var color = i == 2 ? MainWindow._cubeColors[MainWindow._cubicleFaceColors[2, j * 3 + k]] : defaultColor;
            geometry.Children.Add(CreateTriangleModel(vertices[3], vertices[2], vertices[6], color));
            geometry.Children.Add(CreateTriangleModel(vertices[3], vertices[6], vertices[7], color));

            // front side triangles
            color = j == 2 ? MainWindow._cubeColors[MainWindow._cubicleFaceColors[4, (2 - i) * 3 + k]] : defaultColor;
            geometry.Children.Add(CreateTriangleModel(vertices[2], vertices[1], vertices[5], color));
            geometry.Children.Add(CreateTriangleModel(vertices[2], vertices[5], vertices[6], color));
            
            // bottom side triangles
            color = i == 0 ? MainWindow._cubeColors[MainWindow._cubicleFaceColors[5, (2 - j) * 3 + k]] : defaultColor;
            geometry.Children.Add(CreateTriangleModel(vertices[1], vertices[0], vertices[4], color));
            geometry.Children.Add(CreateTriangleModel(vertices[1], vertices[4], vertices[5], color));

            // back side triangles
            color = j == 0 ? MainWindow._cubeColors[MainWindow._cubicleFaceColors[0, i * 3 + k]] : defaultColor;
            geometry.Children.Add(CreateTriangleModel(vertices[0], vertices[3], vertices[7], color));
            geometry.Children.Add(CreateTriangleModel(vertices[0], vertices[7], vertices[4], color));

            // right side triangles
            color = k == 2 ? MainWindow._cubeColors[MainWindow._cubicleFaceColors[3, j * 3 + (2 - i)]] : defaultColor;
            geometry.Children.Add(CreateTriangleModel(vertices[7], vertices[6], vertices[5], color));
            geometry.Children.Add(CreateTriangleModel(vertices[7], vertices[5], vertices[4], color));

            // left side triangles
            color = k == 0 ? MainWindow._cubeColors[MainWindow._cubicleFaceColors[1, j * 3 + i]] : defaultColor;
            geometry.Children.Add(CreateTriangleModel(vertices[2], vertices[3], vertices[0], color));
            geometry.Children.Add(CreateTriangleModel(vertices[2], vertices[0], vertices[1], color));

            return geometry;
        }

        public Facet GetFacet(int facetID)
        {
            switch (facetID)
            {
                case 0:
                    for (var i = 0; i < 6; i++) if (facets[i].normal == new Vector3D(-1, 0, 0)) return facets[i];
                    break;
                case 1:
                    for (var i = 0; i < 6; i++) if (facets[i].normal == new Vector3D(0, -1, 0)) return facets[i];
                    break;
                case 2:
                    for (var i = 0; i < 6; i++) if (facets[i].normal == new Vector3D(0, 0, 1)) return facets[i];
                    break;
                case 3:
                    for (var i = 0; i < 6; i++) if (facets[i].normal == new Vector3D(0, 1, 0)) return facets[i];
                    break;
                case 4:
                    for (var i = 0; i < 6; i++) if (facets[i].normal == new Vector3D(1, 0, 0)) return facets[i];
                    break;
                case 5:
                    for (var i = 0; i < 6; i++) if (facets[i].normal == new Vector3D(0, 0, -1)) return facets[i];
                    break;
                default:
                    break;
            }
            return null;
        }

        public void SwapFacets(int a, int b)
        {
            var temp = facets[a].color;
            facets[a].color = facets[b].color;
            facets[b].color = temp;
        }

        /// <summary>
        /// This deals with the rotation of a cube face
        /// </summary>
        public void Rotate(Vector3D axis, bool direction)
        {
            var angle = direction ? Math.PI / 18 : -Math.PI / 18;
            var rotationMatrix = MathUtils.RotationFromAxisAngle(ref axis, angle);
            var centerOfRotation = axis * Cube.OFFSET;
            for (var i = 0; i < vertices.Length; i++)
                vertices[i] = (vertices[i] - centerOfRotation) * rotationMatrix  + centerOfRotation;
            _center = (_center - centerOfRotation) * rotationMatrix + centerOfRotation;
        }
    }
}