using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thursday
{
    public abstract class Computer
    {
        protected Board b;
        protected ZobristHasher m_Zasher;
        protected Colour WhosMove;
        protected List<Tuple<Move, double>> m_RankedMoves;
        protected int nodesVisited;

        public bool IsThinking { get; private set; }

        public Board Board
        {
            get { return b; }
            set { b = value; }
        }

        public Computer()
        {
            BuildInitialBoard();
            m_Zasher = new ZobristHasher();
            m_RankedMoves = new List<Tuple<Move, double>>();
        }

        public void BuildInitialBoard()
        {
            b = new Board();
            b.SetupStandardBoard();
        }

        public void MakeMove(int fromPos, int toPos)
        {
            if (!Board.MoveIsLegal(fromPos, toPos))
                throw new ApplicationException("Move is illegal. Try another");

            Board = Board.MakeMove(fromPos, toPos);
            WhosMove = Board.WhosMove;
        }

        public abstract Move ComputeBestMove();

        public void CalculateAndMakeMoveAsync(Action MoveCalculated)
        {
            IsThinking = true;
            Move m = ComputeBestMove();
            MakeMove(m.From, m.To);
            MoveCalculated();
            IsThinking = false;
        }
    }
}
