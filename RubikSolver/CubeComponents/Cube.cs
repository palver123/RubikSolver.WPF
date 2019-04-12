using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace RubikSolver.CubeComponents
{
    internal class Cube
    {
        internal enum Completeness
        {
            /// <summary>
            /// The cube is scrambled. The topmost row must be complete, otherwise my AI cannot solve the cube
            /// </summary>
            Scrambled,

            /// <summary>
            /// Vertical edge positions are OK and every preliminary step is completed
            /// </summary>
            SecondRowComplete,

            /// <summary>
            /// Corner positions are OK and every preliminary step is completed
            /// </summary>
            BottomCornersOK,

            /// <summary>
            /// Bottom edge positions are OK and every preliminary step is completed
            /// </summary>
            BottomEdgePositionsOK,

            /// <summary>
            /// Cube is solved completely
            /// </summary>
            Solved
        }

        /// <summary>
        /// Size of a cubicle is 10 units with 1 unit padding between them
        /// </summary>
        private const int SIZE = 10;

        /// <summary>
        /// This is for the animation. 0 if the cube should stand still. A face rotation consists of 9 * 10 degree rotations - this sums up to 90 degrees -
        /// </summary>
        public int _turnState;

        /// <summary>
        /// Stores the index of the next transformation to apply
        /// </summary>
        public int _currentTransformIndex;

        /// <summary>
        /// Stores the transformations (face rotations) which has to be applied. The AI determines the content of this string
        /// </summary>
        public StringBuilder _transformations;

        /// <summary>
        /// Array to store the 26 cubicles - center cubicle is not stored -
        /// </summary>
        public Cubicle[, ,] _cubicles;

        public Completeness _state;

        public Cube()
        {
            _cubicles = new Cubicle[3, 3, 3];
            for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                    for (var k = 0; k < 3; k++)
                    {
                        _cubicles[i, j, k] = new Cubicle(SIZE, new Vector3D((j - 3 / 2) * SIZE * 1.1, (k - 3 / 2) * SIZE * 1.1, (i - 3 / 2) * SIZE * 1.1), i, j, k);
                    }
            _transformations = new StringBuilder();
            _currentTransformIndex = -1;
            _turnState = 0;
            _state = Completeness.Scrambled;
        }

        public void AnimateFaceRotation()
        {
            // this bool is true if we apply a transform, false if we apply its inverse (inverse of a quarter turn means applying it in the opposite isInverse)
            var isInverse = _turnState < 0;
            switch (_transformations[_currentTransformIndex])
            {
                case 'B':
                    foreach (var cubicle in _cubicles) if (cubicle.center.X < -SIZE / 2.0) cubicle.Rotate(new Vector3D(-SIZE * 1.1, 0, 0), new Vector3D(-1, 0, 0), !isInverse);
                    break;
                case 'b':
                    foreach (var cubicle in _cubicles) if (cubicle.center.X < -SIZE / 2.0) cubicle.Rotate(new Vector3D(-SIZE * 1.1, 0, 0), new Vector3D(-1, 0, 0), isInverse);
                    break;
                case 'L':
                    foreach (var cubicle in _cubicles) if (cubicle.center.Y < -SIZE / 2.0) cubicle.Rotate(new Vector3D(0, -SIZE * 1.1, 0), new Vector3D(0, -1, 0), !isInverse);
                    break;
                case 'l':
                    foreach (var cubicle in _cubicles) if (cubicle.center.Y < -SIZE / 2.0) cubicle.Rotate(new Vector3D(0, -SIZE * 1.1, 0), new Vector3D(0, -1, 0), isInverse);
                    break;
                case 'U':
                    foreach (var cubicle in _cubicles) if (cubicle.center.Z > SIZE / 2.0) cubicle.Rotate(new Vector3D(0, 0, SIZE * 1.1), new Vector3D(0, 0, 1), !isInverse);
                    break;
                case 'u':
                    foreach (var cubicle in _cubicles) if (cubicle.center.Z > SIZE / 2.0) cubicle.Rotate(new Vector3D(0, 0, SIZE * 1.1), new Vector3D(0, 0, 1), isInverse);
                    break;
                case 'R':
                    foreach (var cubicle in _cubicles) if (cubicle.center.Y > SIZE / 2.0) cubicle.Rotate(new Vector3D(0, SIZE * 1.1, 0), new Vector3D(0, 1, 0), !isInverse);
                    break;
                case 'r':
                    foreach (var cubicle in _cubicles) if (cubicle.center.Y > SIZE / 2.0) cubicle.Rotate(new Vector3D(0, SIZE * 1.1, 0), new Vector3D(0, 1, 0), isInverse);
                    break;
                case 'F':
                    foreach (var cubicle in _cubicles) if (cubicle.center.X > SIZE / 2.0) cubicle.Rotate(new Vector3D(SIZE * 1.1, 0, 0), new Vector3D(1, 0, 0), !isInverse);
                    break;
                case 'f':
                    foreach (var cubicle in _cubicles) if (cubicle.center.X > SIZE / 2.0) cubicle.Rotate(new Vector3D(SIZE * 1.1, 0, 0), new Vector3D(1, 0, 0), isInverse);
                    break;
                case 'D':
                    foreach (var cubicle in _cubicles) if (cubicle.center.Z < -SIZE / 2.0) cubicle.Rotate(new Vector3D(0, 0, -SIZE * 1.1), new Vector3D(0, 0, -1), !isInverse);
                    break;
                case 'd':
                    foreach (var cubicle in _cubicles) if (cubicle.center.Z < -SIZE / 2.0) cubicle.Rotate(new Vector3D(0, 0, -SIZE * 1.1), new Vector3D(0, 0, -1), isInverse);
                    break;
                default:
                    System.Windows.MessageBox.Show("Cannot read recipe!", "Error!");
                    break;
            }
            if (_turnState > 0) _turnState--;
            else if (_turnState < 0)
            {
                _turnState++;
                if (_turnState == 0) _currentTransformIndex--;
            }
            ReDraw();
        }

        public void ReDraw()
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
                        _cubicles[i, j, k].ReDraw(10, i, j, k);
                    }
        }

        public Cubicle GetCubicleByCenter(Vector3D center)
        {
            return _cubicles.Cast<Cubicle>().FirstOrDefault(cubicle => cubicle.virtualCenter == center);
        }

        public Cubicle GetCubicleByFacetColors(Facet[] facets)
        {
            foreach (var cubicle in _cubicles)
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
