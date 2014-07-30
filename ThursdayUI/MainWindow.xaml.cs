using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;

using Thursday;

namespace ThursdayUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Computer computer;

        public MainWindow()
        {
            InitializeComponent();
            CreatePieceBoxes();
            computer = new Computer();
            RenderBoard(computer.Board);
        }

        int gridSize = 44;
        Image[,] pieceBoxes = new Image[8, 8];
        private bool m_Flipped = false;

        public static class PieceStrings
        {
            public static string WPawn   = "\u2659";
            public static string WKnight = "\u2658";
            public static string WBishop = "\u2657";
            public static string WRook   = "\u2656";
            public static string WQueen  = "\u2655";
            public static string WKing   = "\u2654";

            public static string BPawn   = "\u265F";
            public static string BKnight = "\u265E";
            public static string BBishop = "\u265D";
            public static string BRook   = "\u265C";
            public static string BQueen  = "\u265B";
            public static string BKing   = "\u265A";
        }

        public static class PieceImages
        {
            static string imgDir = Directory.GetCurrentDirectory() + ".\\..\\..\\images\\";

            public static BitmapImage bbp = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bbp.png")));
            public static BitmapImage bbn = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bbn.png")));
            public static BitmapImage bbb = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bbb.png")));
            public static BitmapImage bbr = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bbr.png")));
            public static BitmapImage bbk = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bbk.png")));
            public static BitmapImage bbq = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bbq.png")));

            public static BitmapImage bwp = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bwp.png")));
            public static BitmapImage bwn = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bwn.png")));
            public static BitmapImage bwb = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bwb.png")));
            public static BitmapImage bwr = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bwr.png")));
            public static BitmapImage bwk = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bwk.png")));
            public static BitmapImage bwq = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bwq.png")));

            public static BitmapImage wbp = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wbp.png")));
            public static BitmapImage wbn = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wbn.png")));
            public static BitmapImage wbb = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wbb.png")));
            public static BitmapImage wbr = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wbr.png")));
            public static BitmapImage wbk = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wbk.png")));
            public static BitmapImage wbq = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wbq.png")));

            public static BitmapImage wwp = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wwp.png")));
            public static BitmapImage wwn = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wwn.png")));
            public static BitmapImage wwb = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wwb.png")));
            public static BitmapImage wwr = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wwr.png")));
            public static BitmapImage wwk = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wwk.png")));
            public static BitmapImage wwq = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wwq.png")));

            public static BitmapImage bsq = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bsq.png")));
            public static BitmapImage wsq = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wsq.png")));

            public static BitmapImage For(Colour colour, Colour onColour, PieceType pieceType)
            {
                if (colour == Colour.White && onColour == Colour.White)
                {
                    if (pieceType == PieceType.Pawn) return wwp;
                    if (pieceType == PieceType.Knight) return wwn;
                    if (pieceType == PieceType.Bishop) return wwb;
                    if (pieceType == PieceType.Rook) return wwr;
                    if (pieceType == PieceType.Queen) return wwq;
                    if (pieceType == PieceType.King) return wwk;
                }
                if (colour == Colour.Black && onColour == Colour.White)
                {
                    if (pieceType == PieceType.Pawn) return wbp;
                    if (pieceType == PieceType.Knight) return wbn;
                    if (pieceType == PieceType.Bishop) return wbb;
                    if (pieceType == PieceType.Rook) return wbr;
                    if (pieceType == PieceType.Queen) return wbq;
                    if (pieceType == PieceType.King) return wbk;
                }
                if (colour == Colour.White && onColour == Colour.Black)
                {
                    if (pieceType == PieceType.Pawn) return bwp;
                    if (pieceType == PieceType.Knight) return bwn;
                    if (pieceType == PieceType.Bishop) return bwb;
                    if (pieceType == PieceType.Rook) return bwr;
                    if (pieceType == PieceType.Queen) return bwq;
                    if (pieceType == PieceType.King) return bwk;
                }
                if (colour == Colour.Black && onColour == Colour.Black)
                {
                    if (pieceType == PieceType.Pawn) return bbp;
                    if (pieceType == PieceType.Knight) return bbn;
                    if (pieceType == PieceType.Bishop) return bbb;
                    if (pieceType == PieceType.Rook) return bbr;
                    if (pieceType == PieceType.Queen) return bbq;
                    if (pieceType == PieceType.King) return bbk;
                }

                if (pieceType == PieceType.Blank)
                {
                    if (onColour == Colour.White) return wsq;
                    else return bsq;
                }

                throw new ApplicationException();  
            }
        }

        private void CreatePieceBoxes()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Image image = new Image();
                    image.Width = gridSize;
                    image.Height = gridSize;
                    image.Margin = new Thickness(i * gridSize, j * gridSize, 0, 0);
                    image.Tag = j * 8 + i;

                    this.boardCanvas.Children.Add(image);
                    if (i % 2 == j % 2) image.Source = PieceImages.wsq;
                    else image.Source = PieceImages.bsq;

                    pieceBoxes[i, j] = image;

                    image.MouseLeftButtonDown += new MouseButtonEventHandler(BoxClick);
                }
            }
        }

        private Image fromImage;
        private Image toImage;

        public void BoxClick(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;
            image.Opacity = 0.8f;
            image.UpdateLayout();

            if (fromImage == null)
            {
                fromImage = image;
            }
            else
            {
                toImage = image;
                HumanMoved();
            }
        }

        private void HumanMoved()
        {
            int oldPos = (int)fromImage.Tag;
            int destPos = (int)toImage.Tag;

            if (!m_Flipped)
            {
                oldPos = 63 - oldPos;
                destPos = 63 - destPos;
            }

            try
            {
                computer.MakeMove(oldPos, destPos);
                //Dispatcher.Invoke(DispatcherPriority.Normal, new Action<Board>(RenderBoard), computer.Board);
                //RenderBoard(computer.Board);
                computer.ComputeNegamaxMove();
                labelScoreBoard.Content = String.Format("{0:0.00}", computer.Board.ScoreBoard());
                RenderBoard(computer.Board);
            }
            catch (ApplicationException e)
            {
                MessageBox.Show(e.Message);
            }

            fromImage.Opacity = 1f;
            fromImage = null;
            toImage.Opacity = 1f;
            toImage = null;
        }

        private void buttonGo_Click(object sender, RoutedEventArgs e)
        {
            computer.BuildInitialBoard();
            RenderBoard(computer.Board);
        }

        private void RenderBoard(Board board)
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    Piece piece;

                    if(m_Flipped)
                        piece = board.S[i * 8 + j];
                    else
                        piece = board.S[63 - (i * 8 + j)];

                    Colour squareColour = i % 2 == j % 2 ? Colour.White : Colour.Black;
                    pieceBoxes[j, i].Source = PieceImages.For(piece.Colour, squareColour, piece.PieceType);
                }

            boardCanvas.UpdateLayout();
            this.UpdateLayout();
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            Move bestMove = computer.ComputeAlphaBetaMove();
            computer.MakeMove(bestMove.From, bestMove.To);
            labelScoreBoard.Content = String.Format("{0:0.00}", computer.Board.ScoreBoard());
            RenderBoard(computer.Board);
        }

        private void buttonFlip_Click(object sender, RoutedEventArgs e)
        {
            m_Flipped = !m_Flipped;
            RenderBoard(computer.Board);
        }

        private void buttonScoreBoard_Click(object sender, RoutedEventArgs e)
        {
            labelScoreBoard.Content = String.Format("{0:0.00}", computer.Board.ScoreBoard());
        }
    }
}
