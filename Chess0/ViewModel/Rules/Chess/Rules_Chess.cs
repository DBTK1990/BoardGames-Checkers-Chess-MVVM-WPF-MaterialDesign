using Chess0.Helper;
using Chess0.Model;
using Chess0.Model.Peices;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Chess0.ViewModel.Rules.Chess
{
    class Rules_Chess: BaseRules_Chess, IRules 
    {
       
        public void InitPieces(ObservableBoardCollection<TileModel> Tiles)
        {
            ChessPlayer white = new ChessPlayer(State.White);
            ChessPlayer black = new ChessPlayer(State.Black);

            for (var i = 0; i < white.Pieces.Count; i++)
            {
                Tiles[white.Pieces[i].Pos].Piece = white.Pieces[i];
                Tiles[black.Pieces[i].Pos].Piece = black.Pieces[i];

            }

        }

        public void SimulatePath(TileModel Me , ObservableBoardCollection<TileModel> Tiles)
        {
            HashSet<MyPoint> BlockedPath = new HashSet<MyPoint>();
            List<MyPoint> PossiablePath =Me.Piece.PossiablePath();

            MyPoint Pos = Me.Pos;

           



            for (var CheckIndex = 0; CheckIndex < PossiablePath.Count; CheckIndex++)
            {
                MyPoint Check = PossiablePath[CheckIndex];

                if (!BlockedPath.Contains(Check))
                {
                    Pos += Check;
                    if (Tiles[Pos].Piece == null)
                    {

                        if (Tiles[Me.Pos].Piece is Piece_Pawn_M && (Check == Direction[(int)CompassE.South_Wast] || Check == Direction[(int)CompassE.South_East] || Check == Direction[(int)CompassE.North_East] || Check == Direction[(int)CompassE.North_Wast]))
                        {
                            BlockedPath.Add(Check);
                        }
                        else
                        {
                            Tiles[Pos].MarkColor = "Green";
                            Tiles[Pos].MarkVisibility = "Visible";

                        }

                    }
                    else
                    {
                        if (Tiles[Pos].Piece.Player == Me.Piece.Player)
                        {
                            BlockedPath.Add(Check);
                        }
                        else if (Tiles[Pos].Piece.Player != Me.Piece.Player)
                        {
                            if (Me.Piece is Piece_Pawn_M && (Check == Direction[(int)CompassE.North] || Check == Direction[(int)CompassE.South]))
                                BlockedPath.Add(Check);
                            else
                            {
                                BlockedPath.Add(Check);
                                Tiles[Pos].MarkColor = "Red";
                                Tiles[Pos].MarkVisibility = "Visible";
                            }
                        }
                    }
                }
                if ((CheckIndex + 1) < PossiablePath.Count && PossiablePath[CheckIndex] != PossiablePath[CheckIndex + 1])
                {
                    Pos = Me.Pos;
                }

            }

        }

        public State PlayerTurnSwitch( TileModel focus,  ObservableBoardCollection<TileModel> tiles)
        {

            this.CheckKingTherthend(focus.Piece.Player,tiles);
          
            State PlayerTurn = focus.Piece.Player;
            switch (PlayerTurn)
            {
                case State.Black:
                    PlayerTurn= State.White;
                    break;
                case State.White:
                    PlayerTurn= State.Black;
                    break;
            }

            focus = null;      

            return PlayerTurn;

        }

        public bool WinCondition(object ob)
        {

            bool check = false;
            ObservableCollection<IPiece> DeadPieces = (ObservableCollection<IPiece>)ob;

            foreach(IPiece piece in DeadPieces)
                check = piece is Piece_King_M ? true : false;
      
            return check;
        }

        

        public void MovePiece(MyPoint point, MyPoint moveTo, ObservableBoardCollection<TileModel> Tiles)
        {

            Tiles[moveTo].Piece = Tiles[point].Piece;
            Tiles[point].Piece.Pos = moveTo;

            Tiles[point].Piece = null;

            Tiles[moveTo].Piece.MovesMade++;

           





        }

        public void EatPiece( MyPoint point, MyPoint moveTo, ObservableBoardCollection<TileModel> Tiles,ObservableCollection<IPiece> DeadBlack, ObservableCollection<IPiece> DeadWhite)
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

    }
}
