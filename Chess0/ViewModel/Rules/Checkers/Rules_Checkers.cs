using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess0.Helper;
using Chess0.Model;
using Chess0.Model.Peices;

namespace Chess0.ViewModel.Rules.Checkers
{
    class Rules_Checkers : IRules
    {


        private List<MyPoint> CapturedPiece = new List<MyPoint>();
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

            return PlayerTurn;
        }

        public void SimulatePath(TileModel Me, ObservableBoardCollection<TileModel> Tiles, List<MyPoint> _path=null)
        {
            HashSet<MyPoint> BlockedPath = new HashSet<MyPoint>();
            List<MyPoint> PossiablePath = _path ?? Me.Piece.PossiablePath();

            MyPoint Pos = Me.Pos;
            MyPoint Capture = null;

            if (_focus != Pos)
                CapturedPiece.Clear();

            for (var CheckIndex = 0; CheckIndex < PossiablePath.Count; CheckIndex++)
            {
                MyPoint Check = PossiablePath[CheckIndex];

                if (!BlockedPath.Contains(Check))
                {
                    Pos += Check;
                    if (Tiles[Pos].Piece == null)
                    {
                        if (Capture == null)
                        {
                            BlockedPath.Add(Check);
                            Tiles[Pos].MarkColor = "Green";
                            Tiles[Pos].MarkVisibility = "Visible";
                        }
                        else
                        {
                            Tiles[Pos].MarkColor = "Red";
                            Tiles[Pos].MarkVisibility = "Visible";
                            Capture = null;
                          
                        }

                    }
                    else
                    {
                        if (Tiles[Pos].Piece.Player == Me.Piece.Player)
                        {
                            if (Capture == null)
                            {
                                BlockedPath.Add(Check);
                            }
                         
                        }
                        else if (Tiles[Pos].Piece.Player != Me.Piece.Player)
                        {
                            if (Capture == null)
                            {
                                Capture = Pos;
                                CapturedPiece.Add(Pos);
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


        public void EatPiece( MyPoint point, MyPoint moveTo, ObservableBoardCollection<TileModel> Tiles, ObservableCollection<IPiece> DeadBlack, ObservableCollection<IPiece> DeadWhite)
        {
            switch (Tiles[moveTo].Piece.Player)
            {
                case State.Black:
                    DeadBlack.Add(Tiles[moveTo].Piece);
                    break;
                case State.White:
                    DeadWhite.Add(Tiles[moveTo].Piece);
                    break;

            }
           

           MovePiece(point, moveTo, Tiles);
        }

        public void MovePiece( MyPoint point, MyPoint moveTo, ObservableBoardCollection<TileModel> Tiles)
        {
            throw new NotImplementedException();
        }
    }
}
