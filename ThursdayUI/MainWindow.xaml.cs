﻿using System;
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
using System.Threading;

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
            computer = new ComputerNegamax();
            computer.Difficulty = 4;
            RenderBoard(computer.Board);
        }

        int gridSize = 44;
        Image[,] pieceBoxes = new Image[8, 8];
        private bool m_Flipped = true;

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
            if (!computer.IsThinking)
            {
                Image image = sender as Image;
                image.Opacity = 0.6f;
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
        }

        private void HumanMoved()
        {
            int oldPos = (int)fromImage.Tag;
            int destPos = (int)toImage.Tag;

            if (m_Flipped)
            {
                oldPos = 63 - oldPos;
                destPos = 63 - destPos;
            }

            try
            {
                computer.MakeMove(oldPos, destPos);
                RenderBoard(computer.Board);
                labelWhosTurn.Content = String.Format("Thinking");
                computer.CalculateAndMakeMoveAsync(RenderBoardAsync);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                fromImage.Opacity = 1f;
                fromImage = null;
                toImage.Opacity = 1f;
                toImage = null;
            }
        }

        public void RenderBoardAsync()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action<Board>(UpdateUi), computer.Board);
        }

        private void UpdateUi(Board board)
        {
            RenderBoard(board);
            labelScoreBoard.Content = String.Format("{0:0.00}", computer.Board.ScoreBoard());
            //if(!computer.IsThinking)
                labelComputationTime.Content = String.Format("{0:0.00}s", (float)computer.MsTaken / 1000f);
            labelWhosTurn.Content = String.Format("Your turn");
        }

        private void buttonGo_Click(object sender, RoutedEventArgs e)
        {
            computer.BuildInitialBoard();
            RenderBoard(computer.Board);
        }

        bool m_PlayingDemo = false;
        Thread m_DemoThread;

        private void buttonPlayDemo_Click(object sender, RoutedEventArgs e)
        {
            if(!m_PlayingDemo)
            {
                buttonPlayDemo.Content = "Stop autoplay";
                m_PlayingDemo = true;
                m_DemoThread = new Thread(PlayDemo);
                m_DemoThread.Start();
            }
            else
            {
                buttonPlayDemo.Content = "Start autoplay";
                m_PlayingDemo = false;
                m_DemoThread = null;
            }
        }

        public void PlayDemo()
        {
            while (m_PlayingDemo)
            {
                try
                {
                    computer.ComputeAndMakeBestMove(null);
                }
                catch (ApplicationException e)
                {
                    if (e.Message == "Checkmate")
                    {
                        m_PlayingDemo = false;
                        MessageBox.Show("Checkmate");
                        return;
                    }
                    if (e.Message == "Stalemate")
                    {
                        m_PlayingDemo = false;
                        MessageBox.Show("Stalemate");
                        return;
                    }
                    else
                        throw;
                }
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action<Board>(UpdateUi), computer.Board);
                Thread.Sleep(50);
            }
        }

        private void RenderBoard(Board board)
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    Piece piece;

                    if (m_Flipped)
                        piece = board.S[63 - (i * 8 + j)];
                    else
                        piece = board.S[i * 8 + j];

                    Colour squareColour = i % 2 == j % 2 ? Colour.White : Colour.Black;

                    if (piece == null)
                        pieceBoxes[j, i].Source = PieceImages.For(Colour.Blank, squareColour, PieceType.Blank);
                    else
                        pieceBoxes[j, i].Source = PieceImages.For(piece.Colour, squareColour, piece.PieceType);
                }

            boardCanvas.UpdateLayout();
            this.UpdateLayout();
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            computer.CalculateAndMakeMoveAsync(RenderBoardAsync);
        }

        private void buttonFlip_Click(object sender, RoutedEventArgs e)
        {
            m_Flipped = !m_Flipped;
            UpdateUi(computer.Board);
        }

        private void buttonScoreBoard_Click(object sender, RoutedEventArgs e)
        {
            labelScoreBoard.Content = String.Format("{0:0.00}", computer.Board.ScoreBoard());
        }

        private void SliderDepth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(computer != null)
                computer.Difficulty = (int)e.NewValue;
        }

        private void buttonTakeBack_Click(object sender, RoutedEventArgs e)
        {
            computer.TakeBack();
            UpdateUi(computer.Board);
        }
    }
}
