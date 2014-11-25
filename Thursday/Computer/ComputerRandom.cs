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
            int r = rand.Next(0, b.AllMovesCount);

            return new Move(b.AllMoves[r].From, b.AllMoves[r].To, -100);
        }
    }
}
