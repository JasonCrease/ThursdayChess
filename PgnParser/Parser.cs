using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Thursday;

namespace PgnParser
{
    public class Parser
    {
        private static char[] whiteSpaces = new char[] { ' ', '\n', '\r', '\t' };
        static List<Tuple<Board, Move>> allData = new List<Tuple<Board, Move>>();

        static Parser()
        {
            string wholeFile = "";

            using (StreamReader fs = new StreamReader(File.OpenRead(".\\Ivanchuk.pgn")))
                wholeFile = fs.ReadToEnd();

            string[] games = Regex.Split(wholeFile, "\\[Event.*\\]\r\n\r\n");

            PgnParser.Parser parser = new PgnParser.Parser();

            for (int i = 0; i < 100; i++)
            {
                Board board = new Board();
                var enu = parser.Parse(games[i]).GetEnumerator();
                board.SetupStandardBoard();

                while (enu.MoveNext())
                {
                    allData.Add(new Tuple<Board, Move>(board, enu.Current));
                    board = board.MakeMove(enu.Current.From, enu.Current.To);
                }
            }
        }

        public static Move GetBestMove(Board b)
        {
            List<Tuple<Board, Move>> possibleNextBoards = new List<Tuple<Board, Move>>();

            foreach (Move posMove in b.AllMoves)
            {
                possibleNextBoards.Add(new Tuple<Board, Move>(b.MakeMove(posMove.From, posMove.To), posMove));
            }

            List<Tuple<Move, double>> moveScores = new List<Tuple<Move, double>>();

            foreach (var posNextBoard in possibleNextBoards)
            {
                var bestPossibleBoard = allData.Where(x => x.Item1.WhosMove == posNextBoard.Item1.WhosMove)
                                               .OrderBy(x => Similarity.Calculate(x.Item1, posNextBoard.Item1))
                                               .First();
                double score = Similarity.Calculate(bestPossibleBoard.Item1, posNextBoard.Item1);

                moveScores.Add(new Tuple<Move, double>(posNextBoard.Item2, score));
            }

            Move suggestedMove = moveScores.OrderBy(x => x.Item2).First().Item1;

            return suggestedMove;
        }

        private static Random r = new Random();

        public IEnumerable<Move> Parse(string pgn)
        {
            string tmp = RemoveNonsense(pgn);
            string[] byMoves = SplitIntoMoves(tmp);

            bool whiteMove = true;

            Thursday.Board board = new Thursday.Board();
            board.SetupStandardBoard();

            int curMove = 0;

            foreach (string move in byMoves)
            {
                string[] ms = move.Split(whiteSpaces, StringSplitOptions.RemoveEmptyEntries);

                if (ms.Length == 0)
                {
                    // Boring.  Just continue
                }
                else if (ms.Length == 2)
                {
                    if (IsEndStatement(ms[0])) goto ended;

                    Move move1 = ParseMove(ms[0], board);
                    board = board.MakeMove(move1.From, move1.To);
                    curMove++;

                    PrintDebug(curMove, board);

                    yield return move1;

                    if (IsEndStatement(ms[1])) goto ended;

                    Move move2 = ParseMove(ms[1], board);
                    board = board.MakeMove(move2.From, move2.To);
                    curMove++;

                    PrintDebug(curMove, board);

                    yield return move2;
                }
                else if (ms.Length == 1)
                {
                    if (IsEndStatement(ms[0])) goto ended;

                    Move move1 = ParseMove(ms[0], board);
                    board = board.MakeMove(move1.From, move1.To);
                    curMove++;

                    PrintDebug(curMove, board);

                    yield return move1;
                }
                else if (ms.Length == 3 && IsEndStatement(ms[2]))
                {
                    Move move1 = ParseMove(ms[0], board);
                    board = board.MakeMove(move1.From, move1.To);
                    curMove++;

                    PrintDebug(curMove, board);

                    yield return move1;

                    Move move2 = ParseMove(ms[1], board);
                    board = board.MakeMove(move2.From, move2.To);
                    curMove++;

                    PrintDebug(curMove, board);

                    yield return move2;

                    goto ended;
                }
                else
                {
                    throw new ApplicationException("Not 0 1 or 2 moves found");
                }
            }

        ended:
            ;
        }

        private void PrintDebug(int curMove, Thursday.Board board)
        {
            //Console.WriteLine(curMove);
            //Console.WriteLine(board.ToString());
            //curMove++;
        }

        private bool IsEndStatement(string statement)
        {
            if (statement == "1/2-1/2") return true;
            if (statement == "1-0") return true;
            if (statement == "0-1") return true;

            return false;
        }

