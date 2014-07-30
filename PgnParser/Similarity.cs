using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thursday;

namespace PgnParser
{
    public static class ExtensionMethods
    {
        public static double Squared(this double x)
        {
            return x * x;
        }
    }

    public class Similarity
    {
        public static double Calculate(Board b1, Board b2)
        {
            Stats s1 = new Stats(b1);
            Stats s2 = new Stats(b2);

            double statSimilarity = GetStatSimilarity(s1, s2);
            double boardSimilarity = 0;

            for (int i = 0; i < 63; i++)
            {
                if (b1.S[i].Colour == b2.S[i].Colour
                    && b1.S[i].PieceType == b2.S[i].PieceType)
                    boardSimilarity -= PieceValue(b1.S[i].PieceType);
            }

            //boardSimilarity = 0;

            return statSimilarity + boardSimilarity;
        }

        private static double PieceValue(PieceType pieceType)
        {
            switch (pieceType)
            {
                case PieceType.Blank:
                    return 0;
                case PieceType.Pawn:
                    return 1;
                case PieceType.Knight:
                    return 2;
                case PieceType.Bishop:
                    return 2;
                case PieceType.Rook:
                    return 4;
                case PieceType.Queen:
                    return 5;
                case PieceType.King:
                    return 6;
            }

            throw new ApplicationException();
        }

        // The lower the more similar
        private static double GetStatSimilarity(Stats s1, Stats s2)
        {
            double ret = 0f;
            ret += Math.Abs((double)(s1.TotalPieces - s2.TotalPieces));
            ret += Math.Abs((double)(s1.TotalPossibleMoves - s2.TotalPossibleMoves)) / 5f;

            ret += Math.Abs(s1.WPCount - s2.WPCount) * 1;
            ret += Math.Abs(s1.WBCount - s2.WBCount) * 3;
            ret += Math.Abs(s1.WNCount - s2.WNCount) * 3;
            ret += Math.Abs(s1.WRCount - s2.WRCount) * 5;
            ret += Math.Abs(s1.WQCount - s2.WQCount) * 8;

            ret += Math.Abs(s1.BPCount - s2.BPCount) * 1;
            ret += Math.Abs(s1.BBCount - s2.BBCount) * 3;
            ret += Math.Abs(s1.BNCount - s2.BNCount) * 3;
            ret += Math.Abs(s1.BRCount - s2.BRCount) * 5;
            ret += Math.Abs(s1.BQCount - s2.BQCount) * 8;

            ret += Math.Abs(s1.WPAdvancement - s2.WPAdvancement) * 0.002f;
            ret += Math.Abs(s1.BPAdvancement - s2.BPAdvancement) * 0.002f;

            ret += Math.Abs(s1.BBMobility - s2.BBMobility) * 0.02f;
            ret += Math.Abs(s1.WBMobility - s2.WBMobility) * 0.02f;

            return ret;
        }

    }

    public class Stats
    {
        public int TotalPieces;
        public int TotalPossibleMoves;

        public int WPCount;
        public int WNCount;
        public int WBCount;
        public int WRCount;
        public int WQCount;

        public int WPAdvancement;
        public int WBMobility;

        public int BPCount;
        public int BNCount;
        public int BBCount;
        public int BRCount;
        public int BQCount;

        public int BPAdvancement;
        public int BBMobility;

        public Stats(Board b)
        {
            TotalPossibleMoves = b.AllMoves.Count;

            for (int i = 0; i < 64; i++)
            {
                if(b.S[i].Colour == Colour.White)
                {
                    switch (b.S[i].PieceType)
                    {
                        case PieceType.Blank:
                            break;
                        case PieceType.Pawn:
                            WPCount++;
                            WPAdvancement += i * i;
                            break;
                        case PieceType.Knight:
                            WNCount++;
                            break;
                        case PieceType.Bishop:
                            WBCount++;
                            WBMobility += b.S[i].ValidMoves.Count;
                            break;
                        case PieceType.Rook:
                            WRCount++;
                            break;
                        case PieceType.Queen:
                            WQCount++;
                            break;
                    }
                }
                else if (b.S[i].Colour == Colour.Black)
                {
                    switch (b.S[i].PieceType)
                    {
                        case PieceType.Blank:
                            break;
                        case PieceType.Pawn:
                            BPCount++;
                            BPAdvancement += (7 - i) * (7 - i);
                            break;
                        case PieceType.Knight:
                            BNCount++;
                            break;
                        case PieceType.Bishop:
                            BBCount++;
                            BBMobility += b.S[i].ValidMoves.Count;
                            break;
                        case PieceType.Rook:
                            BRCount++;
                            break;
                        case PieceType.Queen:
                            BQCount++;
                            break;
                    }
                }
            }

            TotalPieces = WPCount + WNCount + WBCount + WRCount + WQCount + BPCount + BNCount + BBCount + BRCount + BQCount;
        }
    }
}
