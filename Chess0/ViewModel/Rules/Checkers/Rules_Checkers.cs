using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Chess0.Helper;
using Chess0.Model;
using Chess0.Model.Peices;

namespace Chess0.ViewModel.Rules.Checkers
{
    class Rules_Checkers : BaseRules_Checkers, IRules
    {


        //this is fine
        public void InitPieces(ObservableBoardCollection<TileModel> Tiles)
        {
            CheckersPlayer white = new CheckersPlayer(State.White);
            CheckersPlayer black = new CheckersPlayer(State.Black);

            for (var i = 0; i < white.Pieces.Count(); i++)
            {
                Tiles[white.Pieces[i].Pos].Piece = white.Pieces[i];
                Tiles[black.Pieces[i].Pos].Piece = black.Pieces[i];

            }
        }

        public void testPieces(ObservableBoardCollection<TileModel> Tiles)
        {

            CheckersPlayer test = new CheckersPlayer();

            foreach (IPiece newPiece in test.Pieces)
            {
                Tiles[newPiece.Pos].Piece = newPiece;
          
            }
        }

       

        public State PlayerTurnSwitch(TileModel focus, ObservableBoardCollection<TileModel> tiles)
        {

            State PlayerTurn;

            if (Lock1 == null)
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

                foreach (TileModel CanEat in tiles)
                {
                   

                    if (CanEat.Piece != null && CanEat.Piece.Player == PlayerTurn)
                    {
                        SimulatePath(CanEat, tiles);

                        foreach (TileModel tile in tiles)
                        {
                            tile.MarkVisibility = "Hidden";
                            tile.MarkColor = "White";
                        }

                        if (isCapturedPiecesEmpty && Lock2.Count() == 0)
                        {
                            Lock2.Clear();   
                        }
                        else if(!isCapturedPiecesEmpty)
                        {
                            Lock2.Add(CanEat.Pos);
                            

                        }
                        capturedpieces.Clear();
                    }

                }

                if (Lock2.Count() !=0)
                    SimulatePath(tiles[Lock2.First()], tiles);
               

            }
            else
            {
                focus = tiles[Lock1];
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
                            if (Me.Piece is Piece_Man_M && moves[Check] == 1 && MyPoint.CheckBound(Pos,7))
                                capturedpieces.Add(Check, Pos);
                            else if (Me.Piece is Piece_FlyingKingC_M && MyPoint.CheckBound(Pos,7))
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


            if (Tiles[moveTo].Piece is Piece_FlyingKingC_M && Tiles[moveTo].Piece.MovesMade != 0)
            {
                SimulatePath(Tiles[moveTo], Tiles);
            }
            else if (Tiles[moveTo].Piece is Piece_Man_M)
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
                    Lock1 = null;
                    capturedpieces.Clear();
            }
                else
                {
                    Lock1 = moveTo;
                }

            if (Lock2.Count != 0)
            {
                Lock2.Clear();


            }

        

        }

        public void MovePiece( MyPoint point, MyPoint moveTo, ObservableBoardCollection<TileModel> Tiles)
        {
            if ((moveTo.X==0 || moveTo.X==7) && (Tiles[point].Piece is Piece_Man_M))
            {
                Tiles[moveTo].Piece = new Piece_FlyingKingC_M(moveTo, Tiles[point].Piece.Player);
            }
            else
            {
                Tiles[moveTo].Piece = Tiles[point].Piece;
                Tiles[point].Piece.Pos = moveTo;
                Tiles[moveTo].Piece.MovesMade++;
            }
            Tiles[point].Piece = null;

           


            capturedpieces.Clear();
        }
    }


}
