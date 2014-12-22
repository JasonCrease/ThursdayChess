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
            PawnAndKing();
            QueenAndKing();
            BishopAndKing();
            RookAndKing();
            KingAndKing();
        }

        public static void QueenAndKing()
        {
            Board b1 = BoardBuilder.BuildBoard("qK.k" + new String('.', 60));
            Assert.IsTrue(b1.KingIsInCheck);

            Board b2 = BoardBuilder.BuildBoard("q......k ..K....." + new String('.', 48));
            Assert.IsFalse(b2.KingIsInCheck);
        }
        public static void BishopAndKing()
        {
            Board b1 = BoardBuilder.BuildBoard("b......k .K......" + new String('.', 48));
            Assert.IsTrue(b1.KingIsInCheck);
            Board b2 = BoardBuilder.BuildBoard("K......k .b......" + new String('.', 48));
            Assert.IsTrue(b2.KingIsInCheck);
            Board b3 = BoardBuilder.BuildBoard("..K....k .b......" + new String('.', 48));
            Assert.IsTrue(b3.KingIsInCheck);
            Board b4 = BoardBuilder.BuildBoard(".b.....k K......." + new String('.', 48));
            Assert.IsTrue(b4.KingIsInCheck);
            Board b5 = BoardBuilder.BuildBoard("bk" + new String('.', 61) + "K");
            Assert.IsTrue(b5.KingIsInCheck);

            Board b0 = BoardBuilder.BuildBoard("b......k ..K....." + new String('.', 48));
            Assert.IsFalse(b0.KingIsInCheck);
        }
        public static void RookAndKing()
        {
            Board b1 = BoardBuilder.BuildBoard("r......k .K......" + new String('.', 48));
            Assert.IsFalse(b1.KingIsInCheck);
            Board b2 = BoardBuilder.BuildBoard("K......k .r......" + new String('.', 48));
            Assert.IsFalse(b2.KingIsInCheck);
            Board b3 = BoardBuilder.BuildBoard("..K....k .r......" + new String('.', 48));
            Assert.IsFalse(b3.KingIsInCheck);
            Board b4 = BoardBuilder.BuildBoard(".r.....k K......." + new String('.', 48));
            Assert.IsFalse(b4.KingIsInCheck);

            Board b0a = BoardBuilder.BuildBoard("r......k K......." + new String('.', 48));
            Assert.IsTrue(b0a.KingIsInCheck);
            Board b0b = BoardBuilder.BuildBoard("rK...... .......k" + new String('.', 48));
            Assert.IsTrue(b0b.KingIsInCheck);
            Board b0c = BoardBuilder.BuildBoard("Kr...... .......k" + new String('.', 48));
            Assert.IsTrue(b0c.KingIsInCheck);
            Board b0d = BoardBuilder.BuildBoard("K....... r......k" + new String('.', 48));
            Assert.IsTrue(b0d.KingIsInCheck);
        }
        public static void PawnAndKing()
        {
            Board b1 = BoardBuilder.BuildBoard("K....... .p.....k" + new String('.', 48));
            Assert.IsTrue(b1.KingIsInCheck);

            Board b2 = BoardBuilder.BuildBoard(".K...... .p.....k" + new String('.', 48));
            Assert.IsFalse(b2.KingIsInCheck);

            Board b3 = BoardBuilder.BuildBoard("..K..... .p....k." + new String('.', 48));
            Assert.IsTrue(b3.KingIsInCheck);
        }
        public static void KingAndKing()
        {
            Board b1 = BoardBuilder.BuildBoard("k....... .K......" + new String('.', 48));
            Board b2 = BoardBuilder.BuildBoard(".k...... .K......" + new String('.', 48));
            Board b3 = BoardBuilder.BuildBoard("..k..... .K......" + new String('.', 48));
            Board b4 = BoardBuilder.BuildBoard("........ kK......" + new String('.', 48));
            Board b5 = BoardBuilder.BuildBoard("........ .Kk....." + new String('.', 48));
            Board b6 = BoardBuilder.BuildBoard(".K...... k......." + new String('.', 48));
            Board b7 = BoardBuilder.BuildBoard(".K...... .k......" + new String('.', 48));
            Board b8 = BoardBuilder.BuildBoard(".K...... ..k....." + new String('.', 48));

            Assert.IsTrue(b1.KingIsInCheck);
            Assert.IsTrue(b2.KingIsInCheck);
            Assert.IsTrue(b3.KingIsInCheck);
            Assert.IsTrue(b4.KingIsInCheck);
            Assert.IsTrue(b5.KingIsInCheck);
            Assert.IsTrue(b6.KingIsInCheck);
            Assert.IsTrue(b7.KingIsInCheck);
            Assert.IsTrue(b8.KingIsInCheck);
        }
    }
}
