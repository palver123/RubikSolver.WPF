using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rubik01.CubeComponents;
using System.Windows.Media.Media3D;

namespace Rubik01.AI.Recipes
{
    internal class BottomCornerOrientation : Recipe
    {
        private readonly bool direction;

        public BottomCornerOrientation(string code, bool direction1, params int[] ingredients)
        {
            this.code = code;
            this.ingredients = new int[ingredients.Length];
            this.direction = direction1;
            for (var i = 0; i < ingredients.Length; i++) this.ingredients[i] = ingredients[i];
        }

        public override bool TryToApply(Cube cube, params int[] parameters)
        {
            if (ingredients.Length != 6) throw new Exception("Error, BottomCornerOrientation does not have 2 ingredients");
            if (parameters.Length < 2) throw new Exception("Error, BottomCornerOrientation needs 2 arguments");
            var solvedIngredient1 = Solver.solvedCube.cubicles[ingredients[0], ingredients[1], ingredients[2]];
            var solvedIngredient2 = Solver.solvedCube.cubicles[ingredients[3], ingredients[4], ingredients[5]];
            var ingredient1 = cube.GetCubicleByCenter(solvedIngredient1.center);
            var ingredient2 = cube.GetCubicleByCenter(solvedIngredient2.center);
            // state == 2 means corner positions are OK and every preliminary step is completed
            if (cube.state == 2 && ingredient1.inPlace && ingredient2.inPlace)
            {
                var oppositeFaceID = GetOppositeFacet(parameters[1]);
                
                if (direction)
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

                cube.transformations.Append(code);
                return true;
            }
            return false;
        }

        public override void Apply(Cube cube, params int[] parameters)
        {
            if (ingredients.Length != 6) throw new Exception("Error, BottomCornerOrientation does not have 2 ingredients");
            if (parameters.Length < 2) throw new Exception("Error, BottomCornerOrientation needs 2 arguments");
            var solvedIngredient1 = Solver.solvedCube.cubicles[ingredients[0], ingredients[1], ingredients[2]];
            var solvedIngredient2 = Solver.solvedCube.cubicles[ingredients[3], ingredients[4], ingredients[5]];
            var c1 = cube.GetCubicleByCenter(solvedIngredient1.center);
            var c2 = cube.GetCubicleByCenter(solvedIngredient2.center);
            // state == 2 means corner positions are OK and every preliminary step is completed
            if (cube.state == 2 && c1.inPlace && c2.inPlace)
            {
                var oppositeFaceID = GetOppositeFacet(parameters[1]);
                if (direction)
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

                cube.transformations.Append(code);
            }
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
