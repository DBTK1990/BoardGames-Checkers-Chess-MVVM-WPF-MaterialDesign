using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Chess0.Helper;


namespace Chess0.Model.Peices
{
    

    class Piece_Queen_M : Compass, IPieceModel 
    {
        private string Path {  get; set; }

        private MyPoint pos;
        public MyPoint Pos
        {
            get
            {
                return pos;
            }

            set
            {

                pos = value;
            }
        }

        private State player;
        public State Player
        {
            get
            {
                return player;
            }
            set
            {
                player = value;
            }
        }
        public int MovesMade { get; set; }

        public BitmapImage ImagePiece { get; set; }

        public Piece_Queen_M(MyPoint pos,State player)
        {

            switch (player)
            {
                case State.White:
                    Path = @"pack://application:,,,/Chess0;component/Resources/Queen_White.png";
                    break;
                case State.Black:
                    Path = @"pack://application:,,,/Chess0;component/Resources/Queen_Black.png";
                    break;
                default:
                    break;
            }
           
            

            ImagePiece = new BitmapImage();
            ImagePiece.BeginInit();
            ImagePiece.UriSource = new Uri(Path);
            ImagePiece.EndInit();

            MovesMade = 0;
            Player = player;
            Pos = pos;

        }

    


        public List<MyPoint> PossiablePath()
        {
            List<MyPoint> paths = new List<MyPoint>();

            foreach (MyPoint direction in Direction)
            {
                for (var moves = 1; moves <= 7; moves++)
                {
                    MyPoint Check = (direction * moves) + Pos;

                    if ((0<=Check.X && Check.X<8) && (0 <= Check.Y && Check.Y < 8))
                    paths.Add(direction);

                }

            }
            
            return paths;
        }
    }

    class Piece_King_M : Compass, IPieceModel
    {
        private string Path { get; set; }

        private MyPoint pos;
        public MyPoint Pos
        {
            get
            {
                return pos;
            }

            set
            {
  
                pos = value;
            }
        }

        private State player;
        public State Player
        {
            get
            {
                return player;
            }
            set
            {
                player = value;
            }
        }

        public int MovesMade { get; set; }

        public BitmapImage ImagePiece { get; set; }

        public Piece_King_M(MyPoint pos, State player)
        {

            switch (player)
            {
                case State.White:
                    Path = @"pack://application:,,,/Chess0;component/Resources/King_White.png";
                    break;
                case State.Black:
                    Path = @"pack://application:,,,/Chess0;component/Resources/King_Black.png";
                    break;
                default:
                    break;
            }



            ImagePiece = new BitmapImage();
            ImagePiece.BeginInit();
            ImagePiece.UriSource = new Uri(Path);
            ImagePiece.EndInit();

            MovesMade = 0;
            Player = player;
            Pos = pos;

        }




        public List<MyPoint> PossiablePath()
        {
            List<MyPoint> paths = new List<MyPoint>();

                foreach (MyPoint direction in Direction)
                {
                    MyPoint Check = (direction) + Pos;

                    if ((0 <= Check.X && Check.X < 8) && (0 <= Check.Y && Check.Y < 8))
                        paths.Add(direction);

                }

            

            return paths;
        }


    }

    class Piece_Rook_M: Compass,IPieceModel
    {
        public string Path { get; set; }

        private MyPoint pos;
        public MyPoint Pos
        {
            get
            {
                return pos;
            }

            set
            {

                pos = value;
            }
        }
        private State player;
        public State Player
        {
            get
            {
                return player;
            }
            set
            {
                player = value;
            }
        }
        public int MovesMade { get; set; }

        public BitmapImage ImagePiece { get; set; }

        public Piece_Rook_M(MyPoint pos, State player)
        {

            switch (player)
            {
                case State.White:
                    Path = @"pack://application:,,,/Chess0;component/Resources/Rook_White.png";
                    break;
                case State.Black:
                    Path = @"pack://application:,,,/Chess0;component/Resources/Rook_Black.png";
                    break;
                default:
                    break;
            }



            ImagePiece = new BitmapImage();
            ImagePiece.BeginInit();
            ImagePiece.UriSource = new Uri(Path);
            ImagePiece.EndInit();

            MovesMade = 0;
            Player = player;
            Pos = pos;

        }

        public List<MyPoint> PossiablePath()
        {
            List<MyPoint> paths = new List<MyPoint>();
            for (var diraction = 6; diraction >= 0; diraction -= 2)
            {
            
                for (var moves = 1; moves<=7; moves++)
                {
                    MyPoint Check = (Direction[diraction] * moves) + Pos;

                    if ((0 <= Check.X && Check.X < 8) && (0 <= Check.Y && Check.Y < 8))
                        paths.Add(Direction[diraction]);
                }

            }

            return paths;
        }
    }

    class Piece_Bishop_M : Compass, IPieceModel
    {
        public string Path { get; set; }

        private MyPoint pos;
        public MyPoint Pos
        {
            get
            {
                return pos;
            }

            set
            {

                pos = value;
            }
        }

        private State player;
        public State Player
        {
            get
            {
                return player;
            }
            set
            {
                player = value;
            }
        }

        public int MovesMade { get; set; }
        public BitmapImage ImagePiece { get; set; }

