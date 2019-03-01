using Chess0.Helper;
using Chess0.Model;
using Chess0.Model.Peices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Chess0.ViewModel.Rules
{
    public class Rules_Chess: IRules
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
            List<MyPoint> PossiablePath = Me.Piece.PossiablePath();

            MyPoint Pos = Me.Pos;

            foreach (TileModel mark_off in Tiles)
                mark_off.MarkVisibility = "Hidden";



            for (var CheckIndex = 0; CheckIndex < PossiablePath.Count; CheckIndex++)
            {
                MyPoint Check = PossiablePath[CheckIndex];

                if (!BlockedPath.Contains(Check))
                {
                    Pos += Check;
                    if (Tiles[Pos].Piece == null)
                    {

                        if (Tiles[Me.Pos].Piece is Piece_Pawn_M && (Check == Compass.Direction[(int)CompassE.South_Wast] || Check == Compass.Direction[(int)CompassE.South_East] || Check == Compass.Direction[(int)CompassE.North_East] || Check == Compass.Direction[(int)CompassE.North_Wast]))
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
                            BlockedPath.Add(Check);
                            Tiles[Pos].MarkColor = "Red";
                            Tiles[Pos].MarkVisibility = "Visible";
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

            this.CheckQueenTherthend(focus.Piece.Player,tiles);
          
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
            ObservableCollection<IPieceModel> DeadPieces = (ObservableCollection<IPieceModel>)ob;

            foreach(IPieceModel piece in DeadPieces)
                check = piece is Piece_Queen_M ? true : false;
      
            return check;
        }

        private void CheckQueenTherthend(State PlayerTurn, ObservableBoardCollection<TileModel> Tiles)
        {


            for (var tileIndex = 0; tileIndex < Tiles.Count; tileIndex++)
            {
                if (Tiles[tileIndex].Piece != null && Tiles[tileIndex].Piece.Player == PlayerTurn)
                {

                    MyPoint Pos = Tiles[tileIndex].Piece.Pos;
                    HashSet<MyPoint> BlockedPath = new HashSet<MyPoint>();
                    List<MyPoint> PossiablePath = Tiles[tileIndex].Piece.PossiablePath();

                    for (var CheckIndex = 0; CheckIndex < PossiablePath.Count; CheckIndex++)
                    {
                        MyPoint Check = PossiablePath[CheckIndex];

                        if (!BlockedPath.Contains(Check))
                        {
                            Pos += Check;
                            if (Tiles[Pos].Piece != null)
                                if (Tiles[Pos].Piece is Piece_Queen_M && Tiles[Pos].Piece.Player != PlayerTurn)
                                {
                                    BlockedPath.Add(Check);
                                    Tiles[tileIndex].MarkColor = "Purple";
                                    Tiles[tileIndex].MarkVisibility = "Visible";
                                }
                                else
                                    BlockedPath.Add(Check);

                        }

                        if ((CheckIndex + 1) < PossiablePath.Count && PossiablePath[CheckIndex] != PossiablePath[CheckIndex + 1])
                        {
                            Pos = Tiles[tileIndex].Pos;
                        }

                    }

                }



            }


        }

    }
}
