using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thursday;
using NUnit.Framework;

namespace ThursdayTests
{
    public class BestMoveTests
    {
        public static void RunAll()
        {
            TakeQueenSimple();
        }

        private static void TakeQueenSimple()
        {
            Move m1 = GetBestMoveForBoard("Qq.K...k" + new String('.', 56));
            Assert.That(m1.From == 0);
            Assert.That(m1.To == 1);

            Move m2 = GetBestMoveForBoard("Rq.K...k" + new String('.', 56));
            Assert.That(m2.From == 0);
            Assert.That(m2.To == 1);

            Move m3 = GetBestMoveForBoard(".q.K...k ...N...." + new String('.', 48));
            Assert.That(m3.From == 11);
            Assert.That(m3.To == 1);
        }

        private static Move GetBestMoveForBoard(string boardStr)
        {
            Computer c = GetComputerForBoard(boardStr);
            c.Difficulty = 3;
            Move m = c.ComputeBestMove();

            return m;
        }

        private static Computer GetComputerForBoard(string boardStr)
        {
            Computer c = new ComputerNegamax();
            c.Board = BoardBuilder.BuildBoard(boardStr);

            return c;
        }
    }
}
