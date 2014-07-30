using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using PgnParser;
using Thursday;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string wholeFile = "";
            List<Tuple<Board, Move>> allData = new List<Tuple<Board, Move>>();

            using(StreamReader fs = new StreamReader(File.OpenRead(".\\Ivanchuk.pgn")))
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
                    board = board.MakeMove(enu.Current.From, enu.Current.To);
                    allData.Add(new Tuple<Board, Move>(board, enu.Current));
                }
            }

            Board b = allData[263].Item1;
            var mostSimilars = allData.Where(x => x.Item1.WhosMove == b.WhosMove).
                                       OrderBy(x => Similarity.Calculate(x.Item1, b)).ToList();
            Move suggestedMove = new Move(-1, -1);

            for (int i = 0; i < mostSimilars.Count; i++)
            {
                Tuple<Board, Move> tup = mostSimilars[i];
                suggestedMove = tup.Item2;
                if (b.AllMoves.Contains(suggestedMove))
                    break;
            }
        }
    }
}