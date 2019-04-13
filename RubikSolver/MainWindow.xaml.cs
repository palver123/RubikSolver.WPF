using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using RubikSolver.AI;
using RubikSolver.CubeComponents;

namespace RubikSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private enum RGBColorChannel
        {
            Red,
            Green,
            Blue
        }

        /// <summary>
        /// Array to store the color of the cubicles 
        /// </summary>
        public static int[,] cubicleFaceColors;

        /// <summary>
        /// The colors of the 6 faces of the cube 
        /// </summary>
        public static SolidColorBrush[] _cubeColors;

        /// <summary>
        /// The currently selected color 
        /// </summary>
        private int selectedColorID;

        /// <summary>
        /// The Rubik's Cube 
        /// </summary>
        private Cube cube;

        /// <summary>
        /// The last known position of the mouse cursor in the viewport 
        /// </summary>
        private Point cursorPosition;

        public MainWindow()
        {
            _cubeColors = new[]
            {
                new SolidColorBrush(Color.FromRgb(171, 10, 18)),
                new SolidColorBrush(Color.FromRgb(30, 40, 108)),
                new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                new SolidColorBrush(Color.FromRgb(19, 104, 36)),
                new SolidColorBrush(Color.FromRgb(253, 44, 12)),
                new SolidColorBrush(Color.FromRgb(250, 209, 19))
            };
            InitializeComponent();
            Cubicle.viewport = mainViewport;

            ResetCube();
            Solver.Initialize();
        }

        public bool IsCubeNetValid()
        {
            var foundCubicles = new List<Cubicle>();
            for (var i = 0; i < 3; i++)
			{
                for (var j = 0; j < 3; j++)
                {
                    for (var k = 0; k < 3; k++)
                    {
                        if (i == 2)
                        {
                            var c1 = Solver.solvedCube._cubicles[i, j, k];
                            var c2 = cube.GetCubicleByCenter(c1.center);
                            var found = false;
                            foreach (var cubicle in foundCubicles)
                            {
                                var contains = true;
                                for (var m = 0; m < 6; m++)
                                {
                                    var contains1 = false;
                                    var contains2 = false;
                                    for (var n = 0; n < 6; n++)
                                    {
                                        if (cubicle.facets[n].color == c2.facets[m].color) contains1 = true;
                                        if (c2.facets[n].color == cubicle.facets[m].color) contains2 = true;
                                    }
                                    if (!contains1 || !contains2) contains = false;
                                }
                                if (contains) found = true;
                            }
                            for (var m = 0; m < 6; m++)
                                if (found || (c1.facets[m].normal == c2.facets[m].normal) && (c1.facets[m].color != c2.facets[m].color)) return false;
                        }
                        else
                        {
                            var cubicle = cube._cubicles[i, j, k];
                            if (Solver.solvedCube.GetCubicleByFacetColors(cubicle.facets) == null) return false;
                            foundCubicles.Add(cubicle);
                        }
                    }
                }
			}
            return true;
        }

        private void colorSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedColorID = colorSelector.SelectedIndex;
            colorPreview.Background = _cubeColors[selectedColorID];
            textBoxRed.Text = _cubeColors[selectedColorID].Color.R.ToString();
            textBoxGreen.Text = _cubeColors[selectedColorID].Color.G.ToString();
            textBoxBlue.Text = _cubeColors[selectedColorID].Color.B.ToString();
        }

        #region Validating color inputs

        //Check if key entered is "numeric".
        private static bool CheckIfNumericKey(Key K, bool isDecimalPoint)
        {
            if (K == Key.Back) //backspace?
                return true;
            if (K == Key.OemPeriod || K == Key.Decimal)  //decimal point?
                return !isDecimalPoint;
            if (K >= Key.D0 && K <= Key.D9)      //digit from top of keyboard?
                return true;
            if (K >= Key.NumPad0 && K <= Key.NumPad9)    //digit from keypad?
                return true;
            return false;   //no "numeric" key
        }

        private void textBoxRed_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBox_TextChanged(sender, RGBColorChannel.Red);
        }

        private void textBoxGreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBox_TextChanged(sender, RGBColorChannel.Green);
        }

        private void textBoxBlue_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBox_TextChanged(sender, RGBColorChannel.Blue);
        }

        private void textBox_TextChanged(object sender, RGBColorChannel channel)
        {
            var textBox = (TextBox)sender;
            int value;
            if (!int.TryParse(textBox.Text, out value))
                return;
            if (value < 0)
                textBox.Text = "0";
            if (value > 255)
                textBox.Text = "255";

            var b = (byte)Math.Min(255, Math.Max(0, value));
            var selectedColor = _cubeColors[selectedColorID].Color;
            Color newColor;
            switch (channel)
            {
                case RGBColorChannel.Red:
                    newColor = Color.FromRgb(b, selectedColor.G, selectedColor.B);
                    break;
                case RGBColorChannel.Green:
                    newColor = Color.FromRgb(selectedColor.R, b, selectedColor.B);
                    break;
                case RGBColorChannel.Blue:
                    newColor = Color.FromRgb(selectedColor.R, selectedColor.G, b);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(channel), channel, null);
            }
            _cubeColors[selectedColorID].Color = newColor;
            colorPreview.Background = _cubeColors[selectedColorID];
        }

        private void textBoxColorControl_KeyDown(object sender, KeyEventArgs e)
        {
            //Get our textbox.
            var Tbx = (TextBox)sender;
            // Initialize the flag.
            if (CheckIfNumericKey(e.Key, Tbx.Text.Contains(".")) == false)
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
        }
 
        #endregion   

        #region Viewport mouse manipulation stuff

        private void mainViewport_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            var newPosition = Mouse.GetPosition(mainViewport);
            var cursorDisplacement = Mouse.GetPosition(mainViewport) - cursorPosition;
            var angleZ = (float)cursorDisplacement.X / 100;
            var angleY = -(float)cursorDisplacement.Y / 125;
            var axis =  Vector3D.CrossProduct(new Vector3D(0, 0, 1), ((PerspectiveCamera)mainViewport.Camera).LookDirection);
            axis.Normalize();
            var rotationY = new Matrix3D(Math.Cos(angleY) + axis.X * axis.X * (1 - Math.Cos(angleY)), axis.X * axis.Y *(1 - Math.Cos(angleY)) - axis.Z * Math.Sin(angleY), axis.X * axis.Z *(1 - Math.Cos(angleY)) + axis.Y * Math.Sin(angleY), 0,
                axis.X * axis.Y *(1 - Math.Cos(angleY)) + axis.Z * Math.Sin(angleY), Math.Cos(angleY) + axis.Y * axis.Y * (1 - Math.Cos(angleY)), axis.Y * axis.Z * (1 - Math.Cos(angleY)) - axis.X * Math.Sin(angleY), 0,
                axis.X * axis.Z *(1 - Math.Cos(angleY)) - axis.Y * Math.Sin(angleY), axis.Y * axis.Z *(1 - Math.Cos(angleY)) + axis.X * Math.Sin(angleY), Math.Cos(angleY) + axis.Z * axis.Z * Math.Sin(angleY), 0,
                0, 0, 0, 1);
            var rotationZ = new Matrix3D(Math.Cos(angleZ), -Math.Sin(angleZ), 0, 0,
                Math.Sin(angleZ), Math.Cos(angleZ), 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);
            ((PerspectiveCamera)mainViewport.Camera).Position = Point3D.Multiply(((PerspectiveCamera)mainViewport.Camera).Position, rotationY*rotationZ);
            ((PerspectiveCamera)mainViewport.Camera).LookDirection = new Point3D(0, 0, 0) - ((PerspectiveCamera)mainViewport.Camera).Position;
            var model = new ModelVisual3D();
            var light = new DirectionalLight {Direction = ((PerspectiveCamera) mainViewport.Camera).LookDirection};
            model.Content = light;
            mainViewport.Children[0] = model;
            cursorPosition = newPosition;
        }

        private void mainViewport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cursorPosition = Mouse.GetPosition(mainViewport);
        }

        private void mainViewport_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void mainViewport_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button) sender;
            btn.Background = _cubeColors[selectedColorID];
            int cubicleID;
            var faceID = cubicleID = -1;
            for (var i = 0; i < cubeNetParent.Children.Count; i++)
                if (cubeNetParent.Children[i].Equals(btn.Parent))
                    faceID = i;
            for (var i = 0; i < ((Grid) cubeNetParent.Children[faceID]).Children.Count; i++)
                if (((Grid) cubeNetParent.Children[faceID]).Children[i].Equals(btn))
                    cubicleID = i;
            cubicleFaceColors[faceID, cubicleID] = selectedColorID;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A:
                    cube._transformations.Append('B');
                    break;
                case Key.S:
                    cube._transformations.Append('b');
                    break;
                case Key.D:
                    cube._transformations.Append('L');
                    break;
                case Key.F:
                    cube._transformations.Append('l');
                    break;
                case Key.G:
                    cube._transformations.Append('U');
                    break;
                case Key.H:
                    cube._transformations.Append('u');
                    break;
                case Key.J:
                    cube._transformations.Append('R');
                    break;
                case Key.K:
                    cube._transformations.Append('r');
                    break;
                case Key.Y:
                    cube._transformations.Append('F');
                    break;
                case Key.X:
                    cube._transformations.Append('f');
                    break;
                case Key.C:
                    cube._transformations.Append('D');
                    break;
                case Key.V:
                    cube._transformations.Append('d');
                    break;
                case Key.Right:
                    if (cube._turnState == 0 && cube._currentTransformIndex < cube._transformations.Length - 1)
                    {
                        cube._turnState = 9;
                        cube._currentTransformIndex++;
                        if (cube._transformations.Length > 1)
                            progressBar.Value = cube._currentTransformIndex / ((float) cube._transformations.Length - 1) * progressBar.Maximum;
                    }

                    break;
                case Key.Left:
                    if (cube._turnState == 0 && cube._currentTransformIndex >= 0)
                    {
                        cube._turnState = -9;
                        if (cube._transformations.Length > 1)
                            progressBar.Value = cube._currentTransformIndex / ((float) cube._transformations.Length - 1) * progressBar.Maximum;
                    }

                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0,40);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (cube._turnState != 0 && cube._currentTransformIndex >= 0 && cube._currentTransformIndex < cube._transformations.Length) cube.AnimateFaceRotation();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            cube = new Cube();
            if (IsCubeNetValid())
            {
                if (Solver.Solve(cube))
                    if (cube._transformations.Length == 0) MessageBox.Show("The cube is already solved.");
                    else MessageBox.Show("I've found a solution of " + cube._transformations.Length + " step (quarter turn metric)");
                else MessageBox.Show("Sorry, I can't solve this cube. Please check if you entered facet colors correctly!\nAlso don't forget that the top row has to be completed!", "Oops");
                cube.ReDraw();
                FocusManager.SetFocusedElement(GetWindow(mainViewport), GetWindow(mainViewport));
            }
            else MessageBox.Show("The given coloring is illegal. Please check if you entered facet colors correctly!\nAlso don't forget that the top row has to be completed!", "Error!");
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            ResetCube();
        }

        private void ResetCube()
        {
            cubicleFaceColors = new[,]
            {
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {2, 2, 2, 2, 2, 2, 2, 2, 2},
                {3, 3, 3, 3, 3, 3, 3, 3, 3},
                {4, 4, 4, 4, 4, 4, 4, 4, 4},
                {5, 5, 5, 5, 5, 5, 5, 5, 5}
            };
            for (var i = 0; i < 9; i++)
            {
                ((Button)cubeNetBack.Children[i]).Background = _cubeColors[cubicleFaceColors[0, i]];
                ((Button)cubeNetLeft.Children[i]).Background = _cubeColors[cubicleFaceColors[1, i]];
                ((Button)cubeNetTop.Children[i]).Background = _cubeColors[cubicleFaceColors[2, i]];
                ((Button)cubeNetRight.Children[i]).Background = _cubeColors[cubicleFaceColors[3, i]];
                ((Button)cubeNetFront.Children[i]).Background = _cubeColors[cubicleFaceColors[4, i]];
                ((Button)cubeNetBottom.Children[i]).Background = _cubeColors[cubicleFaceColors[5, i]];
            }

            progressBar.Value = 0;
            cube = new Cube();
            cube.ReDraw();
        }
    }
}
