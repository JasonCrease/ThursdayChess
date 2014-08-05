using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public List<Tuple<Move, double>> RankedMoves
        {
            get { return m_RankedMoves;  }
        }

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

        private Stack<Board> m_OldBoards = new Stack<Board>();
 
        public void TakeBack()
        {
            if (m_OldBoards.Any())
            {
                Board = m_OldBoards.Pop();
                Board = m_OldBoards.Pop();
            }
        }

        public void MakeMove(int fromPos, int toPos)
        {
            if (!Board.MoveIsLegal(fromPos, toPos))
                throw new ApplicationException("Move is illegal. Try another");

            m_OldBoards.Push(Board);
            Board = Board.MakeMove(fromPos, toPos);
            WhosMove = Board.WhosMove;
        }

        public abstract Move ComputeBestMove();

        public void CalculateAndMakeMoveAsync(Action MoveCalculated)
        {
            System.Threading.Thread t = new System.Threading.Thread(ComputeAndMakeBestMove);
            t.Start(MoveCalculated);
        }

        Stopwatch m_StopWatch = new Stopwatch();

        public void ComputeAndMakeBestMove(object MoveCalculatedAction)
        {
            m_StopWatch.Reset();
            m_StopWatch.Start();
            IsThinking = true;

            Action action = (Action)MoveCalculatedAction;
            Move m = ComputeBestMove();
            this.MakeMove(m.From, m.To);

            IsThinking = false;
            m_StopWatch.Stop();

            action();
        }

        public int Difficulty { get; set; }
        public long MsTaken { get { return m_StopWatch.ElapsedMilliseconds; } }
    }
}
