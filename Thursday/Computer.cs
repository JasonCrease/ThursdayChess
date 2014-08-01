using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thursday
{
    public enum Colour
    {
        White, Black, Blank
    }

    public enum PieceType
    {
        Blank = 0,
        Pawn = 1,
        Knight = 2,
        Bishop = 3,
        Rook = 4,
        Queen = 5,
        King = 6
    }

    public class Computer
    {
        Board b;
        private ZobristHasher m_Zasher;

        public Computer()
        {
            BuildInitialBoard();
            m_Zasher = new ZobristHasher();
        }

        public void BuildInitialBoard()
        {
            b = new Board();
            b.SetupStandardBoard();
        }

        static Random rand = new Random();
        Colour WhosMove;

        public Move ComputeOneMoveLookaheadMove()
        {
            var moveScores = new List<Tuple<Move, double>>();

            foreach (Move move in b.AllMoves)
            {
                Board bd = b.MakeMove(move.From, move.To);
                double score = bd.ScoreBoard();
                moveScores.Add(new Tuple<Move, double>(move, score));
            }

            var bestMove = moveScores[0];

            if (WhosMove == Colour.White)
            {
                foreach (var move in moveScores)
                    if (move.Item2 < bestMove.Item2)
                        bestMove = move;
                //bestMove = moveScores.OrderByDescending(x => x.Item2).First().Item1;
            }
            if (WhosMove == Colour.Black)
            {
                foreach (var move in moveScores)
                    if (move.Item2 > bestMove.Item2)
                        bestMove = move;
                //bestMove = moveScores.OrderBy(x => x.Item2).First().Item1;
            }

            return bestMove.Item1;
        }

        private List<Tuple<Move, double>> m_RankedMoves = new List<Tuple<Move, double>>();

        public Move ComputeNegamaxMove()
        {
            nodesVisited = 0;
            m_RankedMoves = new List<Tuple<Move, double>>();
            double score = Negamax(b, MaxNegamaxDepth, double.MinValue, double.MaxValue, WhosMove == Colour.White ? 1 : -1);

            return m_RankedMoves.OrderByDescending(x => x.Item2).First().Item1;
        }

        private const int MaxNegamaxDepth = 4;
        private int hashUsed, hashNotUsed;

        private double Negamax(Board b, int depth, double alpha, double beta, int colour)
        {
            Tuple<int, double> tuple = m_Zasher.GetDepthScoreTuple(b);
            if (tuple != null && tuple.Item1 >= depth)
            {
                if (depth > 0) hashUsed++;
                return colour * tuple.Item2;
            }

            if (depth == 0)
            {
                double score = b.ScoreBoard();
                m_Zasher.SetDepthScoreTuple(b, new Tuple<int, double>(0, score));
                hashNotUsed++;
                return colour * score;
            }
            else
            {
                foreach (var move in b.AllMoves)
                {
                    Board boardAfterMove = b.MakeMove(move.From, move.To);
                    double score = -Negamax(boardAfterMove, depth - 1, -beta, -alpha, -colour);

                    //m_Zasher.AddIfBetter(boardAfterMove, new Tuple<int, double>(depth, colour * score));

                    nodesVisited++;
                    if (score >= beta)
                        return score;
                    if (score > alpha)
                    {
                        alpha = score;
                    }

                    if (depth == MaxNegamaxDepth)
                    {
                        if (!boardAfterMove.MoverIsInCheck())
                            m_RankedMoves.Add(new Tuple<Move, double>(move, score));
                    }
                }

                return alpha;
            }
        }

        private const int MaxAlphaBetaDepth = 4;
        private int nodesVisited;
        private int betaSkips = 0;

        public Move ComputeAlphaBetaMove()
        {
            nodesVisited = 0;
            betaSkips = 0;
            Move bestMoveYet = new Move(-1, -1);
            double score = AlphaBeta(b, MaxAlphaBetaDepth, double.MinValue, double.MaxValue, WhosMove == Colour.White ? 1 : -1, ref bestMoveYet);

            bool bawge = b.MoverIsInCheck();

            return bestMoveYet;
        }

        private double AlphaBeta(Board b, int depth, double alpha, double beta, int colour, ref Move bestMoveYet)
        {
            if (depth == 0)
                return b.ScoreBoard();
            else
            {
                if (colour == 1)
                {
                    foreach (var move in b.AllMoves)
                    {
                        double result = AlphaBeta(b.MakeMove(move.From, move.To), depth - 1, alpha, beta, -colour, ref bestMoveYet);
                        nodesVisited++;

                        if (result > alpha)
                        {
                            alpha = result;
                            if (depth == MaxAlphaBetaDepth) bestMoveYet = move;
                        }
                        if (beta <= alpha)
                        {
                            betaSkips++;
                            goto skip1;
                        }
                    }
                skip1:
                    return alpha;
                }
                else if (colour == -1)
                {
                    foreach (var move in b.AllMoves)
                    {
                        double result = AlphaBeta(b.MakeMove(move.From, move.To), depth - 1, alpha, beta, -colour, ref bestMoveYet);
                        nodesVisited++;

                        if (result < beta)
                        {
                            beta = result;
                            if (depth == MaxAlphaBetaDepth) bestMoveYet = move;
                            //bestMoveYet = move;
                        }
                        if (beta <= alpha)
                        {
                            betaSkips++;
                            goto skip2;
                        }
                    }
                skip2:
                    return beta;
                }
                else throw new ApplicationException();
            }

            throw new ApplicationException();
        }

        public Tuple<int, int> ComputeRandomMove()
        {
            int r = rand.Next(0, b.AllMoves.Count);

            return new Tuple<int, int>(b.AllMoves[r].From, b.AllMoves[r].To);
        }

        public void MakeMove(int fromPos, int toPos)
        {
            if (!Board.MoveIsLegal(fromPos, toPos))
                throw new ApplicationException("Move is illegal. Try another");

            Board = Board.MakeMove(fromPos, toPos);
            WhosMove = Board.WhosMove;
        }

        public Board Board
        {
            get
            {
                return b;
            }
            set
            {
                b = value;
            }
        }

        public void MakeNegamaxMove()
        {
            Move bestMove = ComputeNegamaxMove();
            MakeMove(bestMove.From, bestMove.To);
        }
        public void MakeAlphaBetaMove()
        {
            Move bestMove = ComputeAlphaBetaMove();
            MakeMove(bestMove.From, bestMove.To);
        }
    }
}
