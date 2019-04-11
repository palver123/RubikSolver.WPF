using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rubik01.CubeComponents;

namespace Rubik01.AI
{
    internal static class Solver
    {
        public static Cube solvedCube;
        
        public static void Initialize()
        {
            solvedCube = new Cube();
        }

        public static bool Solve(Cube cube)
        {
            if (IsSolved(cube))
                return true;

            CheckPositions(cube);
            var counter = 0;

            #region Vertical edges

            var secondRowEdges = new[] {
                    cube.GetCubicleByCenter(solvedCube._cubicles[1, 0, 0].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[1, 0, 2].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[1, 2, 0].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[1, 2, 2].center)
                };
            while ((!secondRowEdges[0].inPlace || !secondRowEdges[1].inPlace || !secondRowEdges[2].inPlace || !secondRowEdges[3].inPlace))
            {
                cube._state = 1;
                if (Library.recipes[69].TryToApply(cube, 1, 4)) counter = 0;
                if (Library.recipes[70].TryToApply(cube, 1, 4)) counter = 0;
                if (Library.recipes[71].TryToApply(cube, 4, 3)) counter = 0;
                if (Library.recipes[72].TryToApply(cube, 4, 3)) counter = 0;
                if (Library.recipes[73].TryToApply(cube, 3, 0)) counter = 0;
                if (Library.recipes[74].TryToApply(cube, 3, 0)) counter = 0;
                if (Library.recipes[75].TryToApply(cube, 0, 1)) counter = 0;
                if (Library.recipes[76].TryToApply(cube, 0, 1)) counter = 0;

                secondRowEdges = new[] {
                    cube.GetCubicleByCenter(solvedCube._cubicles[1, 0, 0].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[1, 0, 2].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[1, 2, 0].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[1, 2, 2].center)
                };
                CheckPositions(cube);
                if (!secondRowEdges[0].inPlace || !secondRowEdges[1].inPlace || !secondRowEdges[2].inPlace || !secondRowEdges[3].inPlace)
                {
                    if (counter == 3)
                    {
                        if (!secondRowEdges[0].inPlace) Library.recipes[73].Apply(cube, 3, 0);
                        else if (!secondRowEdges[1].inPlace) Library.recipes[74].Apply(cube, 3, 0);
                        else if (!secondRowEdges[2].inPlace) Library.recipes[70].Apply(cube, 1, 4);
                        else if (!secondRowEdges[3].inPlace) Library.recipes[69].Apply(cube, 1, 4);
                        counter = 0;
                    }
                    else
                    {
                        Library.recipes[66].Apply(cube);
                        counter++;
                    }
                }
                CheckPositions(cube);
                secondRowEdges = new[] {
                    cube.GetCubicleByCenter(solvedCube._cubicles[1, 0, 0].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[1, 0, 2].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[1, 2, 0].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[1, 2, 2].center)
                };
            }

            #endregion

            #region Bottom corner positions

            if (IsSolved(cube)) return true;
            counter = 0;
            var bottomCorners = new[] {
                    cube.GetCubicleByCenter(solvedCube._cubicles[0, 0, 0].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[0, 0, 2].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[0, 2, 0].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[0, 2, 2].center)
                };

            while (counter < 4 && (!bottomCorners[0].inPlace || !bottomCorners[1].inPlace || !bottomCorners[2].inPlace || !bottomCorners[3].inPlace))
            {
                Library.recipes[31].TryToApply(cube, 0, 1, 1, 4, 4, 3, 1, 0, 4, 1, 3, 4, 0, 3);
                Library.recipes[32].TryToApply(cube, 1, 4, 4, 3, 3, 0, 4, 1, 3, 4, 0, 3, 1, 0);
                Library.recipes[33].TryToApply(cube, 4, 3, 3, 0, 0, 1, 3, 4, 0, 3, 1, 0, 4, 1);
                Library.recipes[34].TryToApply(cube, 3, 0, 0, 1, 1, 4, 0, 3, 1, 0, 4, 1, 3, 4);
                bottomCorners = new[] {
                    cube.GetCubicleByCenter(solvedCube._cubicles[0, 0, 0].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[0, 0, 2].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[0, 2, 0].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[0, 2, 2].center)
                };
                CheckPositions(cube);
                if (!bottomCorners[0].inPlace || !bottomCorners[1].inPlace || !bottomCorners[2].inPlace || !bottomCorners[3].inPlace) Library.recipes[66].Apply(cube);
                CheckPositions(cube);
                bottomCorners = new[] {
                    cube.GetCubicleByCenter(solvedCube._cubicles[0, 0, 0].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[0, 0, 2].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[0, 2, 0].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[0, 2, 2].center)
                };
                counter++;
            }
            cube._state = 2;

            #endregion

            #region Bottom corner orientation

            if (IsSolved(cube))
                return true;
            var bottomCornerSolved = new bool[4];
            counter = 0;

            while (counter < 3 && (!bottomCornerSolved[0] || !bottomCornerSolved[1] || !bottomCornerSolved[2] || !bottomCornerSolved[3]))
            {
                Library.recipes[35].TryToApply(cube, 1, 4);
                Library.recipes[36].TryToApply(cube, 1, 4);
                Library.recipes[37].TryToApply(cube, 4, 3);
                Library.recipes[38].TryToApply(cube, 4, 3);
                Library.recipes[39].TryToApply(cube, 3, 0);
                Library.recipes[40].TryToApply(cube, 3, 0);
                Library.recipes[41].TryToApply(cube, 0, 1);
                Library.recipes[42].TryToApply(cube, 0, 1);

                bottomCornerSolved[0] = bottomCornerSolved[1] = bottomCornerSolved[2] = bottomCornerSolved[3] = true;
                bottomCorners = new[] {
                    cube.GetCubicleByCenter(solvedCube._cubicles[0, 0, 0].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[0, 0, 2].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[0, 2, 0].center),
                    cube.GetCubicleByCenter(solvedCube._cubicles[0, 2, 2].center)
                };
                for (var j = 0; j < 6; j++)
                {
                    if ((bottomCorners[0].facets[j].normal == solvedCube._cubicles[0, 0, 0].facets[j].normal) && (bottomCorners[0].facets[j].color != solvedCube._cubicles[0, 0, 0].facets[j].color)) bottomCornerSolved[0] = false;
                    if ((bottomCorners[1].facets[j].normal == solvedCube._cubicles[0, 0, 2].facets[j].normal) && (bottomCorners[1].facets[j].color != solvedCube._cubicles[0, 0, 2].facets[j].color)) bottomCornerSolved[1] = false;
                    if ((bottomCorners[2].facets[j].normal == solvedCube._cubicles[0, 2, 0].facets[j].normal) && (bottomCorners[2].facets[j].color != solvedCube._cubicles[0, 2, 0].facets[j].color)) bottomCornerSolved[2] = false;
                    if ((bottomCorners[3].facets[j].normal == solvedCube._cubicles[0, 2, 2].facets[j].normal) && (bottomCorners[3].facets[j].color != solvedCube._cubicles[0, 2, 2].facets[j].color)) bottomCornerSolved[3] = false;
                }
                if (!bottomCornerSolved[0] || !bottomCornerSolved[1] || !bottomCornerSolved[2] || !bottomCornerSolved[3]) Library.recipes[35].Apply(cube, 1, 4);
                counter++;
            }
            cube._state = 3;

            #endregion

            #region Bottom edge positions

            if (IsSolved(cube)) return true;
            // Double swaps
            Library.recipes[43].TryToApply(cube, 1, 3, 0, 4);
            Library.recipes[67].TryToApply(cube, 1, 4, 3, 0);
            Library.recipes[68].TryToApply(cube, 4, 3, 1, 0);

            // 3-way rotation swaps with Vakresz
            Library.recipes[44].TryToApply(cube, 1, 3, 3, 0, 0, 1);
            Library.recipes[45].TryToApply(cube, 1, 3, 3, 0, 0, 1);
            Library.recipes[46].TryToApply(cube, 0, 4, 0, 1, 1, 4);
            Library.recipes[47].TryToApply(cube, 0, 4, 0, 1, 1, 4);
            Library.recipes[48].TryToApply(cube, 1, 3, 1, 4, 4, 3);
            Library.recipes[49].TryToApply(cube, 1, 3, 1, 4, 4, 3);
            Library.recipes[50].TryToApply(cube, 0, 4, 4, 3, 3, 0);
            Library.recipes[51].TryToApply(cube, 0, 4, 4, 3, 3, 0);
            
            // 3-way rotation swaps without Vakresz
            Library.recipes[52].TryToApply(cube, 1, 3, 3, 0, 0, 1);
            Library.recipes[53].TryToApply(cube, 1, 3, 3, 0, 0, 1);
            Library.recipes[54].TryToApply(cube, 0, 4, 0, 1, 4, 1);
            Library.recipes[55].TryToApply(cube, 0, 4, 0, 1, 4, 1);
            Library.recipes[56].TryToApply(cube, 1, 3, 1, 4, 4, 3);
            Library.recipes[57].TryToApply(cube, 1, 3, 1, 4, 4, 3);
            Library.recipes[58].TryToApply(cube, 0, 4, 3, 4, 0, 3);
            Library.recipes[59].TryToApply(cube, 0, 4, 3, 4, 0, 3);
            CheckPositions(cube);
            cube._state = 4;

            #endregion

            #region Bottom edge orientation

            if (IsSolved(cube)) return true;
            Library.recipes[60].TryToApply(cube, 1, 4);
            Library.recipes[61].TryToApply(cube, 4, 3);
            Library.recipes[62].TryToApply(cube, 3, 0);
            Library.recipes[63].TryToApply(cube, 0, 1);
            Library.recipes[64].TryToApply(cube, 1, 3);
            Library.recipes[65].TryToApply(cube, 0, 4);

            #endregion

            return IsSolved(cube);
        }

        public static void CheckPositions(Cube cube)
        {
            foreach (var c1 in solvedCube._cubicles)
            {
                var c2 = cube.GetCubicleByCenter(c1.virtualCenter);
                var setEquals = true;
                foreach (var f1 in c1.facets)
                {
                    var facetContain = false;
                    foreach (var f2 in c2.facets)
                        if (f2.color == f1.color) facetContain = true;
                    if (!facetContain) setEquals = false;
                }
                c2.inPlace = setEquals;
            } 
        }

        private static bool IsSolved(Cube cube)
        {
            foreach (var c1 in solvedCube._cubicles)
            {
                var c2 = cube.GetCubicleByCenter(c1.virtualCenter);
                for (var i = 0; i < 6; i++) if ((c1.facets[i].normal == c2.facets[i].normal) && (c1.facets[i].color != c2.facets[i].color)) return false;
            }
            return true;
        }
    }
}
