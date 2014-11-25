using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thursday;
using NUnit.Framework;

namespace ThursdayTests
{
    public class CheckTests
    {
        public static void RunAll()
        {
            QueenAndKing();
            BishopAndKing();
            RookAndKing();
        }

        public static void QueenAndKing()
        {
            Board b1 = BoardBuilder.BuildBoard("qK" + new String('.', 62));
            Assert.IsTrue(b1.KingIsInCheck);

            Board b2 = BoardBuilder.BuildBoard("q....... ..K....." + new String('.', 48));
            Assert.IsFalse(b2.KingIsInCheck);
        }
        public static void BishopAndKing()
        {
            Board b1 = BoardBuilder.BuildBoard("b....... .K......" + new String('.', 48));
            Assert.IsTrue(b1.KingIsInCheck);
            Board b2 = BoardBuilder.BuildBoard("K....... .b......" + new String('.', 48));
            Assert.IsTrue(b2.KingIsInCheck);
            Board b3 = BoardBuilder.BuildBoard("..K..... .b......" + new String('.', 48));
            Assert.IsTrue(b3.KingIsInCheck);
            Board b4 = BoardBuilder.BuildBoard(".b...... K......." + new String('.', 48));
            Assert.IsTrue(b4.KingIsInCheck);

            Board b0 = BoardBuilder.BuildBoard("b....... ..K....." + new String('.', 48));
            Assert.IsFalse(b0.KingIsInCheck);
        }
        public static void RookAndKing()
        {
            Board b1 = BoardBuilder.BuildBoard("r....... .K......" + new String('.', 48));
            Assert.IsFalse(b1.KingIsInCheck);
            Board b2 = BoardBuilder.BuildBoard("K....... .r......" + new String('.', 48));
            Assert.IsFalse(b2.KingIsInCheck);
            Board b3 = BoardBuilder.BuildBoard("..K..... .r......" + new String('.', 48));
            Assert.IsFalse(b3.KingIsInCheck);
            Board b4 = BoardBuilder.BuildBoard(".r...... K......." + new String('.', 48));
            Assert.IsFalse(b4.KingIsInCheck);

            Board b0a = BoardBuilder.BuildBoard("r....... K......." + new String('.', 48));
            Assert.IsTrue(b0a.KingIsInCheck);
            Board b0b = BoardBuilder.BuildBoard("rK...... ........" + new String('.', 48));
            Assert.IsTrue(b0b.KingIsInCheck);
            Board b0c = BoardBuilder.BuildBoard("Kr...... ........" + new String('.', 48));
            Assert.IsTrue(b0c.KingIsInCheck);
            Board b0d = BoardBuilder.BuildBoard("K....... r......." + new String('.', 48));
            Assert.IsTrue(b0d.KingIsInCheck);
        }
    }
}
