using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thursday
{
    public class ComputerRandom : Computer
    {
        static Random rand = new Random();

        public override Move ComputeBestMove()
        {
            for (int i = 0; i < 100; i++)
            {
                int r = rand.Next(0, b.AllMovesCount);

                Move randMove = new Move(b.AllMoves[r].From, b.AllMoves[r].To, -100);
                Board boardAfterMove = new Board(b);
                if (boardAfterMove.MakeMove(randMove.From, randMove.To)) return randMove;
            }

            throw new ApplicationException("Can't find possible move");
        }
    }
}
