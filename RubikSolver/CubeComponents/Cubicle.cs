using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace RubikSolver.CubeComponents
{
    internal class Cubicle
    {
        /// <summary>
        /// Size of a cubicle is 10 units with 1 unit padding between them
        /// </summary>
        internal const int SIZE = 10;

        private readonly Point3D[] vertices;
        public Vector3D center;
        public Vector3D virtualCenter;
        public Facet[] facets;
        public bool inPlace;
        public static Viewport3D viewport;

        public Cubicle(int size, Vector3D center, int i, int j, int k)
        {
            this.center = virtualCenter = center;
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
                new Point3D(-halfSize, halfSize, halfSize) + center,
            };
            facets = new[]{
                new Facet(new Vector3D(-1, 0, 0)),
                new Facet(new Vector3D(0, -1, 0)),
                new Facet(new Vector3D(0, 0, 1)),
                new Facet(new Vector3D(0, 1, 0)),
                new Facet(new Vector3D(1, 0, 0)),
                new Facet(new Vector3D(0, 0, -1)),
            };
            if (j == 0) facets[0].color = MainWindow.cubicleFaceColors[0, i * 3 + k];
            if (k == 0) facets[1].color = MainWindow.cubicleFaceColors[1, j * 3 + i];
            if (i == 2) facets[2].color = MainWindow.cubicleFaceColors[2, j * 3 + k];
            if (k == 2) facets[3].color = MainWindow.cubicleFaceColors[3, j * 3 + (2 - i)];
            if (j == 2) facets[4].color = MainWindow.cubicleFaceColors[4, (2 - i) * 3 + k];
            if (i == 0) facets[5].color = MainWindow.cubicleFaceColors[5, (2 - j) * 3 + k];
        }

        private static Model3DGroup CreateTriangleModel(Point3D p0, Point3D p1, Point3D p2, Color color)
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

            Material material = new DiffuseMaterial(new SolidColorBrush(color));
            var model = new GeometryModel3D(mesh, material);
            var group = new Model3DGroup();
            group.Children.Add(model);
            return group;
        }

        private static Vector3D CalculateNormal(Point3D p0, Point3D p1, Point3D p2)
        {
            var v0 = new Vector3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            var v1 = new Vector3D(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
            return Vector3D.CrossProduct(v0, v1);
        }

        public void ReDraw(int i, int j, int k)
        {
            var modelCube = new Model3DGroup();
            var color = Color.FromRgb(10, 10, 10);

            //top side triangles
            if (i == 2) color = MainWindow._cubeColors[MainWindow.cubicleFaceColors[2, j * 3 + k]].Color;
            modelCube.Children.Add(CreateTriangleModel(vertices[3], vertices[2], vertices[6], color));
            modelCube.Children.Add(CreateTriangleModel(vertices[3], vertices[6], vertices[7], color));
            color = Color.FromRgb(10, 10, 10);
            //front side triangles
            if (j == 2) color = MainWindow._cubeColors[MainWindow.cubicleFaceColors[4, (2 - i) * 3 + k]].Color;
            modelCube.Children.Add(CreateTriangleModel(vertices[2], vertices[1], vertices[5], color));
            modelCube.Children.Add(CreateTriangleModel(vertices[2], vertices[5], vertices[6], color));
            color = Color.FromRgb(10, 10, 10);
            //bottom side triangles
            if (i == 0) color = MainWindow._cubeColors[MainWindow.cubicleFaceColors[5, (2 - j) * 3 + k]].Color;
            modelCube.Children.Add(CreateTriangleModel(vertices[1], vertices[0], vertices[4], color));
            modelCube.Children.Add(CreateTriangleModel(vertices[1], vertices[4], vertices[5], color));
            color = Color.FromRgb(10, 10, 10);
            //back side triangles
            if (j == 0) color = MainWindow._cubeColors[MainWindow.cubicleFaceColors[0, i * 3 + k]].Color;
            modelCube.Children.Add(CreateTriangleModel(vertices[0], vertices[3], vertices[7], color));
            modelCube.Children.Add(CreateTriangleModel(vertices[0], vertices[7], vertices[4], color));
            color = Color.FromRgb(10, 10, 10);
            //right side triangles
            if (k == 2) color = MainWindow._cubeColors[MainWindow.cubicleFaceColors[3, j * 3 + (2 - i)]].Color;
            modelCube.Children.Add(CreateTriangleModel(vertices[7], vertices[6], vertices[5], color));
            modelCube.Children.Add(CreateTriangleModel(vertices[7], vertices[5], vertices[4], color));
            color = Color.FromRgb(10, 10, 10);
            //left side triangles
            if (k == 0) color = MainWindow._cubeColors[MainWindow.cubicleFaceColors[1, j * 3 + i]].Color;
            modelCube.Children.Add(CreateTriangleModel(vertices[2], vertices[3], vertices[0], color));
            modelCube.Children.Add(CreateTriangleModel(vertices[2], vertices[0], vertices[1], color));

            var model = new ModelVisual3D();
            model.Content = modelCube;
            viewport.Children.Add(model);
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

        //This deals with the rotation of a cube face

        public void Rotate(Vector3D centerOfRotation, Vector3D axis, bool direction)
        {
            var rotationMatrix = direction ? new Matrix3D(0.9848 + axis.X * axis.X * 0.0151922, axis.X * axis.Y *0.0151922 - axis.Z * 0.173648, axis.X * axis.Z *0.0151922 + axis.Y * 0.173648, 0,
                                                                axis.X * axis.Y *0.0151922 + axis.Z * 0.173648, 0.9848 + axis.Y * axis.Y * 0.0151922, axis.Y * axis.Z * 0.0151922 - axis.X * 0.173648, 0,
                                                                axis.X * axis.Z *0.0151922 - axis.Y * 0.173648, axis.Y * axis.Z *0.0151922 + axis.X * 0.173648, 0.9848 + axis.Z * axis.Z * 0.0151922, 0,
                                                                0, 0, 0, 1)
                                                  :
                                             new Matrix3D(0.9848 + axis.X * axis.X * 0.0151922, axis.X * axis.Y *0.0151922 + axis.Z * 0.173648, axis.X * axis.Z *0.0151922 - axis.Y * 0.173648, 0,
                                                                axis.X * axis.Y *0.0151922 - axis.Z * 0.173648, 0.9848 + axis.Y * axis.Y * 0.0151922, axis.Y * axis.Z * 0.0151922 + axis.X * 0.173648, 0,
                                                                axis.X * axis.Z * 0.0151922 + axis.Y * 0.173648, axis.Y * axis.Z * 0.0151922 - axis.X * 0.173648, 0.9848 + axis.Z * axis.Z * 0.0151922, 0,
                                                                0, 0, 0, 1);
            for (var i = 0; i < 8; i++)
                vertices[i] = (vertices[i] - centerOfRotation) * rotationMatrix  + centerOfRotation;
            center = (center - centerOfRotation) * rotationMatrix + centerOfRotation;
        }
    }
}