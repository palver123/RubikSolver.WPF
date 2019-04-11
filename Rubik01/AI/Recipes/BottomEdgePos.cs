using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rubik01.CubeComponents;
using System.Windows.Media.Media3D;

namespace Rubik01.AI.Recipes
{
    internal class BottomEdgePos : Recipe
    {
        private readonly bool direction;

        public BottomEdgePos(string code, bool direction, params int[] ingredients)
        {
            this.code = code;
            this.direction = direction;
            this.ingredients = new int[ingredients.Length];
            for (var i = 0; i < ingredients.Length; i++) this.ingredients[i] = ingredients[i];
        }

        public override bool TryToApply(Cube cube, params int[] parameters)
        {
            if (ingredients.Length != 9) throw new Exception("Error, BottomRowPos does not have 3 ingredients");
            if (parameters.Length < 6) throw new Exception("Error, BottomRowPos needs 6 arguments");
            var ingredient1 = cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[0], ingredients[1], ingredients[2]].center);
            var ingredient2 = cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[3], ingredients[4], ingredients[5]].center);
            var ingredient3 = cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[6], ingredients[7], ingredients[8]].center);
            if (!ingredient1.inPlace && !ingredient2.inPlace && !ingredient3.inPlace)
            {
                var solvedC1 = Solver.solvedCube.GetCubicleByFacetColors(ingredient1.facets).center;
                var solvedC2 = Solver.solvedCube.GetCubicleByFacetColors(ingredient2.facets).center;
                var solvedC3 = Solver.solvedCube.GetCubicleByFacetColors(ingredient3.facets).center;

                if (direction)
                {
                    // This ensures that a rotation to the right will help
                    if (ingredient2.GetFacet(5).color == 5 || ingredient3.GetFacet(5).color == 5 || ingredient1.virtualCenter != solvedC3 || ingredient2.virtualCenter != solvedC1 || ingredient3.virtualCenter != solvedC2) return false;

                    var temp = ingredient1.virtualCenter;
                    ingredient1.virtualCenter = ingredient2.virtualCenter;
                    ingredient2.virtualCenter = ingredient3.virtualCenter;
                    ingredient3.virtualCenter = temp;

                    ingredient1.SwapFacets(parameters[0], parameters[1]);
                    ingredient2.SwapFacets(parameters[2], parameters[3]);
                    ingredient2.SwapFacets(5, parameters[3]);
                    ingredient3.SwapFacets(parameters[4], parameters[5]);
                    ingredient3.SwapFacets(5, parameters[5]);
                }
                else
                {
                    // This ensures that a rotation to the right will help
                    if  (ingredient1.GetFacet(5).color == 5 || ingredient3.GetFacet(5).color == 5 || ingredient1.virtualCenter != solvedC2 || ingredient2.virtualCenter != solvedC3 || ingredient3.virtualCenter != solvedC1) return false;

                    var temp = ingredient1.virtualCenter;
                    ingredient1.virtualCenter = ingredient3.virtualCenter;
                    ingredient3.virtualCenter = ingredient2.virtualCenter;
                    ingredient2.virtualCenter = temp;

                    ingredient1.SwapFacets(parameters[4], parameters[5]);
                    ingredient1.SwapFacets(5, parameters[1]);
                    ingredient2.SwapFacets(parameters[0], parameters[1]);
                    ingredient3.SwapFacets(parameters[2], parameters[3]);
                    ingredient3.SwapFacets(5, parameters[3]);
                }

                cube.transformations.Append(code);
                return true;
            }
            return false;
        }
    }
}
