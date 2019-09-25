# RubikSolver.WPF
An educational WPF application to help the average Joe learn solving the Rubik's cube.

# How to build

I currently use Visual Studio 2017, but anything newer than VS 2008 should be able to build it.
Target framework is .NET 4.0 Client profile.

# How to use
After you have built the application run the .exe. You can enter an initial/scrambled cube state by colouring the unfolded cube on the left. My cube has the 6 colours you see when the application starts, but you can override these colours. Unfortunately the application does not store your cube colours persistently (will add this feature soon).
Once you are done painting the cubicles press 'Confirm coloring' and the solver solves it (unless your coloring is invalid). When the solution is ready you can go through it step-by-step, using the arrow keys.
The 3D cube is only for visual feedback now, but I plan to use it for user input too (3D painter tool).

The solver can only solve cubes whose 'top row' is already solved. This is because most people can get one side of the cube done, and my purpose was to show them how to finish the cube. The solver does not find the optimal solution, rather it uses memorizable rotation groups to help the user learn. As stated above, this is an educational application.

# Disclaimer
This was a university project, so I had neither the time nor the knowledge to write clean and maintanable code. I plan to refactor the code along with adding some new features/bugfixes soon!