        private Move ParseMove(string m, Thursday.Board board)
        {
            int dest = 0;
            int from = 0;

            if (m == "O-O-O")
            {
                if (board.WhosMove == Colour.White)
                {
                    from = 3;
                    dest = 5;
                }
                else
                {
                    from = 59;
                    dest = 61;
                }
            }
            else if (m == "O-O")
            {
                if (board.WhosMove == Colour.White)
                {
                    from = 3;
                    dest = 1;
                }
                else
                {
                    from = 59;
                    dest = 57;
                }
            }
            else
            {
                PieceType piece = GetPiece(m);

                if (piece == PieceType.Pawn)
                {
                    if (m.Length == 2)  // Simple pawn move
                    {
                        dest = GetLocation(m);
                        from = (board.AllMoves.First(x => x.To == dest && board.S[x.From].PieceType == PieceType.Pawn)).From;
                    }
                    else if (m.Length == 4 && m[1] == 'x' ||            // Pawn capture
                        (m.Length == 6 && m[4] == '=')     // Pawn capture with Promotion
                        )  
                    {
                        dest = GetLocation(m.Substring(2));
                        from = (board.AllMoves.First(x => x.To == dest &&
                                board.S[x.From].PieceType == PieceType.Pawn &&
                                x.From % 8 == 104 - m[0])).From;         //must be on correct rank
                    }
                    else if (m.Length == 4 && m[2] == '=')  // Promotion
                    {
                        dest = GetLocation(m);
                        from = (board.AllMoves.First(x => x.To == dest && board.S[x.From].PieceType == PieceType.Pawn)).From;
                    }
                    else throw new ApplicationException();
                }
                else
                {
                    // string rest = m.Substring(1);

                    if (m.Length == 3)  // a simple piece move
                    {
                        dest = GetLocation(m.Substring(1));
                        from = (board.AllMoves.First(x => x.To == dest &&
                                board.S[x.From].PieceType == piece)).From;
                    }
                    else if (m.Length == 4 && m[1] == 'x')  // capture
                    {
                        dest = GetLocation(m.Substring(2));
                        from = (board.AllMoves.First(x => x.To == dest &&
                                board.S[x.From].PieceType == piece)).From;
                    }
                    else if (m.Length == 4 && m[1] != 'x')  // ambiguous piece move
                    {
                        dest = GetLocation(m.Substring(2));

                        if (IsRank(m[1]))
                        {
                            from = (board.AllMoves.First(x => x.To == dest &&
                                x.From % 8 == 104 - m[1] &&                    //must be on correct rank
                                board.S[x.From].PieceType == piece)).From;
                        }
                        else if (IsFile(m[1]))
                        {
                            from = (board.AllMoves.First(x => x.To == dest &&
                                x.From / 8 == m[1] - 49 &&                    //must be on correct file
                                board.S[x.From].PieceType == piece)).From;
                        }
                        else throw new ApplicationException();
                    }
                    else if (m.Length == 5 && m[2] == 'x')  // ambiguous piece capture
                    {
                        dest = GetLocation(m.Substring(3));

                        if (IsRank(m[1]))
                        {
                            from = (board.AllMoves.First(x => x.To == dest &&
                                    x.From % 8 == 104 - m[1] &&                    //must be on correct rank
                                    board.S[x.From].PieceType == piece)).From;
                        }
                        else if (IsFile(m[1]))
                        {
                            from = (board.AllMoves.First(x => x.To == dest &&
                                x.From / 8 == m[1] - 49 &&                    //must be on correct file
                                board.S[x.From].PieceType == piece)).From;
                        }
                        else throw new ApplicationException();
                    }
                    else throw new ApplicationException();
                }
            }

            return new Move(from, dest);
        }

        private bool IsRank(char c)
        {
            return c >= 97 && c <= 104;
        }

        private bool IsFile(char c)
        {
            return c >= 49 && c <= 56;
        }

        private int GetLocation(string s)
        {
            int rank = 104 - s[0];
            int file = s[1] - 49;

            if (rank > 7 || rank < 0 || file > 8 || file < 0)
                throw new ApplicationException();

            return file * 8 + rank;
        }

        private PieceType GetPiece(string m)
        {
            char p = m[0];
            if (p >= 97 && p <= 104) return PieceType.Pawn;
            else if (p == 'N') return PieceType.Knight;
            else if (p == 'B') return PieceType.Bishop;
            else if (p == 'R') return PieceType.Rook;
            else if (p == 'Q') return PieceType.Queen;
            else if (p == 'K') return PieceType.King;
            else throw new ApplicationException("Unknown piece " + m[0]);
        }

        private string RemoveNonsense(string pgn)
        {
            pgn = pgn.Replace('\r', ' ');
            pgn = pgn.Replace('\n', ' ');
            pgn = pgn.Replace('+', ' ');
            pgn = pgn.Replace('#', ' ');
            //pgn = pgn.Replace("...", " ");
            pgn = Regex.Replace(pgn, "{.*}", "");
            pgn = Regex.Replace(pgn, "\\[.*\\]", "");

            return pgn;
        }

        private string[] SplitIntoMoves(string pgn)
        {
            string[] moves = Regex.Split(pgn, @"\d{1,3}\.{1,3}");
            return moves;
        }
    }
}
