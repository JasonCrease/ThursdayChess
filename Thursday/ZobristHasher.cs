using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thursday
{
    public class ZobristHasher
    {
        long[,] m_Hs;
        private Dictionary<long, Tuple<int, double>> m_Zs;
 
        public ZobristHasher()
        {
            InitHashes();
            m_Zs = new Dictionary<long, Tuple<int, double>>();
        }

        private void InitHashes()
        {
            Random rand = new Random();
            m_Hs = new long[64, 12];

            for (int i = 0; i < 64; i++)
                for (int j = 0; j < 12; j++)
                    m_Hs[i, j] = (rand.Next(int.MinValue, int.MaxValue) * int.MaxValue) + rand.Next(int.MinValue, int.MaxValue);
        }

        public long GetBoardHash(Board b)
        {
            long h = 0;

            for (int i = 0; i < 64; i++)
            {
                int pieceType = (int)b.S[i].PieceType;
                int pieceColour = (int)b.S[i].Colour;

                if (pieceType != 0)
                {
                    int j = ((pieceType - 1) * 2) + pieceColour;
                    h = h ^ m_Hs[i, j];
                }
            }

            return h;
        }

        public Tuple<int, double> GetDepthScoreTuple(Board b)
        {
            long hash = GetBoardHash(b);

            if (m_Zs.ContainsKey(hash))
                return m_Zs[hash];

            return null;
        }

        public void SetDepthScoreTuple(Board b, Tuple<int, double> tuple)
        {
            long hash = GetBoardHash(b);
            m_Zs[hash] = tuple;
        }

        internal void AddIfBetter(Board b, Tuple<int, double> tuple)
        {
            long hash = GetBoardHash(b);

            if (m_Zs.ContainsKey(hash))
            {
                if (m_Zs[hash].Item1 > tuple.Item1)
                    m_Zs[hash] = tuple;
            }
            else
            {
                m_Zs[hash] = tuple;
            }
        }
    }
}
