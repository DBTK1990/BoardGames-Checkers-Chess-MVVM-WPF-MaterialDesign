using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Chess0.Helper;
using Chess0.Model;
using Chess0.Model.Peices;

namespace Chess0.ViewModel.Rules.Checkers
{
    class Rules_Checkers :  IRules
    {

        protected Dictionary<MyPoint, MyPoint> capturedpieces = new Dictionary<MyPoint, MyPoint>();

        private MyPoint _lock = null;
        public MyPoint Lock
        {
            get => _lock;
            protected set => _lock = value;
        }
        public bool isCapturedPiecesEmpty => (capturedpieces.Count() == 0) ? true : false;

        //this is fine
        public void InitPieces(ObservableBoardCollection<TileModel> Tiles)
        {
            CheckersPlayer white = new CheckersPlayer(State.White);
            CheckersPlayer black = new CheckersPlayer(State.Black);


           
            for (var i = 0; i < 12; i++)
            {
                Tiles[white.Pieces[i].Pos].Piece = white.Pieces[i];
                Tiles[black.Pieces[i].Pos].Piece = black.Pieces[i];

            }
        }

       

        public State PlayerTurnSwitch(TileModel focus, ObservableBoardCollection<TileModel> tiles)
        {



            //check if focus piece can eat another piece
            //if true then
            //:
            //lock=focus.pos 
            //SimulatePath
            //:
            //if false then
            //:
            //change player
            //focus=null
            //:
            State PlayerTurn;

            if (Lock == null)
            {
                PlayerTurn = focus.Piece.Player;

                switch (PlayerTurn)
                {
                    case State.Black:
                        PlayerTurn = State.White;
                        break;
                    case State.White:
                        PlayerTurn = State.Black;
                        break;
                }
                focus = null;
            }
            else
            {
                focus = tiles[Lock];
                PlayerTurn = focus.Piece.Player;
                
            }
            return PlayerTurn;
        }

        public void SimulatePath(TileModel Me, ObservableBoardCollection<TileModel> Tiles)
        {
            HashSet<MyPoint> BlockedPath = new HashSet<MyPoint>();
            List<MyPoint> PossiablePath = Me.Piece.PossiablePath();
            HashSet<MyPoint> MoveList = new HashSet<MyPoint>();
            MyPoint Pos = Me.Pos;
            Dictionary<MyPoint, int> moves = new Dictionary<MyPoint, int>();
           
           
     
            capturedpieces.Clear();
            foreach (MyPoint dir in PossiablePath)
            {
                try
                {
                    moves.Add(dir, 0);
                    System.Console.WriteLine("SimulatePath() method: dir not exist");
                }
                catch (ArgumentException e)
                {
                    System.Console.WriteLine("SimulatePath() method: dir exist");

                }
            }


            for (var CheckIndex = 0; CheckIndex < PossiablePath.Count; CheckIndex++)
            {
                MyPoint Check = PossiablePath[CheckIndex];
                    moves[Check]++;


                if (!BlockedPath.Contains(Check))
                {
                    Pos=Pos+ (Check*moves[Check]);
                    if (Tiles[Pos].Piece == null)
                    {
                        if (Me.Piece is Piece_Man_M)
                        {
                            if (moves[Check] == 1 && !capturedpieces.ContainsKey(Check))
                            {
                                MoveList.Add(Pos);
                                BlockedPath.Add(Check);
                            }
                            else if (moves[Check] == 2 && !isCapturedPiecesEmpty)
                                MoveList.Add(Pos);
                        }
                        else if (Me.Piece is Piece_FlyingKingC_M)
                            MoveList.Add(Pos);



                    }
                    else
                    {
                        if (Tiles[Pos].Piece.Player == Me.Piece.Player)
                        {
                   
                            
                                BlockedPath.Add(Check);
                            
                        }
                        else if (Tiles[Pos].Piece.Player != Me.Piece.Player )
                        {
                            if (Me.Piece is Piece_Man_M && moves[Check] == 1 && CheckBound(Pos))
                                capturedpieces.Add(Check, Pos);
                            else if (Me.Piece is Piece_FlyingKingC_M && CheckBound(Pos))
                            {
                                try { capturedpieces.Add(Check, Pos); }
                                catch (ArgumentException e)
                                {
                                    BlockedPath.Add(Check);
                                }
                            }
                            
                          


                        }

                        MyPoint tempCapture = null;
                        capturedpieces.TryGetValue(Check, out tempCapture);
                        if (tempCapture != null)
                        {
                            if (Math.Floor(MyPoint.getDistence(tempCapture, Pos)) == 1)
                            {
                                capturedpieces.Remove(Check);
                                BlockedPath.Add(Check);
                            }
                        }
                    }
                }



                Pos = Me.Pos;

            }

            if (isCapturedPiecesEmpty)
            {
                foreach (MyPoint move in MoveList)
                {

                    Tiles[move].MarkColor = "Green";
                    Tiles[move].MarkVisibility = "Visible";

                }

            }
            else
            {
                foreach(KeyValuePair<MyPoint,MyPoint> Capture in capturedpieces)
                {
              
                    Tiles[Capture.Value].MarkColor = "Red";
                    Tiles[Capture.Value].MarkVisibility = "Visible";
                    MyPoint pathGreen = Capture.Value + Capture.Key;

                    foreach (MyPoint move in MoveList)
                    {
                        if (pathGreen == move)
                        {
                            Tiles[pathGreen].MarkColor = "Green";
                            Tiles[pathGreen].MarkVisibility = "Visible";
                            pathGreen = pathGreen + Capture.Key;
                        }

                    }
                    


                }
            }
            

        }

