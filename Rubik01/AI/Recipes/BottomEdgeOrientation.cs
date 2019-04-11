using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rubik01.CubeComponents;

namespace Rubik01.AI.Recipes
{
    internal class BottomEdgeOrientation : Recipe
    {
        public BottomEdgeOrientation(string code, params int[] ingredients) :
            base(code, ingredients)
        {
            CheckIngredientCount(6);
        }

        public override bool TryToApply(Cube cube, params int[] parameters)
        {
            CheckParameterCount(parameters.Length);

            var ingredient1 = cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[0], _ingredients[1], _ingredients[2]].center);
            var ingredient2 = cube.GetCubicleByCenter(Solver.solvedCube._cubicles[_ingredients[3], _ingredients[4], _ingredients[5]].center);

            // _state == 4 means bottom edge positions are OK and every preliminary step is completed
            if (cube._state ==  4 && ingredient1.inPlace && ingredient2.inPlace && ingredient1.GetFacet(5).color != 5 && ingredient2.GetFacet(5).color != 5)
            {
                ingredient1.SwapFacets(5, parameters[0]);
                ingredient2.SwapFacets(5, parameters[1]);
                cube._transformations.Append(_code);
                return true;
            }
            return false;
        }

        protected override void CheckParameterCount(int paramCount)
        {
            if (paramCount < 2)
                throw new ArgumentException(@"Error: Vakresz.TryToApply() need 2 parameters");
        }
    }
}
