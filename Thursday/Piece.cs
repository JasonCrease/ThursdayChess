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
        public int[] ValidMoves;
        public int MoveCount;

        public Piece(Colour colour, PieceType pieceType)
        {
            MoveCount = 0;
            int maxMoves = 5;

            if (pieceType == PieceType.Bishop) maxMoves = 14;
            else if (pieceType == PieceType.Rook) maxMoves = 14;
            else if (pieceType == PieceType.Knight) maxMoves = 8;
            else if (pieceType == PieceType.King) maxMoves = 10;
            else if (pieceType == PieceType.Queen) maxMoves = 28;

            ValidMoves = new int[maxMoves];

            Colour = colour;
            PieceType = pieceType;
        }

        public Piece(Piece piece) : this(piece.Colour, piece.PieceType)
        { 
        }

        public void AddMove(int i)
        {
            ValidMoves[MoveCount] = i;
            MoveCount++;
        }
    }
}
