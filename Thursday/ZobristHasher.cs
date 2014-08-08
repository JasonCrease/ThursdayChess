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
            m_Hs = new long[65, 12];

            for (int i = 0; i < 65; i++)
                for (int j = 0; j < 12; j++)
                    m_Hs[i, j] = (rand.Next(int.MinValue, int.MaxValue) * int.MaxValue) + rand.Next(int.MinValue, int.MaxValue);
        }

        public long GetBoardHash(Board b)
        {
            long h = 0;

            for (int i = 0; i < 64; i++)
            {
                if (b.S[i] != null)
                {
                    int pieceType = (int)b.S[i].PieceType;
                    int pieceColour = (int)b.S[i].Colour;

                    int j = ((pieceType - 1) * 2) + pieceColour;
                    h = h ^ m_Hs[i, j];
                }
            }

            if (b.WhosMove == Colour.White) h ^= m_Hs[64, 0];
            if (b.WhosMove == Colour.Black) h ^= m_Hs[64, 1];
            if (b.BlackCanKCastle) h ^= m_Hs[64, 2];
            if (b.BlackCanQCastle) h ^= m_Hs[64, 3];
            if (b.WhiteCanKCastle) h ^= m_Hs[64, 4];
            if (b.WhiteCanQCastle) h ^= m_Hs[64, 5];

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
                // If this position has been calculated to a greater depth, use it instead 
                if (tuple.Item1 > m_Zs[hash].Item1)
                    m_Zs[hash] = tuple;
            }
            else
            {
                m_Zs[hash] = tuple;
            }
        }
    }
}
