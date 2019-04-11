using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rubik01.CubeComponents;
using System.Windows.Media.Media3D;

namespace Rubik01.AI.Recipes
{
    internal class BottomCornerPos: Recipe
    {
        public BottomCornerPos(string code, params int[] ingredients)
        {
            this.code = code;
            this.ingredients = new int[ingredients.Length];
            for (var i = 0; i < ingredients.Length; i++) this.ingredients[i] = ingredients[i];
        }

        public override bool TryToApply(Cube cube, params int[] parameters)
        {
            if (ingredients.Length != 24) throw new Exception("Error, BottomCornerPos does not have 8 ingredients");
            if (parameters.Length < 14) throw new Exception("Error, BottomCornerPos needs 14 parameters");
            var ingredientCubicles = new Cubicle[] {
                cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[0], ingredients[1], ingredients[2]].center),
                cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[3], ingredients[4], ingredients[5]].center),
                cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[6], ingredients[7], ingredients[8]].center),
                cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[9], ingredients[10], ingredients[11]].center),
                cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[12], ingredients[13], ingredients[14]].center),
                cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[15], ingredients[16], ingredients[17]].center),
                cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[18], ingredients[19], ingredients[20]].center),
                cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[21], ingredients[22], ingredients[23]].center),
            };

            // state == 1 means vertical edge positions are OK and every preliminary step is completed
            if (cube.state ==  1 && !ingredientCubicles[0].inPlace && !ingredientCubicles[5].inPlace)
            {
                var solved1 = Solver.solvedCube.GetCubicleByFacetColors(ingredientCubicles[0].facets).center;
                var solved2 = Solver.solvedCube.GetCubicleByFacetColors(ingredientCubicles[5].facets).center;

                if (ingredientCubicles[0].virtualCenter != solved2 || ingredientCubicles[5].virtualCenter != solved1) return false;

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

                cube.transformations.Append(code);
                return true;
            }
            return false;
        }

        private int GetOppositeFacet(int facetID)
        {
            var oppositeFacetID = -1;
            switch (facetID)
            {
                case 0: oppositeFacetID = 4; break;
                case 1: oppositeFacetID = 3; break;
                case 2: oppositeFacetID = 5; break;
                case 3: oppositeFacetID = 1; break;
                case 4: oppositeFacetID = 0; break;
                case 5: oppositeFacetID = 2; break;
                default: break;
            }
            return oppositeFacetID;
        }
    }
}
