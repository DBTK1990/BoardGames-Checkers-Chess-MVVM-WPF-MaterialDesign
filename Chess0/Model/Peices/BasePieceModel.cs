using System;
using System.Collections.Generic;

using System.Windows.Media.Imaging;
using Chess0.Helper;
using Chess0.ViewModel;

namespace Chess0.Model.Peices
{
    abstract class BasePieceModel : Compass, IPiece
    {

        private MyPoint pos;
        public MyPoint Pos
        {
            get => pos;
            set => pos = value;
        }

        private State player;
        public State Player
        {
            get => player;
            set => player = value;

        }
        private int movesmade;
        public int MovesMade
        {
            get => movesmade;
            set => movesmade = value;
        }

        private BitmapImage imagepiece;
        public BitmapImage ImagePiece
        {
            get => imagepiece;
            set => imagepiece = value;
        }
        public static bool operator==(BasePieceModel a, BasePieceModel b)
        {

            if (a is null)
                return b is null;
            else if (b is null)
                return a is null;



            if (a.Pos == b.Pos && a.Player == b.Player && a.MovesMade == b.movesmade && a.ImagePiece.UriSource == b.ImagePiece.UriSource)
                return true;


            return false;
        }
        public static bool operator !=(BasePieceModel a, BasePieceModel b)
        {
            return !(a==b);
        }

        protected void initProprties(MyPoint pos, State player, string path)
        {
            ImagePiece = new BitmapImage();
            ImagePiece.BeginInit();
            ImagePiece.UriSource = new Uri(path);
            ImagePiece.EndInit();

            MovesMade = 0;
            Player = player;
            Pos = pos;
        }

        public abstract List<MyPoint> PossiablePath(MyPoint TargetPos = null);

      
    }
}
