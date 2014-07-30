using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thursday
{
    public struct Piece
    {
        public PieceType PieceType;
        public Colour Colour;
        public List<int> ValidMoves;

        public void AsShort()
        {
            ValidMoves = new List<int>(24);
        }

        public Piece(Colour colour, PieceType pieceType)
        {
            ValidMoves = new List<int>(24);
            Colour = colour;
            PieceType = pieceType;
        }

        public Piece(Piece piece)
        {
            ValidMoves = new List<int>(24);
            this.Colour = piece.Colour;
            this.PieceType = piece.PieceType;
        }
    }
}
