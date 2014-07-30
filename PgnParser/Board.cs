using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PgnParser
{
    public class StoreBoard : Thursday.Board
    {
        // 0 is blank
        //
        // white pieces:
        // 1 pawn
        // 2 knight
        // 3 bishop
        // 4 rook
        // 5 queen
        // 6 king
        //
        // black pieces:
        //  9 pawn
        // 10 knight
        // 11 bishop
        // 12 rook
        // 13 queen
        // 14 king

        public byte[,] S;

        public StoreBoard()
        {
            S = new byte[8, 8];

            S[0, 0] = 4;
            S[0, 1] = 3;
            S[0, 2] = 2;
            S[0, 3] = 5;
            S[0, 4] = 6;
            S[0, 5] = 2;
            S[0, 6] = 3;
            S[0, 7] = 4;

            S[7, 0] = 12;
            S[7, 1] = 11;
            S[7, 2] = 10;
            S[7, 3] = 13;
            S[7, 4] = 14;
            S[7, 5] = 10;
            S[7, 6] = 11;
            S[7, 7] = 12;
        }
    }
}
