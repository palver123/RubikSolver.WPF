using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rubik01.CubeComponents;

namespace Rubik01.AI.Recipes
{
    internal class BottomCornerPos: Recipe
    {
        public BottomCornerPos(string code, params int[] ingredients):
            base(code, ingredients)
        {
            CheckIngredientCount(24);
        }

        public override bool TryToApply(Cube cube, params int[] parameters)
        {
            CheckParameterCount(parameters.Length);

            var ingredientCubicles = new[] {
                cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[0], _ingredients[1], _ingredients[2]].center),
                cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[3], _ingredients[4], _ingredients[5]].center),
                cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[6], _ingredients[7], _ingredients[8]].center),
                cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[9], _ingredients[10], _ingredients[11]].center),
                cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[12], _ingredients[13], _ingredients[14]].center),
                cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[15], _ingredients[16], _ingredients[17]].center),
                cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[18], _ingredients[19], _ingredients[20]].center),
                cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[21], _ingredients[22], _ingredients[23]].center),
            };

            // _state == 1 means vertical edge positions are OK and every preliminary step is completed
            if (cube._state ==  1 && !ingredientCubicles[0].inPlace && !ingredientCubicles[5].inPlace)
            {
                var solved1 = Solver.solvedCube.GetCubicleByFacetColors(ingredientCubicles[0].facets).center;
                var solved2 = Solver.solvedCube.GetCubicleByFacetColors(ingredientCubicles[5].facets).center;
                if (ingredientCubicles[0].virtualCenter != solved2 || ingredientCubicles[5].virtualCenter != solved1)
                    return false;

                var temp = ingredientCubicles[1].virtualCenter;
                ingredientCubicles[1].virtualCenter = ingredientCubicles[4].virtualCenter;
                ingredientCubicles[4].virtualCenter = ingredientCubicles[6].virtualCenter;
                ingredientCubicles[6].virtualCenter = ingredientCubicles[2].virtualCenter;
                ingredientCubicles[2].virtualCenter = temp;
                temp = ingredientCubicles[0].virtualCenter;
                ingredientCubicles[0].virtualCenter = ingredientCubicles[5].virtualCenter;
                ingredientCubicles[5].virtualCenter = temp;

                ingredientCubicles[0].SwapFacets(parameters[0], GetOppositeFacet(parameters[0]));
                ingredientCubicles[0].SwapFacets(parameters[1], 5);
                ingredientCubicles[5].SwapFacets(parameters[2], parameters[3]);
                ingredientCubicles[5].SwapFacets(parameters[3], GetOppositeFacet(parameters[3]));
                ingredientCubicles[7].SwapFacets(parameters[4], 5);
                ingredientCubicles[7].SwapFacets(parameters[4], parameters[5]);

                ingredientCubicles[2].SwapFacets(parameters[6], parameters[7]);
                ingredientCubicles[6].SwapFacets(parameters[8], parameters[9]);
                ingredientCubicles[4].SwapFacets(parameters[10], parameters[11]);
                ingredientCubicles[1].SwapFacets(parameters[12], parameters[13]);

                cube._transformations.Append(_code);
                return true;
            }
            return false;
        }

        protected override void CheckParameterCount(int paramCount)
        {
            if (paramCount < 14)
                throw new ArgumentException($"Error: {nameof(BottomCornerPos)} needs 14 arguments");
        }
    }
}
