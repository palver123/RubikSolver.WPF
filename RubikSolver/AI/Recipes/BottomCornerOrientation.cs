using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RubikSolver.CubeComponents;

namespace RubikSolver.AI.Recipes
{
    internal class BottomCornerOrientation : RecipeWithDirection
    {
        public BottomCornerOrientation(string code, bool direction, params int[] ingredients):
            base(code, direction, ingredients)
        {
            CheckIngredientCount(6);
        }

        public override bool TryToApply(Cube cube, params int[] parameters)
        {
            CheckParameterCount(parameters.Length);

            var solvedIngredient1 = Solver.solvedCube._cubicles[_ingredients[0], _ingredients[1], _ingredients[2]];
            var solvedIngredient2 = Solver.solvedCube._cubicles[_ingredients[3], _ingredients[4], _ingredients[5]];
            var ingredient1 = cube.GetCubicleByCenter(solvedIngredient1.center);
            var ingredient2 = cube.GetCubicleByCenter(solvedIngredient2.center);

            if (cube._state == Cube.Completeness.BottomCornersOK && ingredient1.inPlace && ingredient2.inPlace)
            {
                var oppositeFaceID = GetOppositeFacet(parameters[1]);
                
                if (_direction)
                {
                    // This ensures that the corner cubicles need a rotation
                    if (ingredient1.GetFacet(5).color != solvedIngredient1.GetFacet(parameters[0]).color || ingredient2.GetFacet(5).color != solvedIngredient2.GetFacet(parameters[0]).color) return false;
                    ingredient1.SwapFacets(parameters[0], parameters[1]);
                    ingredient1.SwapFacets(parameters[0], 5);
                    ingredient2.SwapFacets(parameters[0], oppositeFaceID);
                    ingredient2.SwapFacets(parameters[0], 5);
                }
                else
                {
                    // This ensures that the corner cubicles need a rotation
                    if (ingredient1.GetFacet(5).color != solvedIngredient1.GetFacet(parameters[1]).color || ingredient2.GetFacet(5).color != solvedIngredient2.GetFacet(oppositeFaceID).color) return false;
                    ingredient1.SwapFacets(parameters[0], parameters[1]);
                    ingredient1.SwapFacets(parameters[1], 5);
                    ingredient2.SwapFacets(parameters[0], oppositeFaceID);
                    ingredient2.SwapFacets(oppositeFaceID, 5);
                }

                cube._transformations.Append(_code);
                return true;
            }
            return false;
        }

        public override void Apply(Cube cube, params int[] parameters)
        {
            CheckParameterCount(parameters.Length);

            var solvedIngredient1 = Solver.solvedCube._cubicles[_ingredients[0], _ingredients[1], _ingredients[2]];
            var solvedIngredient2 = Solver.solvedCube._cubicles[_ingredients[3], _ingredients[4], _ingredients[5]];
            var c1 = cube.GetCubicleByCenter(solvedIngredient1.center);
            var c2 = cube.GetCubicleByCenter(solvedIngredient2.center);

            if (cube._state == Cube.Completeness.BottomCornersOK && c1.inPlace && c2.inPlace)
            {
                var oppositeFaceID = GetOppositeFacet(parameters[1]);
                if (_direction)
                {
                    c1.SwapFacets(parameters[0], parameters[1]);
                    c1.SwapFacets(parameters[0], 5);
                    c2.SwapFacets(parameters[0], oppositeFaceID);
                    c2.SwapFacets(parameters[0], 5);
                }
                else
                {
                    c1.SwapFacets(parameters[0], parameters[1]);
                    c1.SwapFacets(parameters[1], 5);
                    c2.SwapFacets(parameters[0], oppositeFaceID);
                    c2.SwapFacets(oppositeFaceID, 5);
                }

                cube._transformations.Append(_code);
            }
        }

        protected override void CheckParameterCount(int paramCount)
        {
            if (paramCount < 2)
                throw new ArgumentException($"Error: {nameof(BottomCornerOrientation)} needs 2 arguments");
        }
    }
}
