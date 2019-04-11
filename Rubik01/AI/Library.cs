using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rubik01.AI.Recipes;

namespace Rubik01.AI
{
    internal static class Library
    {
        public static IRecipe[] recipes = new IRecipe[100];

        static Library()
        {
            #region Fixing edge positions
           /* // orange-green edge from inplace/1-turn/2-turns/1-inverse-turn
            recipes[0] = "rDRDFdf";
            recipes[1] = "DrDRDFdf"; 
            recipes[2] = "DDrDRDFdf";
            recipes[3] = "drDRDFdf";

            recipes[4] = "LdldfDf";
            recipes[5] = "DLdldfDf";
            recipes[6] = "DDLdldfDf";
            recipes[7] = "dLdldfDf";

            // green-red edge from inplace/1-turn/2-turns/1-inverse-turn
            recipes[8] = "bDBDRdr";
            recipes[9] = "DbDBDRdr";
            recipes[10] = "DDbDBDRdr";
            recipes[11] = "dbDBDRdr";

            recipes[12] = "FdfdrDR";
            recipes[13] = "DFdfdrDR";
            recipes[14] = "DDFdfdrDR";
            recipes[15] = "dFdfdrDR";

            // red-blue edge from inplace/1-turn/2-turns/1-inverse-turn
            recipes[16] = "lDLDBdb";
            recipes[17] = "DlDLDBdb";
            recipes[18] = "DDlDLDBdb";
            recipes[19] = "dlDLDBdb";

            recipes[20] = "RdrdfDF";
            recipes[21] = "DRdrdfDF";
            recipes[22] = "DDRdrdfDF";
            recipes[23] = "dRdrdfDF";

            // blue-orange edge from inplace/1-turn/2-turns/1-inverse-turn
            recipes[24] = "fDFDLdl";
            recipes[25] = "DfDFDLdl";
            recipes[26] = "DDfDFDLdl";
            recipes[27] = "dfDFDLdl";

            recipes[28] = "BdbdlDL";
            recipes[29] = "DBdbdlDL";
            recipes[30] = "DDBdbdlDL";
            recipes[31] = "dBdbdlDL";
*/
            #endregion

            #region Fixing bottom corner positions

            recipes[31] = new BottomCornerPos("LrDldRDLdld", 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 1, 0, 1, 2, 0, 2, 0, 0, 2, 1, 0, 2, 2);
            recipes[32] = new BottomCornerPos("FbDfdBDFdfd", 0, 2, 0, 0, 1, 0, 0, 2, 1, 0, 1, 1, 0, 0, 1, 0, 2, 2, 0, 1, 2, 0, 0, 2);
            recipes[33] = new BottomCornerPos("RlDrdLDRdrd", 0, 2, 2, 0, 2, 1, 0, 1, 2, 0, 1 ,1, 0, 1, 0, 0, 0, 2, 0, 0, 1, 0, 0, 0);
            recipes[34] = new BottomCornerPos("BfDbdFDBdbd", 0, 0, 2, 0, 1, 2, 0, 0, 1, 0, 1, 1, 0, 2, 1, 0, 0, 0, 0, 1, 0, 0, 2, 0);
            
            #endregion
            
            #region Fixing bottom corner orientation

            recipes[35] = new BottomCornerOrientation("rdRdrddRLDlDLDDl", true, 0, 2, 0, 0, 0, 0);
            recipes[36] = new BottomCornerOrientation("LddldLdlrDDRDrDR", false, 0, 2, 0, 0, 0, 0);

            recipes[37] = new BottomCornerOrientation("bdBdbddBFDfDFDDf", true, 0, 2, 2, 0, 2, 0);
            recipes[38] = new BottomCornerOrientation("FddfdFdfbDDBDbDB", false, 0, 2, 2, 0, 2, 0);

            recipes[39] = new BottomCornerOrientation("ldLdlddLRDrDRDDr", true, 0, 0, 2, 0, 2, 2);
            recipes[40] = new BottomCornerOrientation("RddrdRdrlDDLDlDL", false, 0, 0, 2, 0, 2, 2);

            recipes[41] = new BottomCornerOrientation("fdFdfddFBDbDBDDb", true, 0, 0, 0, 0, 0, 2);
            recipes[42] = new BottomCornerOrientation("BddbdBdbfDDFDfDF", false, 0, 0, 0, 0, 0, 2);
            
            #endregion
            
            #region Fixing bottom edges position

            // swap parallel edges (double-swap)
            recipes[43] = new DoubleEdgeSwap("llRRullRRddllRRullRR", 0, 1, 0, 0, 1, 2, 0, 0, 1, 0, 2, 1);
            // swap arbitrary edges (double-swap)
            recipes[67] = new DoubleEdgeSwap("BBRRrLdULuDFFlRUrrbb", 0, 1, 0, 0, 2, 1, 0, 0, 1, 0, 1, 2);
            recipes[68] = new DoubleEdgeSwap("llbbbFdUFDurrBfUbbll", 0, 2, 1, 0, 1, 2, 0, 1, 0, 0, 0, 1);
            
            // 3-way rotation swap with "vakrész"
            recipes[44] = new BottomEdgePos("lRFLrddlRFLr", true, 0, 1, 0, 0, 1, 2, 0, 0, 1);
            recipes[45] = new BottomEdgePos("lRfrLddRlfLr", false, 0, 1, 0, 0, 1, 2, 0, 0, 1);
            recipes[46] = new BottomEdgePos("fBRFbddfBRFb", true, 0, 2, 1, 0, 0, 1, 0, 1, 0);
            recipes[47] = new BottomEdgePos("fBrbFddBfrFb", false,  0, 2, 1, 0, 0, 1, 0, 1, 0);
            recipes[48] = new BottomEdgePos("rLBRlddrLBRl", true, 0, 1, 2, 0, 1, 0, 0, 2, 1);
            recipes[49] = new BottomEdgePos("rLblRddLrbRl", false, 0, 1, 2, 0, 1, 0, 0, 2, 1);
            recipes[50] = new BottomEdgePos("bFLBfddbFLBf", true, 0, 0, 1, 0, 2, 1, 0, 1, 2);
            recipes[51] = new BottomEdgePos("bFlfBddFblBf", false, 0, 0, 1, 0, 2, 1, 0, 1, 2);
            
            // 3-way rotation swap without "vakrész"
            recipes[52] = new BottomEdgePosLong("rrLLURlbbrLURRll", false, 0, 1, 0, 0, 1 ,2, 0, 0, 1);
            recipes[53] = new BottomEdgePosLong("rrLLulRbbLruRRll", true, 0, 1, 0, 0, 1, 2, 0, 0, 1);
            recipes[54] = new BottomEdgePosLong("bbFFUBfllbFUBBff", false, 0, 2, 1, 0, 0, 1, 0, 1, 0);
            recipes[55] = new BottomEdgePosLong("bbFFufBllFbuFFbb", true,  0, 2, 1, 0, 0, 1, 0, 1, 0);
            recipes[56] = new BottomEdgePosLong("llRRULrfflRULLrr", false, 0, 1, 2, 0, 1, 0, 0, 2, 1);
            recipes[57] = new BottomEdgePosLong("llRRurLffRluLLrr", true, 0, 1, 2, 0, 1, 0, 0, 2, 1 );
            recipes[58] = new BottomEdgePosLong("ffBBUFbrrfBUFFbb", false, 0, 0, 1, 0, 2, 1, 0, 1, 2);
            recipes[59] = new BottomEdgePosLong("ffBBubFrrBfuBBff", true, 0, 0, 1, 0, 2, 1, 0, 1, 2);
            
            #endregion
            
            #region Fixing bottom edges orientation

            //original "vakrész"
            recipes[60] = new BottomEdgeOrientation("LuDffuuDDBdbuuddffdUlD", 0, 1, 0, 0, 2, 1);
            recipes[61] = new BottomEdgeOrientation("FuDrruuDDLdluuddrrdUfD", 0, 2, 1, 0, 1, 2);
            recipes[62] = new BottomEdgeOrientation("RuDbbuuDDFdfuuddbbdUrD", 0, 1, 2, 0, 0, 1);
            recipes[63] = new BottomEdgeOrientation("BuDlluuDDRdruuddlldUbD", 0, 0, 1, 0, 1, 0);

            // 2 distance "vakrész"
            recipes[64] = new BottomEdgeOrientation("LuDffuuDDBddbuuddffdUlDD", 0, 1, 0, 0, 1, 2);
            recipes[65] = new BottomEdgeOrientation("FuDrruuDDLddluuddrrdUfDD", 0, 0, 1, 0, 2, 1);

            #endregion

            recipes[66] = new RotateBottom();

            recipes[69] = new VerticalEdge("lFLrddlRFL", true, 0, 1, 0, 1, 2, 2, 0, 0, 1);
            recipes[70] = new VerticalEdge("RfrLddRlfr", false, 1, 2, 0, 0, 1, 2, 0, 0, 1);
            recipes[71] = new VerticalEdge("fRFbddfBRF", true, 0, 2, 1, 1, 0, 2, 0, 1, 0);
            recipes[72] = new VerticalEdge("BrFbddfBrb", false, 1, 2, 2, 0, 0, 1, 0, 1, 0);
            recipes[73] = new VerticalEdge("rBRlddrLBR", true, 0, 1, 2, 1, 0, 0, 0, 2, 1);
            recipes[74] = new VerticalEdge("LblRddLrbl", false, 1, 0, 2, 0, 1, 0, 0, 2, 1);
            recipes[75] = new VerticalEdge("bLBfddbFLB", true, 0, 0, 1, 1, 2, 0, 0, 1, 2);
            recipes[76] = new VerticalEdge("FlfBddFblf", false, 1, 0, 0, 0, 2, 1, 0, 1, 2);
        }
    }
}
