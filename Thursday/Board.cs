﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thursday
{
    public class Board
    {
        public Piece[] S = new Piece[64];

        internal bool BlackIsInCheck;
        internal bool WhiteIsInCheck;
        internal bool BlackCanQCastle = true;
        internal bool BlackCanKCastle = true;
        internal bool WhiteCanQCastle = true;
        internal bool WhiteCanKCastle = true;

        internal int EpSquare = -1;

        public Colour WhosMove;

        public void SetupStandardBoard()
        {
            this.S[0] = new Thursday.Piece(Colour.White, PieceType.Rook);
            this.S[1] = new Thursday.Piece(Colour.White, PieceType.Knight);
            this.S[2] = new Thursday.Piece(Colour.White, PieceType.Bishop);
            this.S[3] = new Thursday.Piece(Colour.White, PieceType.King);
            this.S[4] = new Thursday.Piece(Colour.White, PieceType.Queen);
            this.S[5] = new Thursday.Piece(Colour.White, PieceType.Bishop);
            this.S[6] = new Thursday.Piece(Colour.White, PieceType.Knight);
            this.S[7] = new Thursday.Piece(Colour.White, PieceType.Rook);

            this.S[56] = new Thursday.Piece(Colour.Black, PieceType.Rook);
            this.S[57] = new Thursday.Piece(Colour.Black, PieceType.Knight);
            this.S[58] = new Thursday.Piece(Colour.Black, PieceType.Bishop);
            this.S[59] = new Thursday.Piece(Colour.Black, PieceType.King);
            this.S[60] = new Thursday.Piece(Colour.Black, PieceType.Queen);
            this.S[61] = new Thursday.Piece(Colour.Black, PieceType.Bishop);
            this.S[62] = new Thursday.Piece(Colour.Black, PieceType.Knight);
            this.S[63] = new Thursday.Piece(Colour.Black, PieceType.Rook);

            for (int i = 8; i < 16; i++)
                this.S[i] = new Thursday.Piece(Colour.White, PieceType.Pawn);
            for (int i = 48; i < 56; i++)
                this.S[i] = new Thursday.Piece(Colour.Black, PieceType.Pawn);

            this.WhosMove = Colour.White;
        }

        public bool MoverIsInCheck()
        {
            if (AllMoves.Any(x => S[x.To].PieceType == PieceType.King))
                return true;
            else
                return false;
        }

        public Board(Board oldBoard, int oldPos, int destPos)
        {
            this.WhosMove = oldBoard.WhosMove == Colour.Black ? Colour.White : Colour.Black;
            this.BlackCanQCastle = oldBoard.BlackCanQCastle;
            this.BlackCanKCastle = oldBoard.BlackCanKCastle;
            this.WhiteCanQCastle = oldBoard.WhiteCanQCastle;
            this.WhiteCanKCastle = oldBoard.WhiteCanKCastle;

            for (int i = 0; i < 64; i++)
                S[i] = new Piece(oldBoard.S[i]);
            
            PieceType pieceType = S[oldPos].PieceType;
            
            // Handle queening and castle prevention
            if (WhosMove == Colour.White)
            {
                if (pieceType == PieceType.Pawn)
                {
                    if (Rank(destPos) == 0)
                        S[oldPos] = new Piece(Colour.Black, PieceType.Queen);
                    if (destPos == oldPos - 16)   // set en-passant square
                        EpSquare = destPos + 8;
                }
                else if (pieceType == PieceType.King)
                {
                    BlackCanQCastle = false;
                    BlackCanKCastle = false;
                    if (destPos == oldPos + 2)   //Q-side castle move rook
                    {
                        S[63] = new Piece(Colour.Black, PieceType.Blank);
                        S[60] = new Piece(Colour.Black, PieceType.Rook);
                    }
                    if (destPos == oldPos - 2)   //K-side castle move rook
                    {
                        S[56] = new Piece(Colour.Black, PieceType.Blank);
                        S[58] = new Piece(Colour.Black, PieceType.Rook);
                    }
                }
                else if (pieceType == PieceType.Rook && oldPos == 56)
                    BlackCanKCastle = false;
                else if (pieceType == PieceType.Rook && oldPos == 63)
                    BlackCanQCastle = false;
            }
            if (WhosMove == Colour.Black)
            {
                if (pieceType == PieceType.Pawn)
                {
                    if (Rank(destPos) == 7)
                        S[oldPos] = new Piece(Colour.White, PieceType.Queen);
                    if (destPos == oldPos + 16)  // set en-passant square
                        EpSquare = destPos - 8;
                }
                if (pieceType == PieceType.King)
                {
                    WhiteCanQCastle = false;
                    WhiteCanKCastle = false;
                    if (destPos == oldPos + 2)   //Q-side castle move rook
                    {
                        S[7] = new Piece(Colour.White, PieceType.Blank);
                        S[4] = new Piece(Colour.White, PieceType.Rook);
                    }
                    if (destPos == oldPos - 2)   //K-side castle move rook
                    {
                        S[0] = new Piece(Colour.White, PieceType.Blank);
                        S[2] = new Piece(Colour.White, PieceType.Rook);
                    }
                }
                if (pieceType == PieceType.Rook && oldPos == 0)
                    WhiteCanKCastle = false;
                if (pieceType == PieceType.Rook && oldPos == 7)
                    WhiteCanQCastle = false;
            }

            if (destPos == oldBoard.EpSquare && pieceType == PieceType.Pawn)
            {
                if (WhosMove == Colour.Black)
                    S[oldBoard.EpSquare - 8] = new Piece(Colour.Blank, PieceType.Blank);
                if (WhosMove == Colour.White)
                    S[oldBoard.EpSquare + 8] = new Piece(Colour.Blank, PieceType.Blank);
            }
            S[destPos] = new Piece(S[oldPos]);
            S[oldPos] = new Piece(Colour.Blank, PieceType.Blank);
        }

        public Board()
        {
        }

        private List<Move> m_AllPossibleMoves = null;

        public List<Move> AllMoves
        {
            get{
                if (m_AllPossibleMoves == null)
                {
                    m_AllPossibleMoves = new List<Move>();
                    EnumerateAllMoves();
                    //m_AllPossibleMoves = m_AllPossibleMoves.OrderByDescending(m => (new Board(this, m.From, m.To)).ScoreBoard()).ToList();
                }

                return m_AllPossibleMoves;
            }
        }

        private void EnumerateAllMoves()
        {
            for (int i = 0; i < 64; i++)
            {
                if (S[i].PieceType != PieceType.Blank && S[i].Colour == WhosMove)
                {
                    switch (S[i].PieceType)
                    {
                        case PieceType.Pawn:
                            EnumeratePawnMoves(i);
                            break;
                        case PieceType.Knight:
                            EnumerateKnightMoves(i);
                            break;
                        case PieceType.King:
                            EnumerateKingMoves(i);
                            break;
                        case PieceType.Rook:
                            EnumerateRookMoves(i);
                            break;
                        case PieceType.Bishop:
                            EnumerateBishopMoves(i);
                            break;
                        case PieceType.Queen:
                            EnumerateQueenMoves(i);
                            break;
                        default:
                            throw new ApplicationException();
                    }

                    foreach (int j in S[i].ValidMoves)
                        m_AllPossibleMoves.Add(new Move(i, j));
                }
            }
        }

        private void EnumerateQueenMoves(int i)
        {
            EnumerateBishopMoves(i);
            EnumerateRookMoves(i);
        }

        private void EnumeratePawnMoves(int i)
        {
            List<int> validMoves = S[i].ValidMoves;

            if (WhosMove == Colour.White)
            {
                // Two-move start
                if (Rank(i) == 1 && IsEmpty(i + 8) && IsEmpty(i + 16))
                    validMoves.Add(i + 16);
                // One-moves
                if (IsEmpty(i + 8))
                    validMoves.Add(i + 8);
                // Take SW
                if (ExistsAtOffset(i, 1, -1) && S[i + 7].PieceType != PieceType.Blank && S[i + 7].Colour == Colour.Black)
                    validMoves.Add(i + 7);
                // Take SE
                if (ExistsAtOffset(i, 1, 1) && S[i + 9].PieceType != PieceType.Blank && S[i + 9].Colour == Colour.Black)
                    validMoves.Add(i + 9);
                if (EpSquare == i + 7)
                    validMoves.Add(i + 7);
                if (EpSquare == i + 9)
                    validMoves.Add(i + 9);
            }
            else
            {
                // Two-move start
                if (Rank(i) == 6 && S[i - 8].PieceType == PieceType.Blank && S[i - 16].PieceType == PieceType.Blank)
                    validMoves.Add(i - 16);
                // One-moves
                if (S[i - 8].PieceType == PieceType.Blank)
                    validMoves.Add(i - 8);
                // Take NE
                if (ExistsAtOffset(i, -1, 1) && S[i - 7].PieceType != PieceType.Blank && S[i - 7].Colour == Colour.White)
                    validMoves.Add(i - 7);
                // Take NW
                if (ExistsAtOffset(i, -1, -1) && S[i - 9].PieceType != PieceType.Blank && S[i - 9].Colour == Colour.White)
                    validMoves.Add(i - 9);
                if (EpSquare == i - 7)
                    validMoves.Add(i - 7);
                if (EpSquare == i - 9)
                    validMoves.Add(i - 9);
            }
        }

        static int[] knightXoffs = new int[] { -2, -1, 1, 2, 2, 1, -1, -2 };
        static int[] knightYoffs = new int[] { 1, 2, 2, 1, -1, -2, -2, -1 };

        private void EnumerateKnightMoves(int i)
        {
            List<int> validMoves = S[i].ValidMoves;

            for (int c = 0; c < 8; c++)
            {
                int xOff = knightXoffs[c];
                int yOff = knightYoffs[c];

                int j = i + (xOff * 8) + yOff;
                if (ExistsAtOffset(i, xOff, yOff))
                    if (IsEmptyAtOffset(i, xOff, yOff))
                        validMoves.Add(j);
                    else
                    {
                        if (S[i].Colour != S[j].Colour)
                            validMoves.Add(j);
                    }
            }
        }

        private void EnumerateBishopMoves(int i)
        {
            Colour col = S[i].Colour;
            List<int> validMoves = S[i].ValidMoves;

            // Slide piece NW until it hits something
            for (int offset = 1; ExistsAtOffset(i, -offset, -offset); offset++)
            {
                int j = i - (offset * 8) - offset;
                if (IsEmpty(j))
                    validMoves.Add(j);
                else
                {
                    if (S[j].Colour != col)  // If piece slide end is another colour, consider taking it
                        validMoves.Add(j);
                    break;
                }
            }

            // Slide piece NE until it hits something
            for (int offset = 1; ExistsAtOffset(i, -offset, offset); offset++)
            {
                int j = i - (offset * 8) + offset;
                if (IsEmpty(j))
                    validMoves.Add(j);
                else
                {
                    if (S[j].Colour != col)  // If piece slide end is another colour, consider taking it
                        validMoves.Add(j);
                    break;
                }
            }

            // Slide piece SW until it hits something
            for (int offset = 1; ExistsAtOffset(i, offset, -offset); offset++)
            {
                int j = i + (offset * 8) - offset;
                if (IsEmpty(j))
                    validMoves.Add(j);
                else
                {
                    if (S[j].Colour != col)  // If piece slide end is another colour, consider taking it
                        validMoves.Add(j);
                    break;
                }
            }


            // Slide piece SE until it hits something
            for (int offset = 1; ExistsAtOffset(i, offset, offset); offset++)
            {
                int j = i + (offset * 8) + offset;
                if (IsEmpty(j))
                    validMoves.Add(j);
                else
                {
                    if (S[j].Colour != col)  // If piece slide end is another colour, consider taking it
                        validMoves.Add(j);
                    break;
                }
            }
        }

        private void EnumerateRookMoves(int i)
        {
            List<int> validMoves = S[i].ValidMoves;
            Colour col = S[i].Colour;
            int j = 0;

            // Slide piece north until it hits something
            for (j = i - 8; j >= 0; j -= 8)
            {
                if (S[j].PieceType == PieceType.Blank)
                    validMoves.Add(j);
                else
                {
                    if (j != i && S[j].Colour != col)  // If piece slide end is another colour, consider taking it
                        validMoves.Add(j);
                    break;
                }
            }

            // Slide piece south until it hits something
            for (j = i + 8; j < 64; j += 8)
            {
                if (S[j].PieceType == PieceType.Blank)
                    validMoves.Add(j);
                else
                {
                    if (j != i && S[j].Colour != col)  // If piece slide end is another colour, consider taking it
                        validMoves.Add(j);
                    break;
                }
            }

            // Slide piece east until it hits something
            for (int offset = 1; ExistsAtOffset(i, 0, offset); offset++)
            {
                if (IsEmpty(i + offset))
                    validMoves.Add(i + offset);
                else
                {
                    if (S[i + offset].Colour != col)  // If piece slide end is another colour, consider taking it
                        validMoves.Add(i + offset);
                    break;
                }
            }

            // Slide piece west until it hits something
            for (int offset = -1; ExistsAtOffset(i, 0, offset); offset--)
            {
                if (IsEmpty(i + offset))
                    validMoves.Add(i + offset);
                else
                {
                    if (S[i + offset].Colour != col)  // If piece slide end is another colour, consider taking it
                        validMoves.Add(i + offset);
                    break;
                }
            }
        }

        private void EnumerateKingMoves(int i)
        {
            List<int> validMoves = S[i].ValidMoves;

            if (ExistsAtOffset(i, -1, -1) && IsEmptyOrEnemy(i - 9))
                validMoves.Add((short)(i - 9));
            if (ExistsAtOffset(i, -1, 0) && IsEmptyOrEnemy(i - 8))
                validMoves.Add((short)(i - 8));
            if (ExistsAtOffset(i, -1, 1) && IsEmptyOrEnemy(i - 7))
                validMoves.Add((short)(i - 7));

            if (ExistsAtOffset(i, 0, -1) && IsEmptyOrEnemy(i - 1))
                validMoves.Add((short)(i - 1));
            if (ExistsAtOffset(i, 0, 1) && IsEmptyOrEnemy(i + 1))
                validMoves.Add((short)(i + 1));

            if (ExistsAtOffset(i, 1, -1) && IsEmptyOrEnemy(i + 7))
                validMoves.Add((short)(i + 7));
            if (ExistsAtOffset(i, 1, 0) && IsEmptyOrEnemy(i + 8))
                validMoves.Add((short)(i + 8));
            if (ExistsAtOffset(i, 1, 1) && IsEmptyOrEnemy(i + 9))
                validMoves.Add((short)(i + 9));

            if (WhosMove == Colour.White && WhiteCanKCastle)
                if (IsEmpty(1) && IsEmpty(2) && 
                    S[0].PieceType == PieceType.Rook && S[0].Colour == Colour.White)
                    validMoves.Add(1);
            if (WhosMove == Colour.White && WhiteCanQCastle)
                if (IsEmpty(4) && IsEmpty(5) && IsEmpty(6) && 
                    S[7].PieceType == PieceType.Rook && S[7].Colour == Colour.White)
                    validMoves.Add(5);
            if (WhosMove == Colour.Black && BlackCanKCastle)
                if (IsEmpty(57) && IsEmpty(58) && 
                    S[56].PieceType == PieceType.Rook && S[56].Colour == Colour.Black)
                    validMoves.Add(57);
            if (WhosMove == Colour.Black && BlackCanQCastle)
                if (IsEmpty(60) && IsEmpty(61) && IsEmpty(62) && 
                    S[63].PieceType == PieceType.Rook && S[63].Colour == Colour.Black)
                    validMoves.Add(61);
        }

        private bool IsEmpty(int i)
        {
            return S[i].PieceType == PieceType.Blank;
        }

        private bool IsEmptyOrEnemy(int i)
        {
            return S[i].PieceType == PieceType.Blank || S[i].Colour != WhosMove;
        }

        private bool IsEmpty(int y, int x)
        {
            return S[(y * 8) + x].PieceType == PieceType.Blank;
        }

        private bool IsEmptyAtOffset(int i, int y, int x)
        {
            return S[i + (y * 8) + x].PieceType == PieceType.Blank;
        }

        private bool ExistsAtOffset(int startSquare, int dirY, int dirX)
        {
            return Mailbox.X1012[Mailbox.X88[startSquare] + (dirY * 10) + dirX] >= 0;
        }


        private int Rank(int i)
        {
            return ((int)(i / 8));
        }

        private int File(int i)
        {
            return i % 8;
        }

        public Board MakeMove(int oldPos, int destPos)
        {
            return new Board(this, oldPos, destPos);
        }

        public double ScoreBoard()
        {
            double score = 0;

            for (int i = 0; i < 64; i++)
            {
                Piece p = S[i];

                if (p.PieceType == PieceType.Blank)
                    goto skip;

                double pVal = 0;

                pVal += PieceValue(p.PieceType);
                pVal -= Math.Abs((double)File(i) - 3.5f) / 70f;  // favour central pieces

                // Favour advanced pieces
                if (S[i].Colour == Colour.White)
                {
                    pVal += (double)(Rank(i)) / 20f;
                    score += pVal;
                }
                else
                {
                    pVal += (double)(7 - Rank(i)) / 20f;
                    score -= pVal;
                }


                skip: ;
            }

            //score += CalculateKingSafety();
            score += (this.AllMoves.Count() / 200f * (WhosMove == Colour.White ? 1f : -1f));

            return score;
        }


        private double PieceValue(PieceType pieceType)
        {
            if (pieceType == PieceType.Blank)
                return 0;
            if (pieceType == PieceType.Pawn)
                return 1;
            if (pieceType == PieceType.Knight)
                return 3.1;
            if (pieceType == PieceType.Bishop)
                return 3.25;
            if (pieceType == PieceType.Rook)
                return 5;
            if (pieceType == PieceType.Queen)
                return 9;
            if (pieceType == PieceType.King)
                return 1000;

            throw new ApplicationException();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(64);

            for (int i = 63; i >= 0; i--)
            {
                if (S[i].PieceType == PieceType.Blank)
                {
                    if (i % 2 == ((int)i / 8) % 2) sb.Append(" ");
                    else sb.Append(" ");
                }
                else
                    switch (S[i].PieceType)
                    {
                        case PieceType.Pawn:
                            sb.Append((char)(80 + (int)S[i].Colour * 32));
                            break;
                        case PieceType.Knight:
                            sb.Append((char)(78 + (int)S[i].Colour * 32));
                            break;
                        case PieceType.Bishop:
                            sb.Append((char)(66 + (int)S[i].Colour * 32));
                            break;
                        case PieceType.Rook:
                            sb.Append((char)(82 + (int)S[i].Colour * 32));
                            break;
                        case PieceType.Queen:
                            sb.Append((char)(81 + (int)S[i].Colour * 32));
                            break;
                        case PieceType.King:
                            sb.Append((char)(75 + (int)S[i].Colour * 32));
                            break;
                        default:
                            throw new ApplicationException();
                            break;
                    }

                if (i % 8 == 0) sb.AppendLine();
            }

            return sb.ToString();
        }

        internal bool MoveIsLegal(int fromPos, int toPos)
        {
            var dummy = AllMoves;
            if (!S[fromPos].ValidMoves.Contains(toPos)) return false;
            if (S[fromPos].Colour != WhosMove) return false;

            return true;
        }
    }
}
