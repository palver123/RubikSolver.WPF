using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rubik01.CubeComponents;
using System.Windows.Media.Media3D;

namespace Rubik01.AI.Recipes
{
    internal class BottomEdgeOrientation : Recipe
    {
        public BottomEdgeOrientation(string code, params int[] ingredients)
        {
            this.code = code;
            this.ingredients = new int[ingredients.Length];
            for (var i = 0; i < ingredients.Length; i++) this.ingredients[i] = ingredients[i];
        }

        public override bool TryToApply(Cube cube, params int[] parameters)
        {
            if (ingredients.Length != 6) throw new Exception("Error, vakresz does not have 2 ingredients");
            if (parameters.Length < 2) throw new Exception("Error, Vakresz.Apply() need 2 parameters");
            var ingredient1 = cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[0], ingredients[1], ingredients[2]].center);
            var ingredient2 = cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[3], ingredients[4], ingredients[5]].center);
            // state == 4 means bottom edge positions are OK and every preliminary step is completed
            if (cube.state ==  4 && ingredient1.inPlace && ingredient2.inPlace && ingredient1.GetFacet(5).color != 5 && ingredient2.GetFacet(5).color != 5)
            {
                ingredient1.SwapFacets(5, parameters[0]);
                ingredient2.SwapFacets(5, parameters[1]);
                cube.transformations.Append(code);
                return true;
            }
            return false;
        }
    }
}
