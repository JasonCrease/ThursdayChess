using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thursday
{
    public class BoardBuilder
    {
        public static Board BuildBoard(string boardStr, bool isWhitesMove = true,
            bool whiteCanQCastle = true, bool whiteCanKCastle = true, bool blackCanQCastle = true, bool blackCanKCastle = true)
        {
            Board b = new Board();
            b.WhosMove = isWhitesMove ? Colour.White : Colour.Black;
            b.WhiteCanQCastle = whiteCanQCastle;
            b.WhiteCanKCastle = whiteCanKCastle;
            b.BlackCanQCastle = blackCanQCastle;
            b.BlackCanKCastle = blackCanKCastle;

            string s = "";
            for (int i = 0; i < boardStr.Length; i++)
                if (!char.IsWhiteSpace(boardStr[i])) s += boardStr[i];

            if (s.Length != 64) throw new ApplicationException("Board of wrong length");

            for (int i = 0; i < 64; i++)
            {
                char c = s[i];
                char cl = Char.ToLower(s[i]);

                if (c == '.')
                    b.S[i] = null;
                else
                {
                    Colour colour = Colour.Blank;
                    PieceType pieceType = PieceType.Blank;

                    if (char.IsUpper(c))
                        colour = Colour.White;

                    if (char.IsLower(c))
                        colour = Colour.Black;

                    if (cl == 'p')
                        pieceType = PieceType.Pawn;
                    if (cl == 'b')
                        pieceType = PieceType.Bishop;
                    if (cl == 'r')
                        pieceType = PieceType.Rook;
                    if (cl == 'k')
                        pieceType = PieceType.King;
                    if (cl == 'q')
                        pieceType = PieceType.Queen;
                    if (cl == 'n')
                        pieceType = PieceType.Knight;

                    if (colour == Colour.Blank)
                        throw new ApplicationException();
                    if (pieceType == PieceType.Blank)
                        throw new ApplicationException();

                    b.S[i] = new Piece(colour, pieceType);
                }
            }

            return b;
        }
    }
}
