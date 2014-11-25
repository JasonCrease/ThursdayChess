using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thursday
{
    public class ComputerNegamax : Computer
    {
        private int MaxNegamaxDepth;

        public override Move ComputeBestMove()
        {
            double score = 0f;
            nodesVisited = 0;
            m_RankedMoves = new List<Tuple<Move, double>>();
            MaxNegamaxDepth = this.Difficulty;

            score = Negamax(b, MaxNegamaxDepth, double.MinValue, double.MaxValue, WhosMove == Colour.White ? 1 : -1);

            if (m_RankedMoves.Count() == 0)
                if(b.KingIsInCheck)
                    throw new ApplicationException("Checkmate!");
                else
                    throw new ApplicationException("Stalemate!");

            if (WhosMove == Colour.Black)
                return m_RankedMoves.OrderBy(x => x.Item2).First().Item1;
            else if (WhosMove == Colour.White)
                return m_RankedMoves.OrderByDescending(x => x.Item2).First().Item1;
            else
                throw new ApplicationException();
        }

        private int hashUsed, hashNotUsed;

        private double Negamax(Board b, int depth, double alpha, double beta, int colour)
        {
            Tuple<int, double> tuple = m_Zasher.GetDepthScoreTuple(b);
            // If the same or better calculation of this state has already been made, use it.
            if (tuple != null && tuple.Item1 >= depth && depth < MaxNegamaxDepth)
            {
                if (depth >  0) hashUsed++;
                if (depth >= 0) hashUsed++;
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
                for (int i = 0; i < b.AllMovesCount; i++)
                {
                    Move move = b.AllMoves[i];
                    Board boardAfterMove = b.MakeMove(move.From, move.To);

                    if (!b.KingIsInCheck)
                    {
                        double score = -Negamax(boardAfterMove, depth - 1, -beta, -alpha, -colour);

                        // Cache this score
                        m_Zasher.AddIfBetter(boardAfterMove, new Tuple<int, double>(depth - 1, colour * score));

                        nodesVisited++;
                        if (score >= beta)
                            return score;
                        if (score > alpha)
                        {
                            alpha = score;
                        }

                        if (depth == MaxNegamaxDepth)
                        {
                            if (!boardAfterMove.YouCanTakeOpponentsKing())
                            {
                                m_RankedMoves.Add(new Tuple<Move, double>(move, colour * score));
                                m_Zasher.AddIfBetter(b, new Tuple<int, double>(depth, colour * score));
                            }
                        }
                    }
                }

                return alpha;
            }
        }
    }
}
