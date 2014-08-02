using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thursday
{
    public enum MoveType
    {
        Simple,
        Capture,
        QCastle,
        KCastle,
        Promotion
   } 

    public struct Move
    {
        public int From;
        public int To;

        public Move(int from, int to)
        {
            From = from;
            To = to;
        }
    }
}