        public Piece_Bishop_M(MyPoint pos, State player)
        {
            switch (player)
            {
                case State.White:
                    Path = @"pack://application:,,,/Chess0;component/Resources/Bishop_White.png";
                    break;
                case State.Black:
                    Path = @"pack://application:,,,/Chess0;component/Resources/Bishop_Black.png";
                    break;
                default:
                    break;
            }



            ImagePiece = new BitmapImage();
            ImagePiece.BeginInit();
            ImagePiece.UriSource = new Uri(Path);
            ImagePiece.EndInit();

            MovesMade = 0;
            Player = player;
            Pos = pos;

        }

        public List<MyPoint> PossiablePath()
        {
            List<MyPoint> paths = new List<MyPoint>();
            for (var diraction = 7; diraction >= 1; diraction -= 2)
            {
                for (var moves = 1; moves <= 7; moves++)
                {

                    MyPoint Check = (Direction[diraction] * moves) + Pos;

                    if ((0 <= Check.X && Check.X < 8) && (0 <= Check.Y && Check.Y < 8))
                        paths.Add(Direction[diraction]);
                }

            }

            return paths;
        }
    }

    class Piece_Knight_M : Compass, IPieceModel
    {
        public string Path { get; set; }

        private MyPoint pos;
        public MyPoint Pos
        {
            get
            {
                return pos;
            }

            set
            {

                pos = value;
            }
        }


        private State player;
        public State Player
        {
            get
            {
                return player;
            }
            set
            {
                player = value;
            }
        }

        public int MovesMade { get; set; }

        public BitmapImage ImagePiece { get; set; }

        public Piece_Knight_M(MyPoint pos, State player)
        {

            switch (player)
            {
                case State.White:
                    Path = @"pack://application:,,,/Chess0;component/Resources/Knight_White.png";
                    break;
                case State.Black:
                    Path = @"pack://application:,,,/Chess0;component/Resources/Knight_Black.png";
                    break;
                default:
                    break;
            }
            ImagePiece = new BitmapImage();
            ImagePiece.BeginInit();
            ImagePiece.UriSource = new Uri(Path);
            ImagePiece.EndInit();


            MovesMade = 0;
            Player = player;
            Pos = pos;

        }

        public List<MyPoint> PossiablePath()
        {
            List<MyPoint> paths = new List<MyPoint>();
            MyPoint[] temp = { new MyPoint(1, 2), new MyPoint(2, 1)};

            for (var directionIndex = 7; directionIndex >= 1; directionIndex -= 2)
            {
                MyPoint Check = (Direction[directionIndex]*temp[0]) + Pos;

                if ((0 <= Check.X && Check.X < 8) && (0 <= Check.Y && Check.Y < 8))
                    paths.Add(Direction[directionIndex] * temp[0]);

                Check = (Direction[directionIndex] * temp[1]) + Pos;
                if ((0 <= Check.X && Check.X < 8) && (0 <= Check.Y && Check.Y < 8))
                    paths.Add(Direction[directionIndex] * temp[1]);
            }

            return paths;
        }
    }
    
   class Piece_Pawn_M : Compass, IPieceModel
    {
        public string Path { get; set; }

        private MyPoint pos;
        public MyPoint Pos
        {
            get
            {
                return pos;
            }

            set
            {
                pos = value;
            }
        }
        private State player;
        public State Player
        {
            get
            {
                return player;
            }
            set
            {
                player = value;
            }
        } 

        public int MovesMade { get; set; }
        public BitmapImage ImagePiece { get; set; }

        public Piece_Pawn_M(MyPoint pos, State player)
        {

            switch (player)
            {
                case State.White:
                    Path = @"pack://application:,,,/Chess0;component/Resources/Pawn_White.png";
                    break;
                case State.Black:
                    Path = @"pack://application:,,,/Chess0;component/Resources/Pawn_Black.png";
                    break;
                default:
                    break;
            }



            ImagePiece = new BitmapImage();
            ImagePiece.BeginInit();
            ImagePiece.UriSource = new Uri(Path);
            ImagePiece.EndInit();

            MovesMade = 0;///dont forget to update thissssss with commit or somthing
            Player = player;
            Pos = pos;

        }
        
        public List<MyPoint> PossiablePath()
        {
         
            List<MyPoint> paths = new List<MyPoint>();
            MyPoint Check = null;

            switch (Player)
            { 
                case State.White:

                int directionWhiteIndex = 7, next = 6;

                while (directionWhiteIndex >= 0)
                {
                        Check = Direction[directionWhiteIndex]+Pos;
                        if ((0 <= Check.X && Check.X < 8) && (0 <= Check.Y && Check.Y < 8))
                            paths.Add(Direction[directionWhiteIndex]);

                    directionWhiteIndex -= next;
                    next /= next;
                }
                    if (MovesMade == 0)
                        paths.Add((Direction[(int)CompassE.North]*2));

                    break;
                case State.Black:
                    for (var directionBlackIndex = 3; directionBlackIndex <= 5; directionBlackIndex++)
                    {
                        Check = Direction[directionBlackIndex] + Pos;
                        if ((0 <= Check.X && Check.X < 8) && (0 <= Check.Y && Check.Y < 8))
                            paths.Add(Direction[directionBlackIndex]);

                    }
                    if (MovesMade == 0)
                        paths.Add((Direction[(int)CompassE.South] * 2) );


                    break;
            }

            
            return paths;
        }
    }

}
