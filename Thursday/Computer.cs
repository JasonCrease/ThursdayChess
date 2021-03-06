﻿using System;
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
            b = Board.InitialStandardBoard;
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

            Board newBoard = new Board(Board);
            bool isLegal = newBoard.MakeMove(fromPos, toPos);

            if (isLegal)
            {
                m_OldBoards.Push(Board);
                Board = newBoard;
                WhosMove = newBoard.WhosMove;
            }
            else
            {
                throw new ApplicationException("Move is illegal. Try another");
            }
        }

        public abstract Move ComputeBestMove();

        public void CalculateAndMakeMoveAsync(Action MoveCalculated)
        {
            System.Threading.Thread t = new System.Threading.Thread(ComputeAndMakeBestMove);
            t.Start(MoveCalculated);
        }

        Stopwatch m_StopWatch = new Stopwatch();
        long m_MsTaken = 0L;

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
            m_MsTaken = m_StopWatch.ElapsedMilliseconds;

            if (action != null) action();
        }

        public int Difficulty { get; set; }
        public long MsTaken { get { return m_MsTaken; } }
    }
}
