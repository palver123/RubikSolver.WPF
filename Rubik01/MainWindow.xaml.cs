using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using Rubik01.CubeComponents;
using Rubik01.AI;

namespace Rubik01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Array to store the color of the cubicles
        public static int[,] cubicleFaceColors;
        // The colors of the 6 faces of the cube
        public static SolidColorBrush[] cubeColors;
        // The currently selected color 
        private int selectedColorID = 0;
        // The Rubik's Cube
        private Cube cube;
        // The last known position of the mouse cursor in the viewport
        private Point cursorPosition;

        public MainWindow()
        {
            cubeColors = new SolidColorBrush[6]{
                new SolidColorBrush(Color.FromRgb(171, 10, 18)),
                new SolidColorBrush(Color.FromRgb(30, 40, 108)),
                new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                new SolidColorBrush(Color.FromRgb(19, 104, 36)),
                new SolidColorBrush(Color.FromRgb(253, 44, 12)),
                new SolidColorBrush(Color.FromRgb(250, 209, 19))
            };
            cubicleFaceColors = new int[6, 9]{
                {0,0,0,0,0,0,0,0,0},
                {1,1,1,1,1,1,1,1,1},
                {2,2,2,2,2,2,2,2,2},
                {3,3,3,3,3,3,3,3,3},
                {4,4,4,4,4,4,4,4,4},
                {5,5,5,5,5,5,5,5,5}
            };
            InitializeComponent();
            for (var i = 0; i < 9; i++)
            {
                ((Button)cubeNetBack.Children[i]).Background = cubeColors[cubicleFaceColors[0, i]];
                ((Button)cubeNetLeft.Children[i]).Background = cubeColors[cubicleFaceColors[1, i]];
                ((Button)cubeNetTop.Children[i]).Background = cubeColors[cubicleFaceColors[2, i]];
                ((Button)cubeNetRight.Children[i]).Background = cubeColors[cubicleFaceColors[3, i]];
                ((Button)cubeNetFront.Children[i]).Background = cubeColors[cubicleFaceColors[4, i]];
                ((Button)cubeNetBottom.Children[i]).Background = cubeColors[cubicleFaceColors[5, i]];
            }
            Cubicle.viewport = mainViewport;
            cube = new Cube();
            cube.reDraw();
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
                            var c1 = Solver.solvedCube.cubicles[i, j, k];
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
                            var cubicle = cube.cubicles[i, j, k];
                            if (Solver.solvedCube.GetCubicleByFacetColors(cubicle.facets) == null) return false;
                            else foundCubicles.Add(cubicle);
                        }
                    }
                }
			}
            return true;
        }

        private void colorSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedColorID = colorSelector.SelectedIndex;
            colorPreview.Background = cubeColors[selectedColorID];
            textBoxRed.Text = cubeColors[selectedColorID].Color.R.ToString();
            textBoxGreen.Text = cubeColors[selectedColorID].Color.G.ToString();
            textBoxBlue.Text = cubeColors[selectedColorID].Color.B.ToString();
        }

        #region Validating color inputs

        //Check if key entered is "numeric".
        private bool CheckIfNumericKey(Key K, bool isDecimalPoint)
        {
            if (K == Key.Back) //backspace?
                return true;
            else if (K == Key.OemPeriod || K == Key.Decimal)  //decimal point?
                return isDecimalPoint ? false : true;       //or: return !isDecimalPoint
            else if ((K >= Key.D0) && (K <= Key.D9))      //digit from top of keyboard?
                return true;
            else if ((K >= Key.NumPad0) && (K <= Key.NumPad9))    //digit from keypad?
                return true;
            else
                return false;   //no "numeric" key
        }

        private void textBoxRed_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Get our textbox.
            var Tbx = (TextBox)sender;
            var value = -1;
            Int32.TryParse(Tbx.Text, out value);
            if (value < 0) Tbx.Text = "0";
            if (value > 255) Tbx.Text = "255";
            byte red = 0;
            Byte.TryParse(textBoxRed.Text, out red);
            var newColor = Color.FromRgb(red, cubeColors[selectedColorID].Color.G, cubeColors[selectedColorID].Color.B);
            cubeColors[selectedColorID].Color = newColor;
            colorPreview.Background = new SolidColorBrush(newColor);
        }

        private void textBoxGreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            var Tbx = (TextBox)sender;
            var value = -1;
            Int32.TryParse(Tbx.Text, out value);
            if (value < 0) Tbx.Text = "0";
            if (value > 255) Tbx.Text = "255";
            byte green = 0;
            Byte.TryParse(textBoxGreen.Text, out green);
            var newColor = Color.FromRgb(cubeColors[selectedColorID].Color.R, green, cubeColors[selectedColorID].Color.B);
            cubeColors[selectedColorID].Color = newColor;
            colorPreview.Background = new SolidColorBrush(newColor);
        }

        private void textBoxBlue_TextChanged(object sender, TextChangedEventArgs e)
        {
            var Tbx = (TextBox)sender;
            var value = -1;
            Int32.TryParse(Tbx.Text, out value);
            if (value < 0) Tbx.Text = "0";
            if (value > 255) Tbx.Text = "255";
            byte blue = 0;
            Byte.TryParse(textBoxBlue.Text, out blue);
            var newColor = Color.FromRgb(cubeColors[selectedColorID].Color.R, cubeColors[selectedColorID].Color.G, blue);
            cubeColors[selectedColorID].Color = newColor;
            colorPreview.Background = new SolidColorBrush(newColor);
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
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
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
                var light = new DirectionalLight();
                light.Direction = ((PerspectiveCamera)mainViewport.Camera).LookDirection;
                model.Content = light;
                mainViewport.Children[0] = model;
                cursorPosition = newPosition;
            }
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
            var btn = (Button)sender;
            btn.Background = cubeColors[selectedColorID];
            int faceID, cubicleID;
            faceID = cubicleID = -1;
            for (var i = 0; i < cubeNetParent.Children.Count; i++) if (cubeNetParent.Children[i].Equals(btn.Parent)) faceID = i;
            for (var i = 0; i < ((Grid)cubeNetParent.Children[faceID]).Children.Count; i++) if (((Grid)cubeNetParent.Children[faceID]).Children[i].Equals(btn)) cubicleID = i;
            cubicleFaceColors[faceID, cubicleID] = selectedColorID;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A:
                    cube.transformations.Append('B');
                    break;
                case Key.S:
                    cube.transformations.Append('b');
                    break;
                case Key.D:
                    cube.transformations.Append('L');
                    break;
                case Key.F:
                    cube.transformations.Append('l');
                    break;
                case Key.G:
                    cube.transformations.Append('U');
                    break;
                case Key.H:
                    cube.transformations.Append('u');
                    break;
                case Key.J:
                    cube.transformations.Append('R');
                    break;
                case Key.K:
                    cube.transformations.Append('r');
                    break;
                case Key.Y:
                    cube.transformations.Append('F');
                    break;
                case Key.X:
                    cube.transformations.Append('f');
                    break;
                case Key.C:
                    cube.transformations.Append('D');
                    break;
                case Key.V:
                    cube.transformations.Append('d');
                    break;
                case Key.Right:
                    if (cube.turnState == 0 && cube.currentTransformIndex < cube.transformations.Length - 1)
                    {
                        cube.turnState = 9;
                        cube.currentTransformIndex++;
                        if (cube.transformations.Length > 1) progressBar.Value = cube.currentTransformIndex / ((float)cube.transformations.Length - 1) * progressBar.Maximum;
                    }
                    break;
                case Key.Left:
                    if (cube.turnState == 0 && cube.currentTransformIndex >= 0)
                    {
                        cube.turnState = -9;
                        if (cube.transformations.Length > 1) progressBar.Value = cube.currentTransformIndex / ((float)cube.transformations.Length - 1) * progressBar.Maximum;
                    }
                    break;
                default: break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0,40);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (cube.turnState != 0 && cube.currentTransformIndex >= 0 && cube.currentTransformIndex < cube.transformations.Length) cube.AnimateFaceRotation();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            cube = new Cube();
            if (IsCubeNetValid())
            {
                if (AI.Solver.Solve(cube))
                    if (cube.transformations.Length == 0) MessageBox.Show("The cube is already solved.");
                    else MessageBox.Show("I've found a solution of " + cube.transformations.Length + " step (quarter turn metric)");
                else MessageBox.Show("Sorry, I can't solve this cube. Please check if you entered facet colors correctly!\nAlso don't forget that the top row has to be completed!", "Oops");
                cube.reDraw();
                FocusManager.SetFocusedElement(GetWindow(mainViewport), GetWindow(mainViewport));
            }
            else MessageBox.Show("The given coloring is illegal. Please check if you entered facet colors correctly!\nAlso don't forget that the top row has to be completed!", "Error!");
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            cubicleFaceColors = new int[6, 9]{
                {0,0,0,0,0,0,0,0,0},
                {1,1,1,1,1,1,1,1,1},
                {2,2,2,2,2,2,2,2,2},
                {3,3,3,3,3,3,3,3,3},
                {4,4,4,4,4,4,4,4,4},
                {5,5,5,5,5,5,5,5,5}
            };
            for (var i = 0; i < 9; i++)
            {
                ((Button)cubeNetBack.Children[i]).Background = cubeColors[cubicleFaceColors[0, i]];
                ((Button)cubeNetLeft.Children[i]).Background = cubeColors[cubicleFaceColors[1, i]];
                ((Button)cubeNetTop.Children[i]).Background = cubeColors[cubicleFaceColors[2, i]];
                ((Button)cubeNetRight.Children[i]).Background = cubeColors[cubicleFaceColors[3, i]];
                ((Button)cubeNetFront.Children[i]).Background = cubeColors[cubicleFaceColors[4, i]];
                ((Button)cubeNetBottom.Children[i]).Background = cubeColors[cubicleFaceColors[5, i]];
            }
            progressBar.Value = 0;
            cube = new Cube();
            cube.reDraw();
        }
    }
}