        public bool WinCondition(object ob)
        {
            ObservableCollection<IPiece> DeadPieces = (ObservableCollection<IPiece>)ob;
            return (DeadPieces.Count()==12)?true:false;
        }

        

        private bool CheckBound(MyPoint boundCheck)
        {
            return boundCheck.X != 0 && boundCheck.Y != 0 && boundCheck.X != 7 && boundCheck.Y != 7?true:false;
           
        }


        public void EatPiece(MyPoint point, MyPoint moveTo, ObservableBoardCollection<TileModel> Tiles, ObservableCollection<IPiece> DeadBlack, ObservableCollection<IPiece> DeadWhite)
        {


            MyPoint direction =  (moveTo - point) /Math.Floor( MyPoint.getDistence(point, moveTo) );
        

            foreach (KeyValuePair<MyPoint, MyPoint> CanIeat in capturedpieces)
            {
                if (CanIeat.Key == direction)
                {
                    switch (Tiles[CanIeat.Value].Piece.Player)
                    {
                        case State.Black:
                            DeadBlack.Add(Tiles[CanIeat.Value].Piece);
                            break;
                        case State.White:
                            DeadWhite.Add(Tiles[CanIeat.Value].Piece);
                            break;
                    }

                    Tiles[CanIeat.Value].Piece = null;
                }

            }


            MovePiece(point, moveTo, Tiles);
            //setlock to postison if anohter eat possiable

            if (moveTo.X != 0 && moveTo.X != 7)
            {
                SimulatePath(Tiles[moveTo], Tiles);
            }

            if (isCapturedPiecesEmpty)
            {
                foreach (TileModel tile in Tiles)
                {
                    tile.MarkVisibility = "Hidden";
                    tile.MarkColor = "White";
                }
                    Lock = null;
                    capturedpieces.Clear();
            }
                else
                {
                    Lock = moveTo;
                }
           
           

        

        }

        public void MovePiece( MyPoint point, MyPoint moveTo, ObservableBoardCollection<TileModel> Tiles)
        {
            if (moveTo.X==0 || moveTo.X==7)
            {
                Tiles[moveTo].Piece = new Piece_FlyingKingC_M(moveTo, Tiles[point].Piece.Player);
            }
            else
            {
                Tiles[moveTo].Piece = Tiles[point].Piece;
                Tiles[point].Piece.Pos = moveTo;
            }
            Tiles[point].Piece = null;

            Tiles[moveTo].Piece.MovesMade++;

            capturedpieces.Clear();
        }
    }


}
