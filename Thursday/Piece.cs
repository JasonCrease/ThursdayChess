using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thursday
{
    public class Piece
    {
        public PieceType PieceType;
        public Colour Colour;
        public int[] Moves;
        public double[] Powers;
        public int MoveCnt;

        public Piece(Colour colour, PieceType pieceType)
        {
            MoveCnt = 0;
            int maxMoves = 0;

            if (pieceType == PieceType.Pawn) maxMoves = 5;
            else if (pieceType == PieceType.Bishop) maxMoves = 13;
            else if (pieceType == PieceType.Rook) maxMoves = 14;
            else if (pieceType == PieceType.Knight) maxMoves = 8;
            else if (pieceType == PieceType.King) maxMoves = 10;
            else if (pieceType == PieceType.Queen) maxMoves = 27;

            Moves = new int[maxMoves];
            Powers = new double[maxMoves];

            Colour = colour;
            PieceType = pieceType;
        }

        public Piece(Piece piece) : this(piece.Colour, piece.PieceType)
        {
        }

        public void AddMove(int i, double power)
        {
            Moves[MoveCnt] = i;
            MoveCnt++;
        }
    }
}
