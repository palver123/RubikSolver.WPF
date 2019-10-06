using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RubikSolver.CubeComponents;

namespace RubikSolver.AI.Recipes
{
    internal class BottomEdgePosLong : RecipeWithDirection
    {
        public BottomEdgePosLong(string code, bool direction, params int[] ingredients):
            base(code, direction, ingredients)
        {
            CheckIngredientCount(9);
        }

        public override bool TryToApply(Cube cube, params int[] parameters)
        {
            CheckParameterCount(parameters.Length);

            var ingredient1 = cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[0], _ingredients[1], _ingredients[2]]._center);
            var ingredient2 = cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[3], _ingredients[4], _ingredients[5]]._center);
            var ingredient3 = cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[6], _ingredients[7], _ingredients[8]]._center);

            if (cube._state == Cube.Completeness.BottomEdgePositionsOK && !ingredient1.inPlace && !ingredient2.inPlace && !ingredient3.inPlace /*&& c1.GetFacet(5).color == 5 && c2.GetFacet(5).color == 5 && c3.GetFacet(5).color == 5*/)
            {
                var solvedC1 = Solver.solvedCube.GetCubicleByFacetColors(ingredient1.facets)._center;
                var solvedC2 = Solver.solvedCube.GetCubicleByFacetColors(ingredient2.facets)._center;
                var solvedC3 = Solver.solvedCube.GetCubicleByFacetColors(ingredient3.facets)._center;

                if (_direction)
                {
                    // This ensures that a rotation to the right will help
                    if (ingredient1.virtualCenter != solvedC3 || ingredient2.virtualCenter != solvedC1 || ingredient3.virtualCenter != solvedC2) return false;

                    var temp = ingredient1.virtualCenter;
                    ingredient1.virtualCenter = ingredient2.virtualCenter;
                    ingredient2.virtualCenter = ingredient3.virtualCenter;
                    ingredient3.virtualCenter = temp;

                    ingredient1.SwapFacets(parameters[0], parameters[1]);
                    ingredient2.SwapFacets(parameters[2], parameters[3]);
                    ingredient3.SwapFacets(parameters[4], parameters[5]);
                }
                else
                {
                    // This ensures that a rotation to the right will help
                    if (ingredient1.virtualCenter != solvedC2 || ingredient2.virtualCenter != solvedC3 || ingredient3.virtualCenter != solvedC1) return false;

                    var temp = ingredient1.virtualCenter;
                    ingredient1.virtualCenter = ingredient3.virtualCenter;
                    ingredient3.virtualCenter = ingredient2.virtualCenter; 
                    ingredient2.virtualCenter = temp;

                    ingredient1.SwapFacets(parameters[4], parameters[5]);
                    ingredient2.SwapFacets(parameters[0], parameters[1]);
                    ingredient3.SwapFacets(parameters[2], parameters[3]);
                }

                cube._transformations.Append(_code);
                return true;
            }
            return false;
        }

        protected override void CheckParameterCount(int paramCount)
        {
            if (paramCount < 6)
                throw new ArgumentException($"Error: {nameof(BottomEdgePosLong)} needs 6 arguments");
        }
    }
}
