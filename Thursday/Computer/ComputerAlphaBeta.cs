using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thursday
{
    public class ComputerAlphaBeta : Computer
    {
        private const int MaxAlphaBetaDepth = 3;
        private int betaSkips = 0;

        public override Move ComputeBestMove()
        {
            nodesVisited = 0;
            betaSkips = 0;
            Move bestMoveYet = new Move(-1, -1, -100);
            double score = AlphaBeta(b, MaxAlphaBetaDepth, double.MinValue, double.MaxValue, WhosMove == Colour.White ? 1 : -1, ref bestMoveYet);

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
                    for (int i = 0; i < b.AllMovesCount; i++)
                    {
                        Move move = b.AllMoves[i];
                        double result = AlphaBeta(b.GetBoardAfterMove(move.From, move.To), depth - 1, alpha, beta, -colour, ref bestMoveYet);
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
                    for (int i = 0; i < b.AllMovesCount; i++)
                    {
                        Move move = b.AllMoves[i];
                        double result = AlphaBeta(b.GetBoardAfterMove(move.From, move.To), depth - 1, alpha, beta, -colour, ref bestMoveYet);
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
    }
}
