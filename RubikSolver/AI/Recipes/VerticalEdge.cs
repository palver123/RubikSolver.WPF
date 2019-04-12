using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RubikSolver.CubeComponents;

namespace RubikSolver.AI.Recipes
{
    internal class VerticalEdge : RecipeWithDirection
    {
        public VerticalEdge(string code, bool direction, params int[] ingredients):
            base(code, direction, ingredients)
        {
            CheckIngredientCount(9);
        }

        public override void Apply(Cube cube, params int[] parameters)
        {
            CheckParameterCount(parameters.Length);

            var ingredient1 = cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[0], _ingredients[1], _ingredients[2]].center);
            var ingredient2 = cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[3], _ingredients[4], _ingredients[5]].center);
            var ingredient3 = cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[6], _ingredients[7], _ingredients[8]].center);
            if (_direction)
            {
                var temp = ingredient1.virtualCenter;
                ingredient1.virtualCenter = ingredient2.virtualCenter;
                ingredient2.virtualCenter = ingredient3.virtualCenter;
                ingredient3.virtualCenter = temp;

                ingredient1.SwapFacets(parameters[0], GetOppositeFacet(parameters[0]));
                ingredient1.SwapFacets(parameters[1], 5);
                ingredient2.SwapFacets(parameters[1], GetOppositeFacet(parameters[1]));
                ingredient2.SwapFacets(5, GetOppositeFacet(parameters[0]));
                ingredient3.SwapFacets(5, GetOppositeFacet(parameters[1]));
                ingredient3.SwapFacets(GetOppositeFacet(parameters[1]), parameters[0]);
            }
            else
            {
                var temp = ingredient1.virtualCenter;
                ingredient1.virtualCenter = ingredient3.virtualCenter;
                ingredient3.virtualCenter = ingredient2.virtualCenter;
                ingredient2.virtualCenter = temp;

                ingredient1.SwapFacets(parameters[0], 5);
                ingredient1.SwapFacets(parameters[1], GetOppositeFacet(parameters[1]));
                ingredient2.SwapFacets(parameters[0], GetOppositeFacet(parameters[0]));
                ingredient2.SwapFacets(5, parameters[1]);
                ingredient3.SwapFacets(5, GetOppositeFacet(parameters[1]));
                ingredient3.SwapFacets(GetOppositeFacet(parameters[1]), GetOppositeFacet(parameters[0]));
            }

            cube._transformations.Append(_code);
        }

        protected override void CheckParameterCount(int paramCount)
        {
            if (paramCount < 2)
                throw new ArgumentException($"Error: {nameof(VerticalEdge)} needs 2 arguments");
        }

        public override bool TryToApply(Cube cube, params int[] parameters)
        {
            CheckParameterCount(parameters.Length);

            var ingredient1 = cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[0], _ingredients[1], _ingredients[2]].center);
            var ingredient2 = cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[3], _ingredients[4], _ingredients[5]].center);
            var ingredient3 = cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[6], _ingredients[7], _ingredients[8]].center);
            if (!ingredient1.inPlace && !ingredient2.inPlace && !ingredient3.inPlace)
            {
                if (_direction)
                {
                    var solvedC1 = Solver.solvedCube.GetCubicleByFacetColors(ingredient1.facets);
                    if (ingredient2.virtualCenter != solvedC1.center || ingredient1.GetFacet(parameters[0]).color != solvedC1.GetFacet(GetOppositeFacet(parameters[0])).color) return false;

                    var temp = ingredient1.virtualCenter;
                    ingredient1.virtualCenter = ingredient2.virtualCenter;
                    ingredient2.virtualCenter = ingredient3.virtualCenter;
                    ingredient3.virtualCenter = temp;

                    ingredient1.SwapFacets(parameters[0], GetOppositeFacet(parameters[0]));
                    ingredient1.SwapFacets(parameters[1], 5);
                    ingredient2.SwapFacets(parameters[1], GetOppositeFacet(parameters[1]));
                    ingredient2.SwapFacets(5, GetOppositeFacet(parameters[0]));
                    ingredient3.SwapFacets(5, GetOppositeFacet(parameters[1]));
                    ingredient3.SwapFacets(GetOppositeFacet(parameters[1]), parameters[0]);
                }
                else
                {
                    var solvedC2 = Solver.solvedCube.GetCubicleByFacetColors(ingredient2.facets);
                    if (ingredient1.virtualCenter != solvedC2.center || ingredient2.GetFacet(GetOppositeFacet(parameters[0])).color != solvedC2.GetFacet(parameters[0]).color) return false;

                    var temp = ingredient1.virtualCenter;
                    ingredient1.virtualCenter = ingredient3.virtualCenter;
                    ingredient3.virtualCenter = ingredient2.virtualCenter;
                    ingredient2.virtualCenter = temp;

                    ingredient1.SwapFacets(parameters[0], 5);
                    ingredient1.SwapFacets(parameters[1], GetOppositeFacet(parameters[1]));
                    ingredient2.SwapFacets(parameters[0], GetOppositeFacet(parameters[0]));
                    ingredient2.SwapFacets(5, parameters[1]);
                    ingredient3.SwapFacets(5, GetOppositeFacet(parameters[1]));
                    ingredient3.SwapFacets(GetOppositeFacet(parameters[1]), GetOppositeFacet(parameters[0]));
                }

                cube._transformations.Append(_code);
                return true;
            }
            return false;
        }
    }
}
