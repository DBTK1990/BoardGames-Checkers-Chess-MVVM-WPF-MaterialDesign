using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Chess0.Helper;
using Chess0.Model;
using Chess0.Model.Peices;
using Chess0.ViewModel.AI_Player;

namespace Chess0.ViewModel.Rules.Checkers
{
    class Rules_Checkers : BaseRules_Checkers
    {


        //this is fine
        public override void InitPieces(ObservableBoardCollection<TileModel> Tiles)
        {
            CheckersPlayer white = new CheckersPlayer(State.White);
            CheckersPlayer black = new CheckersPlayer(State.Black);

            for (var i = 0; i < white.Pieces.Count(); i++)
            {
                Tiles[white.Pieces[i].Pos].Piece = white.Pieces[i];
                Tiles[black.Pieces[i].Pos].Piece = black.Pieces[i];

            }
        }

        public void TestPieces(ObservableBoardCollection<TileModel> Tiles)
        {

            CheckersPlayer test = new CheckersPlayer();

            foreach (IPiece newPiece in test.Pieces)
            {
                Tiles[newPiece.Pos].Piece = newPiece;

            }
        }



        public override State PlayerTurnSwitch(TileModel focus, ObservableBoardCollection<TileModel> tiles)
        {


            foreach (TileModel tile in tiles)
            {
                tile.MarkVisibility = "Hidden";
            }


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


                Restriction2_IsAnyPieceHasToEatEnemy(tiles, PlayerTurn);


                if (!IsLock2Empty())
                    SimulatePath(tiles[Lock2.First()], tiles);



            }
            else
            {
                focus = tiles[Lock1];
                PlayerTurn = focus.Piece.Player;

                SimulatePath(focus, tiles);


            }


            return PlayerTurn;
        }


        private PathData ViablePath(TileModel Me, ObservableBoardCollection<TileModel> Tiles)
        {
            HashSet<MyPoint> BlockedPath = new HashSet<MyPoint>();
            List<MyPoint> PossiablePath = Me.Piece.PossiablePath(); 
            MyPoint Pos = Me.Pos;
            Dictionary<MyPoint, int> moves = new Dictionary<MyPoint, int>();
            Dictionary<MyPoint, MyPoint> capturedpieces = new Dictionary<MyPoint, MyPoint>();
            HashSet<MyPoint> MoveList = new HashSet<MyPoint>();


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
                    Pos = Pos + (Check * moves[Check]);
                    if (Tiles[Pos].Piece == null)
                    {
                        if (Me.Piece is Piece_Man_M)
                        {
                            if (moves[Check] == 1 && !capturedpieces.ContainsKey(Check))
                            {
                                MoveList.Add(Pos);
                                BlockedPath.Add(Check);
                            }
                            else if (moves[Check] == 2 && capturedpieces.Count()!=0)
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
                        else if (Tiles[Pos].Piece.Player != Me.Piece.Player)
                        {
                            if (Me.Piece is Piece_Man_M && moves[Check] == 1 && MyPoint.CheckBound(Pos, 7))
                                capturedpieces.Add(Check, Pos);
                            else if (Me.Piece is Piece_FlyingKingC_M && MyPoint.CheckBound(Pos, 7))
                            {
                                try { capturedpieces.Add(Check, Pos); }
                                catch (ArgumentException e)
                                {
                                    BlockedPath.Add(Check);
                                }
                            }




                        }


                        capturedpieces.TryGetValue(Check, out MyPoint tempCapture);
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


            if (capturedpieces.Count() == 0)
                return new PathData(Tiles, MoveList);

            return new PathData(Tiles, MoveList, capturedpieces);
        }

        private void MarkForUser(PathData viablepath, ObservableBoardCollection<TileModel> Tiles) 
        {
            foreach (TileModel tile in Tiles)
            {
                tile.MarkVisibility = "Hidden"; 
                tile.MarkColor = "White";
            }

            if (viablepath.capturedPieces == null)
            {
                foreach (MyPoint move in viablepath.Moves)
                {

                    Tiles[move].MarkColor = "Green";
                    Tiles[move].MarkVisibility = "Visible";

                }

            }
            else
            {
                foreach (KeyValuePair<MyPoint, MyPoint> Capture in viablepath.capturedPieces.First())
                {

                    Tiles[Capture.Value].MarkColor = "Red";
                    Tiles[Capture.Value].MarkVisibility = "Visible";
                    MyPoint pathGreen = Capture.Value + Capture.Key;

                    foreach (MyPoint move in viablepath.Moves)
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

        public override void SimulatePath(TileModel Me, ObservableBoardCollection<TileModel> Tiles)
        {

            PathData viablepath = ViablePath(Me, Tiles);
            MarkForUser(viablepath, Tiles);
            //function MarkForUser



        }




        public override bool WinCondition(object ob)
        {
            ObservableCollection<IPiece> DeadPieces = (ObservableCollection<IPiece>)ob;
            return (DeadPieces.Count() == 12) ? true : false;
        }






        public override void EatPiece(MyPoint point, MyPoint moveTo, ObservableBoardCollection<TileModel> Tiles, ObservableCollection<IPiece> DeadBlack, ObservableCollection<IPiece> DeadWhite)
        {


            MyPoint direction = (moveTo - point) / Math.Floor(MyPoint.getDistence(point, moveTo));

            PathData viablepath= ViablePath(Tiles[point], Tiles);
            foreach (KeyValuePair<MyPoint, MyPoint> CanIeat in viablepath.capturedPieces.First())
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

            Restriction1_IsThisPieceCanEatAnotherEnemy(Tiles, moveTo);
         
            
        }

        public override void MovePiece(MyPoint point, MyPoint moveTo, ObservableBoardCollection<TileModel> Tiles)
        {
            if ((moveTo.X == 0 || moveTo.X == 7) && (Tiles[point].Piece is Piece_Man_M))
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

          
        }



     

        protected override void Restriction1_IsThisPieceCanEatAnotherEnemy(ObservableBoardCollection<TileModel> Tiles, MyPoint CheckPiece)
        {
            PathData viablePath = new PathData(null,null);
            if (Tiles[CheckPiece].Piece is Piece_FlyingKingC_M && Tiles[CheckPiece].Piece.MovesMade != 0)
                viablePath=ViablePath(Tiles[CheckPiece], Tiles); //------>action func
            else if (Tiles[CheckPiece].Piece is Piece_Man_M)
                viablePath=ViablePath(Tiles[CheckPiece], Tiles);//------>action func


          
            if ( viablePath.capturedPieces == null)
                Lock1 = null;
            else
            {
                Lock1 = CheckPiece;
             
            }
            



            if (!IsLock2Empty())
                Lock2.Clear();
            

        }

        protected override void Restriction2_IsAnyPieceHasToEatEnemy( ObservableBoardCollection<TileModel> Tiles, State PlayerTurn)
        {
            foreach (TileModel CanEat in Tiles)
            {


                if (CanEat.Piece != null && CanEat.Piece.Player == PlayerTurn)
                {
                    PathData viablePath= ViablePath(CanEat, Tiles);

                    if (viablePath.capturedPieces==null && IsLock2Empty() )
                        Lock2.Clear();
                    else if (viablePath.capturedPieces != null)
                    {
                        Lock2.Add(CanEat.Pos);


                    }
                  
                }

            }

        }

             public override IEnumerator<PathData> GetPieceAllPossiableMove(ObservableBoardCollection<TileModel> StateOfBoard, MyPoint PieceToCheck)
             {

            //clone stateofBOARD

            throw new Exception();
             } 
    }



}
