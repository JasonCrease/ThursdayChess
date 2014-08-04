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
    class PieceImages
    {
        static string imgDir;

        static PieceImages()
        {
            imgDir = Directory.GetCurrentDirectory() + ".\\images\\";
            if (!Directory.Exists(imgDir))
                imgDir = Directory.GetCurrentDirectory() + ".\\..\\..\\images\\";

            bbp = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bbp.png")));
            bbn = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bbn.png")));
            bbb = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bbb.png")));
            bbr = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bbr.png")));
            bbk = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bbk.png")));
            bbq = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bbq.png")));

            bwp = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bwp.png")));
            bwn = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bwn.png")));
            bwb = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bwb.png")));
            bwr = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bwr.png")));
            bwk = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bwk.png")));
            bwq = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bwq.png")));

            wbp = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wbp.png")));
            wbn = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wbn.png")));
            wbb = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wbb.png")));
            wbr = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wbr.png")));
            wbk = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wbk.png")));
            wbq = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wbq.png")));

            wwp = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wwp.png")));
            wwn = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wwn.png")));
            wwb = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wwb.png")));
            wwr = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wwr.png")));
            wwk = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wwk.png")));
            wwq = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wwq.png")));

            bsq = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "bsq.png")));
            wsq = new BitmapImage(new Uri(System.IO.Path.Combine(imgDir, "wsq.png")));

        }

        public static BitmapImage bbp, bbn, bbb, bbr, bbk, bbq, bwp, bwn, bwb, bwr, bwk, bwq, wbp, wbn, wbb, wbr, wbk, wbq, wwp, wwn, wwb, wwr, wwk, wwq, bsq, wsq;

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
}
