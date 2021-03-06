﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thursday
{
    public class Board
    {
        internal static int[] X1012 = new int[120] {
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1,  0,  1,  2,  3,  4,  5,  6,  7, -1,
            -1,  8,  9, 10, 11, 12, 13, 14, 15, -1,
            -1, 16, 17, 18, 19, 20, 21, 22, 23, -1,
            -1, 24, 25, 26, 27, 28, 29, 30, 31, -1,
            -1, 32, 33, 34, 35, 36, 37, 38, 39, -1,
            -1, 40, 41, 42, 43, 44, 45, 46, 47, -1,
            -1, 48, 49, 50, 51, 52, 53, 54, 55, -1,
            -1, 56, 57, 58, 59, 60, 61, 62, 63, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
        };

        internal static int[] X88 = new int[64]{
            21, 22, 23, 24, 25, 26, 27, 28,
            31, 32, 33, 34, 35, 36, 37, 38,
            41, 42, 43, 44, 45, 46, 47, 48,
            51, 52, 53, 54, 55, 56, 57, 58,
            61, 62, 63, 64, 65, 66, 67, 68,
            71, 72, 73, 74, 75, 76, 77, 78,
            81, 82, 83, 84, 85, 86, 87, 88,
            91, 92, 93, 94, 95, 96, 97, 98
        };

        internal static int[] poss = new int[64] {
            0,  1,  2,  3,  4,  5,  6,  7,
            8,  9, 10, 11, 12, 13, 14, 15, 
            16, 17, 18, 19, 20, 21, 22, 23, 
            24, 25, 26, 27, 28, 29, 30, 31, 
            32, 33, 34, 35, 36, 37, 38, 39, 
            40, 41, 42, 43, 44, 45, 46, 47,
            48, 49, 50, 51, 52, 53, 54, 55,
            56, 57, 58, 59, 60, 61, 62, 63
        };


        internal static int[] rank = {
            0,0,0,0,0,0,0,0,
            1,1,1,1,1,1,1,1,
            2,2,2,2,2,2,2,2,
            3,3,3,3,3,3,3,3,
            4,4,4,4,4,4,4,4,
            5,5,5,5,5,5,5,5,
            6,6,6,6,6,6,6,6,
            7,7,7,7,7,7,7,7,
        };

        public Piece[] S = new Piece[64];

        internal bool BlackCanQCastle = true;
        internal bool BlackCanKCastle = true;
        internal bool WhiteCanQCastle = true;
        internal bool WhiteCanKCastle = true;

        internal int EpSquare = -99;

        public Colour WhosMove;

        private Board()
        {
            this.WhosMove = Colour.White;
        }

        public static Board InitialStandardBoard
        {
            get{
                Board b = new Board();
                b.S[0] = new Piece(Colour.White, PieceType.Rook);
                b.S[1] = new Piece(Colour.White, PieceType.Knight);
                b.S[2] = new Piece(Colour.White, PieceType.Bishop);
                b.S[3] = new Piece(Colour.White, PieceType.King);
                b.S[4] = new Piece(Colour.White, PieceType.Queen);
                b.S[5] = new Piece(Colour.White, PieceType.Bishop);
                b.S[6] = new Piece(Colour.White, PieceType.Knight);
                b.S[7] = new Piece(Colour.White, PieceType.Rook);

                b.S[56] = new Piece(Colour.Black, PieceType.Rook);
                b.S[57] = new Piece(Colour.Black, PieceType.Knight);
                b.S[58] = new Piece(Colour.Black, PieceType.Bishop);
                b.S[59] = new Piece(Colour.Black, PieceType.King);
                b.S[60] = new Piece(Colour.Black, PieceType.Queen);
                b.S[61] = new Piece(Colour.Black, PieceType.Bishop);
                b.S[62] = new Piece(Colour.Black, PieceType.Knight);
                b.S[63] = new Piece(Colour.Black, PieceType.Rook);

                for (int i = 8; i < 16; i++)
                    b.S[i] = new Piece(Colour.White, PieceType.Pawn);
                for (int i = 48; i < 56; i++)
                    b.S[i] = new Piece(Colour.Black, PieceType.Pawn);
                for (int i = 16; i < 48; i++)
                    b.S[i] = null;

                return b;
            }
        }

        public bool YouCanTakeOpponentsKing()
        {
            for (int i = 0; i < AllMovesCount; i++ )
            { 
                Move m = AllMoves[i];
                if (S[m.To] != null && S[m.To].PieceType == PieceType.King && S[m.To].Colour != WhosMove)
                    return true;
            }

            return false;
        }

        public Board(Board currentBoard)
        {
            this.BlackCanQCastle = currentBoard.BlackCanQCastle;
            this.BlackCanKCastle = currentBoard.BlackCanKCastle;
            this.WhiteCanQCastle = currentBoard.WhiteCanQCastle;
            this.WhiteCanKCastle = currentBoard.WhiteCanKCastle;
            this.WhosMove = currentBoard.WhosMove;

            for (int i = 0; i < 64; i++)
            {
                if (currentBoard.S[i] != null)
                    S[i] = new Piece(currentBoard.S[i]);
            }
            m_OldBoard = currentBoard;
        }

        // Return false if move is illegal
        public bool MakeMove(int oldPos, int destPos)
        {
            PieceType pieceType = S[oldPos].PieceType;
            
            // Handle queening and castle prevention
            if (WhosMove == Colour.Black)
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
                        S[63] = null;
                        S[60] = new Piece(Colour.Black, PieceType.Rook);
                    }
                    if (destPos == oldPos - 2)   //K-side castle move rook
                    {
                        S[56] = null;
                        S[58] = new Piece(Colour.Black, PieceType.Rook);
                    }
                }
                else if (pieceType == PieceType.Rook && oldPos == 56)
                    BlackCanKCastle = false;
                else if (pieceType == PieceType.Rook && oldPos == 63)
                    BlackCanQCastle = false;
            }
            if (WhosMove == Colour.White)
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
                        S[7] = null; 
                        S[4] = new Piece(Colour.White, PieceType.Rook);
                    }
                    if (destPos == oldPos - 2)   //K-side castle move rook
                    {
                        S[0] = null; 
                        S[2] = new Piece(Colour.White, PieceType.Rook);
                    }
                }
                if (pieceType == PieceType.Rook && oldPos == 0)
                    WhiteCanKCastle = false;
                if (pieceType == PieceType.Rook && oldPos == 7)
                    WhiteCanQCastle = false;
            }

            if (destPos == m_OldBoard.EpSquare && pieceType == PieceType.Pawn)
            {
                if (WhosMove == Colour.White)
                    S[m_OldBoard.EpSquare - 8] = null;
                if (WhosMove == Colour.Black)
                    S[m_OldBoard.EpSquare + 8] = null;
            }
            S[destPos] = new Piece(S[oldPos]);
            S[oldPos] = null;

            if (KingIsInCheck) return false;

            this.WhosMove = m_OldBoard.WhosMove == Colour.White ? Colour.Black : Colour.White;

            return true;
        }

        private Move[] m_AllPossibleMoves = null;
        private int m_AllMovesCount = -1;
        public int AllMovesCount { 
            get {
                if (m_AllMovesCount == -1)
                {
                    m_AllMovesCount = 0;
                    m_AllPossibleMoves = new Move[150];
                    EnumerateAllMoves();
                }

                return m_AllMovesCount; 
            }
        }

        public Move[] AllMoves
        {
            get{
                if (m_AllPossibleMoves == null)
                {
                    m_AllMovesCount = 0;
                    m_AllPossibleMoves = new Move[150];
                    EnumerateAllMoves();
                    //m_AllPossibleMoves = m_AllPossibleMoves.OrderByDescending(m => (new Board(this, m.From, m.To)).ScoreBoard()).ToList();
                }

                return m_AllPossibleMoves;
            }
        }

        private void EnumerateAllMoves()
        {
            m_AllMovesCount = 0;

            for (int i = 0; i < 64; i++)
            {
                if (S[i] != null && S[i].Colour == WhosMove)
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

                    for (int j = 0; j < S[i].MoveCnt; j++)
                    {
                        m_AllPossibleMoves[m_AllMovesCount] = new Move(i, S[i].Moves[j], S[i].Powers[j]);
                        m_AllMovesCount++;
                    }
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
            Piece p = S[i];

            if (WhosMove == Colour.White)
            {
                // Two-move start
                if (Rank(i) == 1 && IsEmpty(i + 8) && IsEmpty(i + 16))
                    p.AddMove(i + 16, Power.PawnMove);
                // One-moves
                if (IsEmpty(i + 8))
                    p.AddMove(i + 8, Power.PawnMove);
                // Take SW
                if (ExistsAtOffset(i, 1, -1) && S[i + 7] != null && S[i + 7].Colour == Colour.Black)
                    p.AddMove(i + 7, Power.PawnCapture);
                // Take SE
                if (ExistsAtOffset(i, 1, 1) && S[i + 9] != null && S[i + 9].Colour == Colour.Black)
                    p.AddMove(i + 9, Power.PawnCapture);
                if (EpSquare == i + 7 && i % 8 != 0)
                    p.AddMove(i + 7, Power.PawnCapture);
                if (EpSquare == i + 9 && i % 8 != 7)
                    p.AddMove(i + 9, Power.PawnCapture);
            }
            else
            {
                // Two-move start
                if (Rank(i) == 6 && IsEmpty(i - 8) == null && IsEmpty(i - 16))
                    p.AddMove(i - 16, Power.PawnMove);
                // One-moves
                if (IsEmpty(i - 8))
                    p.AddMove(i - 8, Power.PawnMove);
                // Take NE
                if (ExistsAtOffset(i, -1, 1) && S[i - 7] != null && S[i - 7].Colour == Colour.White)
                    p.AddMove(i - 7, Power.PawnCapture);
                // Take NW
                if (ExistsAtOffset(i, -1, -1) && S[i - 9] != null && S[i - 9].Colour == Colour.White)
                    p.AddMove(i - 9, Power.PawnCapture);
                if (EpSquare == i - 7 && i % 8 != 0)
                    p.AddMove(i - 7, Power.PawnCapture);
                if (EpSquare == i - 9 && i % 8 != 7)
                    p.AddMove(i - 9, Power.PawnCapture);
            }
        }

        static int[] knightXoffs = new int[] { -2, -1, 1, 2, 2, 1, -1, -2 };
        static int[] knightYoffs = new int[] { 1, 2, 2, 1, -1, -2, -2, -1 };
        private  Board m_OldBoard;

        private void EnumerateKnightMoves(int i)
        {
            Piece p = S[i];

            for (int c = 0; c < 8; c++)
            {
                int xOff = knightXoffs[c];
                int yOff = knightYoffs[c];

                int j = i + (xOff * 8) + yOff;
                if (ExistsAtOffset(i, xOff, yOff))
                    if (IsEmptyAtOffset(i, xOff, yOff))
                        p.AddMove(j, Power.KnightMove);   // knight move
                    else
                    {
                        if (S[i].Colour != S[j].Colour)
                            p.AddMove(j, Power.KnightCapture);   // knight capture
                    }
            }
        }

        private void EnumerateBishopMoves(int i)
        {
            Colour col = S[i].Colour;
            Piece p = S[i];

            // Slide piece NW until it hits something
            for (int offset = 1; ExistsAtOffset(i, -offset, -offset); offset++)
            {
                int j = i - (offset * 8) - offset;
                if (IsEmpty(j))
                    p.AddMove(j, Power.BishopMove);   // bishop move
                else
                {
                    if (S[j].Colour != col)  // If piece slide end is another colour, consider taking it
                        p.AddMove(j, Power.BishopCapture);   // bishop capture
                    break;
                }
            }

            // Slide piece NE until it hits something
            for (int offset = 1; ExistsAtOffset(i, -offset, offset); offset++)
            {
                int j = i - (offset * 8) + offset;
                if (IsEmpty(j))
                    p.AddMove(j, Power.BishopMove);   // bishop move
                else
                {
                    if (S[j].Colour != col)  // If piece slide end is another colour, consider taking it
                        p.AddMove(j, Power.BishopCapture);   // bishop capture
                    break;
                }
            }

            // Slide piece SW until it hits something
            for (int offset = 1; ExistsAtOffset(i, offset, -offset); offset++)
            {
                int j = i + (offset * 8) - offset;
                if (IsEmpty(j))
                    p.AddMove(j, Power.BishopMove);
                else
                {
                    if (S[j].Colour != col)  // If piece slide end is another colour, consider taking it
                        p.AddMove(j, Power.BishopCapture);
                    break;
                }
            }


            // Slide piece SE until it hits something
            for (int offset = 1; ExistsAtOffset(i, offset, offset); offset++)
            {
                int j = i + (offset * 8) + offset;
                if (IsEmpty(j))
                    p.AddMove(j, Power.BishopMove);
                else
                {
                    if (S[j].Colour != col)  // If piece slide end is another colour, consider taking it
                        p.AddMove(j, Power.BishopCapture);
                    break;
                }
            }
        }

        private void EnumerateRookMoves(int i)
        {
            Piece p = S[i];
            Colour col = S[i].Colour;
            int j = 0;

            // Slide piece north until it hits something
            for (j = i - 8; j >= 0; j -= 8)
            {
                if (S[j] == null)
                    p.AddMove(j, Power.RookMove);
                else
                {
                    if (j != i && S[j].Colour != col)  // If piece slide end is another colour, consider taking it
                        p.AddMove(j, Power.RookCapture);
                    break;
                }
            }

            // Slide piece south until it hits something
            for (j = i + 8; j < 64; j += 8)
            {
                if (S[j] == null)
                    p.AddMove(j, Power.RookMove);
                else
                {
                    if (j != i && S[j].Colour != col)  // If piece slide end is another colour, consider taking it
                        p.AddMove(j, Power.RookCapture);
                    break;
                }
            }

            // Slide piece east until it hits something
            for (int offset = 1; ExistsAtOffset(i, 0, offset); offset++)
            {
                if (IsEmpty(i + offset))
                    p.AddMove(i + offset, Power.RookMove);
                else
                {
                    if (S[i + offset].Colour != col)  // If piece slide end is another colour, consider taking it
                        p.AddMove(i + offset, Power.RookCapture);
                    break;
                }
            }

            // Slide piece west until it hits something
            for (int offset = -1; ExistsAtOffset(i, 0, offset); offset--)
            {
                if (IsEmpty(i + offset))
                    p.AddMove(i + offset, Power.RookMove);
                else
                {
                    if (S[i + offset].Colour != col)  // If piece slide end is another colour, consider taking it
                        p.AddMove(i + offset, Power.RookCapture);
                    break;
                }
            }
        }

        private void EnumerateKingMoves(int i)
        {
            Piece p = S[i];

            if (ExistsAtOffset(i, -1, -1) && IsEmptyOrEnemy(i - 9))
                p.AddMove((short)(i - 9), Power.KingMove);
            if (ExistsAtOffset(i, -1, 0) && IsEmptyOrEnemy(i - 8))
                p.AddMove((short)(i - 8), Power.KingMove);
            if (ExistsAtOffset(i, -1, 1) && IsEmptyOrEnemy(i - 7))
                p.AddMove((short)(i - 7), Power.KingMove);

            if (ExistsAtOffset(i, 0, -1) && IsEmptyOrEnemy(i - 1))
                p.AddMove((short)(i - 1), Power.KingMove);
            if (ExistsAtOffset(i, 0, 1) && IsEmptyOrEnemy(i + 1))
                p.AddMove((short)(i + 1), Power.KingMove);

            if (ExistsAtOffset(i, 1, -1) && IsEmptyOrEnemy(i + 7))
                p.AddMove((short)(i + 7), Power.KingMove);
            if (ExistsAtOffset(i, 1, 0) && IsEmptyOrEnemy(i + 8))
                p.AddMove((short)(i + 8), Power.KingMove);
            if (ExistsAtOffset(i, 1, 1) && IsEmptyOrEnemy(i + 9))
                p.AddMove((short)(i + 9), Power.KingMove);

            if (WhosMove == Colour.White && WhiteCanKCastle)
                if (IsEmpty(1) && IsEmpty(2) && S[0] != null &&
                    S[0].PieceType == PieceType.Rook && S[0].Colour == Colour.White)
                    p.AddMove(1, Power.KingMove);
            if (WhosMove == Colour.White && WhiteCanQCastle)
                if (IsEmpty(4) && IsEmpty(5) && IsEmpty(6) && S[7] != null &&
                    S[7].PieceType == PieceType.Rook && S[7].Colour == Colour.White)
                    p.AddMove(5, Power.KingMove);
            if (WhosMove == Colour.Black && BlackCanKCastle)
                if (IsEmpty(57) && IsEmpty(58) && S[56] != null &&
                    S[56].PieceType == PieceType.Rook && S[56].Colour == Colour.Black)
                    p.AddMove(57, Power.KingMove);
            if (WhosMove == Colour.Black && BlackCanQCastle)
                if (IsEmpty(60) && IsEmpty(61) && IsEmpty(62) && S[63] != null &&
                    S[63].PieceType == PieceType.Rook && S[63].Colour == Colour.Black)
                    p.AddMove(61, Power.KingMove);
        }

        private bool IsEmpty(int i)
        {
            return S[i] == null; // return S[i].PieceType == PieceType.Blank;
        }

        private bool IsEmptyOrEnemy(int i)
        {
            return S[i] == null || S[i].Colour != WhosMove;
        }

        private bool IsEmpty(int y, int x)
        {
            return S[(y * 8) + x] == null; //.PieceType == PieceType.Blank;
        }

        private bool IsEmptyAtOffset(int i, int y, int x)
        {
            return S[i + (y * 8) + x] == null;
            //return S[i + (y * 8) + x].PieceType == PieceType.Blank;
        }

        private bool ExistsAtOffset(int startSquare, int dirY, int dirX)
        {
            return X1012[X88[startSquare] + (dirY * 10) + dirX] >= 0;
        }

        // 0-based: white rook starts at rank 0, file 0
        private static int Rank(int i)
        {
            return rank[i];
        }

        // 0-based: white rook starts at rank 0, file 0
        private static int File(int i)
        {
            return i % 8;
        }

        public double ScoreBoard()
        {
            double score = 0;
            int pieceCount = 0;

            score += ((double)this.AllMovesCount / 200d) * (WhosMove == Colour.White ? 1d : -1d);

            for (int i = 0; i < 64; i++)
            {
                if (S[i] == null)
                    goto skip;

                pieceCount++;
                double pVal = 0;

                pVal += PieceValue(S[i].PieceType);
                pVal -= Math.Abs((double)File(i) - 3.5d) / 70d;  // favour central pieces

                // Favour advanced pieces
                if (S[i].Colour == Colour.White)
                {
                    pVal += (double)(Rank(i)) / 20d;
                    score += pVal;
                }
                else
                {
                    pVal += (double)(7d - Rank(i)) / 20d;
                    score -= pVal;
                }



            skip: ;
            }

            // Favour a secure king in early game
            if (pieceCount > 15)
                for (int i = 0; i < 64; i++)
                    if (S[i] != null && S[i].PieceType == PieceType.King)
                    {
                        score += (Math.Abs((double)File(i) - 3.5d) / 40d) * (WhosMove == S[i].Colour ? 1 : -1);
                        //score -= (S[i].AttackCnt * 0.04d) * (WhosMove == Colour.White ? 1 : -1);
                    }

            return score;
        }


        private double PieceValue(PieceType pieceType)
        {
            if (pieceType == PieceType.Blank)
                throw new ApplicationException();

            if (pieceType == PieceType.Pawn)
                return 1d;
            if (pieceType == PieceType.Knight)
                return 3.1d;
            if (pieceType == PieceType.Bishop)
                return 3.25d;
            if (pieceType == PieceType.Rook)
                return 5d;
            if (pieceType == PieceType.Queen)
                return 9d;

            // By process of elimination, it's the king
            return 1000d;
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
            if (fromPos == toPos) return false;
            if (!S[fromPos].Moves.Contains(toPos)) return false;
            if (S[fromPos].Colour != WhosMove) return false;

            return true;
        }

        public bool KingIsInCheck
        {
            get
            {
                int kingPos = -1;
                int enemyKingPos = -1;


                for (int i = 0; i < 64; i++)
                {
                    if (S[i] != null && S[i].PieceType == PieceType.King)
                    {
                        if  (S[i].Colour == WhosMove)
                            kingPos = i;
                        else
                            enemyKingPos = i;
                    }
                }

                if (kingPos == -1)
                    throw new ApplicationException("Cannot find my king");
                if (enemyKingPos == -1)
                    throw new ApplicationException("Cannot find enemy king");

                #region Diagonal search

                // See if bishop or queen is to NW
                for (int offset = 1; ExistsAtOffset(kingPos, -offset, -offset); offset++)
                {
                    int j = kingPos - (offset * 8) - offset;
                    if (!IsEmpty(j))
                    {
                        if (S[j].Colour != WhosMove && (S[j].PieceType == PieceType.Bishop || S[j].PieceType == PieceType.Queen))
                            return true;
                        break;
                    }
                }
                // See if bishop or queen is to NE
                for (int offset = 1; ExistsAtOffset(kingPos, -offset, offset); offset++)
                {
                    int j = kingPos - (offset * 8) + offset;
                    if (!IsEmpty(j))
                    {
                        if (S[j].Colour != WhosMove && (S[j].PieceType == PieceType.Bishop || S[j].PieceType == PieceType.Queen))
                            return true;
                        break;
                    }
                }
                // See if bishop or queen is to SE
                for (int offset = 1; ExistsAtOffset(kingPos, offset, -offset); offset++)
                {
                    int j = kingPos + (offset * 8) - offset;
                    if (!IsEmpty(j))
                    {
                        if (S[j].Colour != WhosMove && (S[j].PieceType == PieceType.Bishop || S[j].PieceType == PieceType.Queen))
                            return true;
                        break;
                    }
                }
                // See if bishop or queen is to SW
                for (int offset = 1; ExistsAtOffset(kingPos, offset, offset); offset++)
                {
                    int j = kingPos + (offset * 8) + offset;
                    if (!IsEmpty(j))
                    {
                        if (S[j].Colour != WhosMove && (S[j].PieceType == PieceType.Bishop || S[j].PieceType == PieceType.Queen))
                            return true;
                        break;
                    }
                }

                #endregion

                #region Horizontal and vertical search

                // Search N
                for (int j = kingPos - 8; j >= 0; j -= 8)
                {
                    if (!IsEmpty(j))
                    {
                        if (S[j].Colour != WhosMove && (S[j].PieceType == PieceType.Rook || S[j].PieceType == PieceType.Queen))
                            return true;
                        break;
                    }
                }
                // Search S
                for (int j = kingPos + 8; j < 64; j += 8)
                {
                    if (!IsEmpty(j))
                    {
                        if (S[j].Colour != WhosMove && (S[j].PieceType == PieceType.Rook || S[j].PieceType == PieceType.Queen))
                            return true;
                        break;
                    }
                }
                // Search W
                for (int j = kingPos - 1; j >= Rank(kingPos) * 8; j -= 1)
                {
                    if (!IsEmpty(j))
                    {
                        if (S[j].Colour != WhosMove && (S[j].PieceType == PieceType.Rook || S[j].PieceType == PieceType.Queen))
                            return true;
                        break;
                    }
                }
                // Search E
                for (int j = kingPos + 1; j < (Rank(kingPos) + 1) * 8; j += 1)
                {
                    if (!IsEmpty(j))
                    {
                        if (S[j].Colour != WhosMove && (S[j].PieceType == PieceType.Rook || S[j].PieceType == PieceType.Queen))
                            return true;
                        break;
                    }
                }

                #endregion

                #region Knight search

                for (int c = 0; c < 8; c++)
                {
                    int xOff = knightXoffs[c];
                    int yOff = knightYoffs[c];

                    int j = kingPos + (xOff * 8) + yOff;
                    if (ExistsAtOffset(kingPos, xOff, yOff))
                        if (S[j] != null && S[j].PieceType == PieceType.Knight && S[j].Colour != WhosMove) return true;
                }

                #endregion

                #region Pawn search

                int pawnPos1, pawnPos2, dirY;

                if (WhosMove == Colour.White)
                {
                    dirY = 1;
                    pawnPos1 = kingPos + 7;
                    pawnPos2 = kingPos + 9;
                }
                else
                {
                    dirY = -1;
                    pawnPos1 = kingPos - 9;
                    pawnPos2 = kingPos - 7;
                }

                if (ExistsAtOffset(kingPos, dirY, -1))
                    if (S[pawnPos1] != null && S[pawnPos1].PieceType == PieceType.Pawn && S[pawnPos1].Colour != WhosMove) return true;
                if (ExistsAtOffset(kingPos, dirY, 1))
                    if (S[pawnPos2] != null && S[pawnPos2].PieceType == PieceType.Pawn && S[pawnPos2].Colour != WhosMove) return true;

                #endregion

                #region King search

                if (ExistsAtOffset(kingPos, -1, -1) && enemyKingPos == kingPos - 9) return true;
                if (ExistsAtOffset(kingPos, -1, 0) && enemyKingPos == kingPos - 8) return true;
                if (ExistsAtOffset(kingPos, -1, 1) && enemyKingPos == kingPos - 7) return true;

                if (ExistsAtOffset(kingPos, 0, -1) && enemyKingPos == kingPos - 1) return true;
                if (ExistsAtOffset(kingPos, 0, 1) && enemyKingPos == kingPos + 1) return true;

                if (ExistsAtOffset(kingPos, 1, -1) && enemyKingPos == kingPos + 7) return true;
                if (ExistsAtOffset(kingPos, 1, 0) && enemyKingPos == kingPos + 8) return true;
                if (ExistsAtOffset(kingPos, 1, 1) && enemyKingPos == kingPos + 9) return true;

                #endregion

                return false;
            }
        }

        internal Board GetBoardAfterMove(int p1, int p2)
        {
            throw new NotImplementedException();
        }

        public static Board Blank
        {
            get
            {
                Board b = new Board();
                b.WhosMove = Colour.White;
                return b;
            }
        }
    }
}
