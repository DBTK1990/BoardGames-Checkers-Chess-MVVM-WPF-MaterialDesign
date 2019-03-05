using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Chess0.Helper;
using Chess0.Model;
using Chess0.Model.Peices;

namespace Chess0.ViewModel.Rules.Checkers
{
    class Rules_Checkers : IRules
    {


        private Dictionary<MyPoint,MyPoint> capturedpieces = new Dictionary<MyPoint, MyPoint>();

        public bool isCapturedPiecesEmpty=> (capturedpieces.Count() == 0) ? true : false;        
       
        private MyPoint _focus = null;

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
            State PlayerTurn = focus.Piece.Player;
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
            capturedpieces.Clear();
            return PlayerTurn;
        }

        public void SimulatePath(TileModel Me, ObservableBoardCollection<TileModel> Tiles, List<MyPoint> _path=null)
        {
            HashSet<MyPoint> BlockedPath = new HashSet<MyPoint>();
            List<MyPoint> PossiablePath = _path ?? Me.Piece.PossiablePath();

            MyPoint Pos = Me.Pos;
            MyPoint Capture = null;
        

            if (_focus != Pos)
                capturedpieces.Clear();

            for (var CheckIndex = 0; CheckIndex < PossiablePath.Count; CheckIndex++)
            {
                MyPoint Check = PossiablePath[CheckIndex];

                if (!BlockedPath.Contains(Check))
                {
                    Pos += Check;
                    if (Tiles[Pos].Piece == null)
                    {
                     
                            BlockedPath.Add(Check);
                            Tiles[Pos].MarkColor = "Green";
                            Tiles[Pos].MarkVisibility = "Visible";
                      
                      
                    }
                    else
                    {
                        if (Tiles[Pos].Piece.Player == Me.Piece.Player)
                        {
                                BlockedPath.Add(Check);
                         
                        }
                        else if (Tiles[Pos].Piece.Player != Me.Piece.Player)
                        {

                            if (Capture == null)
                            {
                                Tiles[Pos].MarkColor = "Red";
                                Tiles[Pos].MarkVisibility = "Visible";
                                capturedpieces.Add(Pos, Check);
                                Capture = Pos;
                            }
                            else
                            {
                                Tiles[Capture].MarkVisibility = "Hidden";
                                capturedpieces.Remove(Capture);
                            }
                        }
                    }
                }
                if ((CheckIndex + 1) < PossiablePath.Count && PossiablePath[CheckIndex] != PossiablePath[CheckIndex + 1])
                {
                    Pos = Me.Pos;
                }

            }

            _focus = Me.Pos;

        }

        public bool WinCondition(object ob)
        {
            ObservableCollection<IPiece> DeadPieces = (ObservableCollection<IPiece>)ob;
            return (DeadPieces.Count()==12)?true:false;
        }

        private double getDistence(MyPoint a,MyPoint b)
        {

            MyPoint res = a - b;

            res.X *= res.X;
            res.Y *= res.Y;

            return Math.Sqrt(res.X + res.Y);


        }


        public void EatPiece( MyPoint point, MyPoint moveTo, ObservableBoardCollection<TileModel> Tiles, ObservableCollection<IPiece> DeadBlack, ObservableCollection<IPiece> DeadWhite)
        {


            MyPoint direction = ((moveTo - point) / Math.Floor(getDistence(point, moveTo)));
      

            foreach( KeyValuePair<MyPoint,MyPoint> CanIeat  in capturedpieces)
            {
                if (CanIeat.Value == direction)
                {
                    switch (Tiles[CanIeat.Key].Piece.Player)
                    {
                        case State.Black:
                            DeadBlack.Add(Tiles[CanIeat.Key].Piece);
                            break;
                        case State.White:
                            DeadWhite.Add(Tiles[CanIeat.Key].Piece);
                            break;
                    }

                    Tiles[CanIeat.Key].Piece = null;
                }

            }
          
           
           MovePiece(point, moveTo, Tiles);
        }

        public void MovePiece( MyPoint point, MyPoint moveTo, ObservableBoardCollection<TileModel> Tiles)
        {
            Tiles[moveTo].Piece = Tiles[point].Piece;
            Tiles[point].Piece.Pos = moveTo;

            Tiles[point].Piece = null;

            Tiles[moveTo].Piece.MovesMade++;

        }
    }


}
