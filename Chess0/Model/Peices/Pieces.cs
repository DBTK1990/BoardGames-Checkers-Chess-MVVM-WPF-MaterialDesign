using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Chess0.Helper;
using Chess0.ViewModel;

namespace Chess0.Model.Peices
{

    #region Chess Pieces
    class Piece_Queen_M : BasePieceModel 
    {
        public Piece_Queen_M(MyPoint pos,State player)
        {
            string TempPath="";
            switch (player)
            {
                case State.White:
                    TempPath = @"pack://application:,,,/Chess0;component/Resources/Queen_White.png";
                    break;
                case State.Black:
                    TempPath = @"pack://application:,,,/Chess0;component/Resources/Queen_Black.png";
                    break;
                default:
                    break;
            }

            base.initProprties(pos, player, TempPath);

        
        }

        public override List<MyPoint> PossiablePath()
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

    class Piece_King_M : BasePieceModel
    {
       
        public Piece_King_M(MyPoint pos, State player)
        {
            string TempPath = "";
            switch (player)
            {
                case State.White:
                    TempPath = @"pack://application:,,,/Chess0;component/Resources/King_White.png";
                    break;
                case State.Black:
                    TempPath = @"pack://application:,,,/Chess0;component/Resources/King_Black.png";
                    break;
                default:
                    break;
            }


            base.initProprties(pos, player, TempPath);

        }




        public override List<MyPoint> PossiablePath()
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

    class Piece_Rook_M: BasePieceModel
    {
        public Piece_Rook_M(MyPoint pos, State player)
        {

            string TempPath = "";

            switch (player)
            {
                case State.White:
                    TempPath = @"pack://application:,,,/Chess0;component/Resources/Rook_White.png";
                    break;
                case State.Black:
                    TempPath = @"pack://application:,,,/Chess0;component/Resources/Rook_Black.png";
                    break;
                default:
                    break;
            }

            base.initProprties(pos, player, TempPath);


        }

        public override List<MyPoint> PossiablePath()
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

    class Piece_Bishop_M : BasePieceModel
    {
     
        public Piece_Bishop_M(MyPoint pos, State player)
        {
            string TempPath = "";
            switch (player)
            {
                case State.White:
                    TempPath = @"pack://application:,,,/Chess0;component/Resources/Bishop_White.png";
                    break;
                case State.Black:
                    TempPath = @"pack://application:,,,/Chess0;component/Resources/Bishop_Black.png";
                    break;
                default:
                    break;
            }

            base.initProprties(pos, player, TempPath);

        }

        public override List<MyPoint> PossiablePath()
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

    class Piece_Knight_M : BasePieceModel
    {
     
        public Piece_Knight_M(MyPoint pos, State player)
        {
            string TempPath = "";
            switch (player)
            {
                case State.White:
                    TempPath = @"pack://application:,,,/Chess0;component/Resources/Knight_White.png";
                    break;
                case State.Black:
                    TempPath = @"pack://application:,,,/Chess0;component/Resources/Knight_Black.png";
                    break;
                default:
                    break;
            }

            base.initProprties(pos, player, TempPath);

        }

        public override List<MyPoint> PossiablePath()
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
    
    class Piece_Pawn_M : BasePieceModel
    {

        public Piece_Pawn_M(MyPoint Pos, State Player)
        {

            string TempPath = "";
          
            switch (Player)
            {
                case State.White:
                    TempPath = @"pack://application:,,,/Chess0;component/Resources/Pawn_White.png";
                    break;
                case State.Black:
                    TempPath = @"pack://application:,,,/Chess0;component/Resources/Pawn_Black.png";
                    break;
                default:
                    break;
            }

            base.initProprties(Pos, Player, TempPath);

        }
        
        public override List<MyPoint> PossiablePath()
        {
         
            List<MyPoint> paths = new List<MyPoint>();
            MyPoint Check = null;

            switch (Player)
            { 
                case State.White:

                    int directionWhiteIndex = 7, next = 6;

                    while (directionWhiteIndex >= 0)
                    {
                        Check = Direction[directionWhiteIndex] + Pos;
                        if ((0 <= Check.X && Check.X < 8) && (0 <= Check.Y && Check.Y < 8))
                        {
                            if (MovesMade == 0 && directionWhiteIndex == 0)
                            {
                                paths.Add(Direction[directionWhiteIndex]);
                                paths.Add(Direction[directionWhiteIndex]);
                            }
                            else
                                paths.Add(Direction[directionWhiteIndex]);
                        }

                        directionWhiteIndex -= next;
                        next /= next;
                    }
                    break;
                case State.Black:
                    for (var directionBlackIndex = 3; directionBlackIndex <= 5; directionBlackIndex++)
                    {
                        Check = Direction[directionBlackIndex] + Pos;
                        if ((0 <= Check.X && Check.X < 8) && (0 <= Check.Y && Check.Y < 8))
                        {
                            if (MovesMade == 0 && directionBlackIndex == 4)
                            {
                                paths.Add((Direction[directionBlackIndex]));
                                paths.Add((Direction[directionBlackIndex]));
                            }
                            else
                                paths.Add(Direction[directionBlackIndex]);
                        }
                    }
                    break;
            }

            
            return paths;
        }
    }

    #endregion Chess Pieces

    #region Checkers Pieces


    class Piece_Man_M : BasePieceModel
    {
      
        public Piece_Man_M(MyPoint pos, State player)
        {
            string TempPath = "";

            switch (player)
            {
                case State.White:
                    TempPath = @"pack://application:,,,/Chess0;component/Resources/Man_White.png";
                    break;
                case State.Black:
                    TempPath = @"pack://application:,,,/Chess0;component/Resources/Man_Black.png";
                    break;
                default:
                    break;
            }


            base.initProprties(Pos, Player, TempPath);

        }

        public override List<MyPoint> PossiablePath()
        {

            List<MyPoint> paths = new List<MyPoint>();
            MyPoint Check = null;

            switch (Player)
            {
                case State.White:

                    int directionWhiteIndex = 7;

                    while (directionWhiteIndex >= 1)
                    {
                        Check = Direction[directionWhiteIndex] + Pos;

                        if ((0 <= Check.X && Check.X < 8) && (0 <= Check.Y && Check.Y < 8))
                        {
                            paths.Add(Direction[directionWhiteIndex]);
                            paths.Add(Direction[directionWhiteIndex]);
                        }
                        

                        directionWhiteIndex -= 6;
                        
                    }
                    break;
                case State.Black:
                    for (var directionBlackIndex = 3; directionBlackIndex <= 5; directionBlackIndex+=2)
                    {
                        Check = Direction[directionBlackIndex] + Pos;
                        if ((0 <= Check.X && Check.X < 8) && (0 <= Check.Y && Check.Y < 8))
                        {
                                paths.Add((Direction[directionBlackIndex]));
                                paths.Add((Direction[directionBlackIndex]));
                        }
                    }
                    break;
            }


            return paths;
        }
    }

    class Piece_FlyingKingC_M : BasePieceModel
    {

        public Piece_FlyingKingC_M(MyPoint pos, State player)
        {
            string TempPath = "";

            switch (player)
            {
                case State.White:
                    TempPath = @"pack://application:,,,/Chess0;component/Resources/FlyKing_White.png";
                    break;
                case State.Black:
                    TempPath = @"pack://application:,,,/Chess0;component/Resources/FlyKing_Black.png";
                    break;
                default:
                    break;
            }

            base.initProprties(Pos, Player, TempPath);

        }

        public override List<MyPoint> PossiablePath()
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
 
    #endregion Checkers Pieces
}



