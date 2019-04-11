using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows.Controls;
using Rubik01.AI;

namespace Rubik01.CubeComponents
{
    internal class Cube
    {
        // This is for the animation. 0 if the cube should stand still. A face rotation consists of 9 * 10 degree rotations - this sums up to 90 degrees -
        public int turnState;
        // Stores the index of the next transformation to apply
        public int currentTransformIndex;
        // Stores the transformations (face rotations) which has to be applied. The AI determines the content of this string
        public StringBuilder transformations;
        // Array to store the 26 cubicles - center cubicle is not stored -
        public Cubicle[, ,] cubicles;
        // size of a cubicle is 10 units with 1 unit padding between them
        private const int SIZE = 10;
        // solve stage of the cube: Scrambled = 0, Second Row Complete, Corner Positions OK, Corner Orientation OK, Edge Positions OK, Solved
        public int state;

        public Cube()
        {
            cubicles = new Cubicle[3, 3, 3];
            for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                    for (var k = 0; k < 3; k++)
                    {
                        cubicles[i, j, k] = new Cubicle(SIZE, new Vector3D((j - 3 / 2) * SIZE * 1.1, (k - 3 / 2) * SIZE * 1.1, (i - 3 / 2) * SIZE * 1.1), i, j, k);
                    }
            transformations = new StringBuilder();
            currentTransformIndex = -1;
            turnState = 0;
            state = 0;
        }

        public void AnimateFaceRotation()
        {
            // this bool is true if we apply a transform, false if we apply its inverse (inverse of a quarter turn means applying it in the opposite isInverse)
            var isInverse = turnState < 0 ? true : false;
            switch (transformations[currentTransformIndex])
            {
                case 'B':
                    foreach (var cubicle in cubicles) if (cubicle.center.X < -SIZE / 2.0) cubicle.Rotate(new Vector3D(-SIZE * 1.1, 0, 0), new Vector3D(-1, 0, 0), !isInverse);
                    break;
                case 'b':
                    foreach (var cubicle in cubicles) if (cubicle.center.X < -SIZE / 2.0) cubicle.Rotate(new Vector3D(-SIZE * 1.1, 0, 0), new Vector3D(-1, 0, 0), isInverse);
                    break;
                case 'L':
                    foreach (var cubicle in cubicles) if (cubicle.center.Y < -SIZE / 2.0) cubicle.Rotate(new Vector3D(0, -SIZE * 1.1, 0), new Vector3D(0, -1, 0), !isInverse);
                    break;
                case 'l':
                    foreach (var cubicle in cubicles) if (cubicle.center.Y < -SIZE / 2.0) cubicle.Rotate(new Vector3D(0, -SIZE * 1.1, 0), new Vector3D(0, -1, 0), isInverse);
                    break;
                case 'U':
                    foreach (var cubicle in cubicles) if (cubicle.center.Z > SIZE / 2.0) cubicle.Rotate(new Vector3D(0, 0, SIZE * 1.1), new Vector3D(0, 0, 1), !isInverse);
                    break;
                case 'u':
                    foreach (var cubicle in cubicles) if (cubicle.center.Z > SIZE / 2.0) cubicle.Rotate(new Vector3D(0, 0, SIZE * 1.1), new Vector3D(0, 0, 1), isInverse);
                    break;
                case 'R':
                    foreach (var cubicle in cubicles) if (cubicle.center.Y > SIZE / 2.0) cubicle.Rotate(new Vector3D(0, SIZE * 1.1, 0), new Vector3D(0, 1, 0), !isInverse);
                    break;
                case 'r':
                    foreach (var cubicle in cubicles) if (cubicle.center.Y > SIZE / 2.0) cubicle.Rotate(new Vector3D(0, SIZE * 1.1, 0), new Vector3D(0, 1, 0), isInverse);
                    break;
                case 'F':
                    foreach (var cubicle in cubicles) if (cubicle.center.X > SIZE / 2.0) cubicle.Rotate(new Vector3D(SIZE * 1.1, 0, 0), new Vector3D(1, 0, 0), !isInverse);
                    break;
                case 'f':
                    foreach (var cubicle in cubicles) if (cubicle.center.X > SIZE / 2.0) cubicle.Rotate(new Vector3D(SIZE * 1.1, 0, 0), new Vector3D(1, 0, 0), isInverse);
                    break;
                case 'D':
                    foreach (var cubicle in cubicles) if (cubicle.center.Z < -SIZE / 2.0) cubicle.Rotate(new Vector3D(0, 0, -SIZE * 1.1), new Vector3D(0, 0, -1), !isInverse);
                    break;
                case 'd':
                    foreach (var cubicle in cubicles) if (cubicle.center.Z < -SIZE / 2.0) cubicle.Rotate(new Vector3D(0, 0, -SIZE * 1.1), new Vector3D(0, 0, -1), isInverse);
                    break;
                default:
                    System.Windows.MessageBox.Show("Cannot read recipe!", "Error!");
                    break;
            }
            if (turnState > 0) turnState--;
            else if (turnState < 0)
            {
                turnState++;
                if (turnState == 0) currentTransformIndex--;
            }
            reDraw();
        }

        public void reDraw()
        {
            var numberOfCubicles = Cubicle.viewport.Children.Count;
            for (var n = 1; n < numberOfCubicles; n++)
            {
                Cubicle.viewport.Children.RemoveAt(1);
            }
            for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                    for (var k = 0; k < 3; k++)
                    {
                        // size of a cubicle is 10 units with 1 unit padding between them
                        cubicles[i, j, k].ReDraw(10, i, j, k);
                    }
        }

        public Cubicle GetCubicleByCenter(Vector3D center)
        {
            foreach (var cubicle in cubicles) if (cubicle.virtualCenter == center) return cubicle;
            return null;
        }

        public Cubicle GetCubicleByFacetColors(Facet[] facets)
        {
            foreach (var cubicle in cubicles)
            {
                var found = true;
                for (var i = 0; i < 6; i++)
                {
                    var contains1 = false;
                    var contains2 = false;
                    for (var j = 0; j < 6; j++)
                    {
                        if (cubicle.facets[j].color == facets[i].color) contains1 = true;
                        if (facets[j].color == cubicle.facets[i].color) contains2 = true;
                    }
                    if (!contains1 || !contains2) found = false;
                }
                if (found) return cubicle;
            }
            return null;
        }
    }
}
