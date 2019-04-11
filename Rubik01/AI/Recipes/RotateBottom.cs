using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rubik01.CubeComponents;
using System.Windows.Media.Media3D;

namespace Rubik01.AI.Recipes
{
    internal class RotateBottom : IRecipe
    {
        public bool TryToApply(Cube cube, params int[] parameters) { return false; }

        public void Apply(Cube cube, params int[] parameters)
        {
            var ingredientCubicles = new[] {
                cube.GetCubicleByCenter(Solver.solvedCube._cubicles[0, 0, 0].center),
                cube.GetCubicleByCenter(Solver.solvedCube._cubicles[0, 0, 1].center),
                cube.GetCubicleByCenter(Solver.solvedCube._cubicles[0, 0, 2].center),
                cube.GetCubicleByCenter(Solver.solvedCube._cubicles[0, 1, 0].center),
                cube.GetCubicleByCenter(Solver.solvedCube._cubicles[0, 1, 1].center),
                cube.GetCubicleByCenter(Solver.solvedCube._cubicles[0, 1, 2].center),
                cube.GetCubicleByCenter(Solver.solvedCube._cubicles[0, 2, 0].center),
                cube.GetCubicleByCenter(Solver.solvedCube._cubicles[0, 2, 1].center),
                cube.GetCubicleByCenter(Solver.solvedCube._cubicles[0, 2, 2].center),
            };

            var rotationMatrix = new Matrix3D(0, 1, 0, 0,
                                                   -1, 0, 0, 0,
                                                   0, 0, 1, 0,
                                                   0, 0, 0, 1);
            var centerOfRotation = Solver.solvedCube._cubicles[0, 1, 1].center;
            for (var i = 0; i < 9; i++)
            {
                ingredientCubicles[i].virtualCenter = (ingredientCubicles[i].virtualCenter - centerOfRotation) * rotationMatrix + centerOfRotation;
                ingredientCubicles[i].virtualCenter.X = Math.Round(ingredientCubicles[i].virtualCenter.X);
                ingredientCubicles[i].virtualCenter.Y = Math.Round(ingredientCubicles[i].virtualCenter.Y);
                ingredientCubicles[i].virtualCenter.Z = Math.Round(ingredientCubicles[i].virtualCenter.Z);
            }

            ingredientCubicles[0].SwapFacets(1, 4);
            ingredientCubicles[0].SwapFacets(0, 1);
            ingredientCubicles[1].SwapFacets(1, 0);
            ingredientCubicles[2].SwapFacets(0, 3);
            ingredientCubicles[2].SwapFacets(3, 1);
            ingredientCubicles[3].SwapFacets(1, 4);
            ingredientCubicles[5].SwapFacets(3, 0);
            ingredientCubicles[6].SwapFacets(1, 4);
            ingredientCubicles[6].SwapFacets(1, 3);
            ingredientCubicles[7].SwapFacets(4, 3);
            ingredientCubicles[8].SwapFacets(3, 0);
            ingredientCubicles[8].SwapFacets(4, 3);

            cube._transformations.Append("D");
        }
    }
}
