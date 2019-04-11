using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rubik01.CubeComponents;
using System.Windows.Media.Media3D;

namespace Rubik01.AI.Recipes
{
    internal class DoubleEdgeSwap : Recipe
    {
        public DoubleEdgeSwap(string code, params int[] ingredients)
        {
            this.code = code;
            this.ingredients = new int[ingredients.Length];
            for (var i = 0; i < ingredients.Length; i++) this.ingredients[i] = ingredients[i];
        }

        public override bool TryToApply(Cube cube, params int[] parameters)
        {
            if (ingredients.Length != 12) throw new Exception("Error, DoubleEdgeSwap does not have 4 ingredients");
            var ingredient1 = cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[0], ingredients[1], ingredients[2]].center);
            var ingredient2 = cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[3], ingredients[4], ingredients[5]].center);
            var ingredient3 = cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[6], ingredients[7], ingredients[8]].center);
            var ingredient4 = cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[9], ingredients[10], ingredients[11]].center);
            // state == 3 means bottom corner orientations are OK and every preliminary step is completed
            if (cube.state == 3 && !ingredient1.inPlace && !ingredient2.inPlace && !ingredient3.inPlace && !ingredient4.inPlace)
            {
                var solvedC1 = Solver.solvedCube.GetCubicleByFacetColors(ingredient1.facets).center;
                var solvedC2 = Solver.solvedCube.GetCubicleByFacetColors(ingredient2.facets).center;
                var solvedC3 = Solver.solvedCube.GetCubicleByFacetColors(ingredient3.facets).center;
                var solvedC4 = Solver.solvedCube.GetCubicleByFacetColors(ingredient4.facets).center;

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

                cube.transformations.Append(code);
                return true;
            }
            return false;
        }
    }
}
