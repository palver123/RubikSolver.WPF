using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rubik01.CubeComponents;
using System.Windows.Media.Media3D;

namespace Rubik01.AI.Recipes
{
    internal class VerticalEdge : Recipe
    {
        private readonly bool direction;

        public VerticalEdge(string code, bool direction, params int[] ingredients)
        {
            this.code = code;
            this.direction = direction;
            this.ingredients = new int[ingredients.Length];
            for (var i = 0; i < ingredients.Length; i++) this.ingredients[i] = ingredients[i];
        }

        public override void Apply(Cube cube, params int[] parameters)
        {
            if (ingredients.Length != 9) throw new Exception("Error, VerticalEdge does not have 3 ingredients");
            if (parameters.Length < 2) throw new Exception("Error, Vertical Edge needs 2 arguments");
            var ingredient1 = cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[0], ingredients[1], ingredients[2]].center);
            var ingredient2 = cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[3], ingredients[4], ingredients[5]].center);
            var ingredient3 = cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[6], ingredients[7], ingredients[8]].center);
            if (direction)
            {
                var solvedC1 = Solver.solvedCube.GetCubicleByFacetColors(ingredient1.facets);

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

            cube.transformations.Append(code);
        }

        public override bool TryToApply(Cube cube, params int[] parameters)
        {
            if (ingredients.Length != 9) throw new Exception("Error, VerticalEdge does not have 3 ingredients");
            if (parameters.Length < 2) throw new Exception("Error, Vertical Edge needs 2 arguments");
            var ingredient1 = cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[0], ingredients[1], ingredients[2]].center);
            var ingredient2 = cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[3], ingredients[4], ingredients[5]].center);
            var ingredient3 = cube.GetCubicleByCenter(Solver.solvedCube.cubicles[ingredients[6], ingredients[7], ingredients[8]].center);
            if (!ingredient1.inPlace && !ingredient2.inPlace && !ingredient3.inPlace)
            {
                if (direction)
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
