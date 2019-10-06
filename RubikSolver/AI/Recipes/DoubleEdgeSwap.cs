using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RubikSolver.CubeComponents;

namespace RubikSolver.AI.Recipes
{
    internal class DoubleEdgeSwap : Recipe
    {
        public DoubleEdgeSwap(string code, params int[] ingredients):
            base(code, ingredients)
        {
        }

        public override bool TryToApply(Cube cube, params int[] parameters)
        {
            CheckParameterCount(parameters.Length);

            var ingredient1 = cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[0], _ingredients[1], _ingredients[2]]._center);
            var ingredient2 = cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[3], _ingredients[4], _ingredients[5]]._center);
            var ingredient3 = cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[6], _ingredients[7], _ingredients[8]]._center);
            var ingredient4 = cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[9], _ingredients[10], _ingredients[11]]._center);

            if (cube._state == Cube.Completeness.BottomEdgePositionsOK && !ingredient1.inPlace && !ingredient2.inPlace && !ingredient3.inPlace && !ingredient4.inPlace)
            {
                var solvedC1 = Solver.solvedCube.GetCubicleByFacetColors(ingredient1.facets)._center;
                var solvedC2 = Solver.solvedCube.GetCubicleByFacetColors(ingredient2.facets)._center;
                var solvedC3 = Solver.solvedCube.GetCubicleByFacetColors(ingredient3.facets)._center;
                var solvedC4 = Solver.solvedCube.GetCubicleByFacetColors(ingredient4.facets)._center;

                if (ingredient1.virtualCenter != solvedC2 || ingredient2.virtualCenter != solvedC1 || ingredient3.virtualCenter != solvedC4 || ingredient4.virtualCenter != solvedC3) return false;

                var temp = ingredient1.virtualCenter;
                ingredient1.virtualCenter = ingredient2.virtualCenter;
                ingredient2.virtualCenter = temp;
                temp = ingredient3.virtualCenter;
                ingredient3.virtualCenter = ingredient4.virtualCenter;
                ingredient4.virtualCenter = temp;

                ingredient1.SwapFacets(parameters[0], parameters[1]);
                ingredient2.SwapFacets(parameters[1], parameters[0]);
                ingredient3.SwapFacets(parameters[2], parameters[3]);
                ingredient4.SwapFacets(parameters[3], parameters[2]);

                cube._transformations.Append(_code);
                return true;
            }
            return false;
        }

        protected override void CheckParameterCount(int paramCount)
        {
            if (paramCount < 4)
                throw new ArgumentException($"Error: {nameof(BottomEdgePosLong)} needs 4 arguments");
        }
    }
}
