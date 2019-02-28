using Chess0.Helper;
using Chess0.Model;
using Chess0.Model.Peices;
using System;
using System.Collections.Generic;


namespace Chess0.ViewModel
{
    public class Rules_Chess
    {
  
        public static void SimulatePath(TileModel Me ,ref ObservableBoardCollection<TileModel> Tiles)
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
                            Tiles[Pos].MarkVisibility = "Visiable";

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
                            Tiles[Pos].MarkVisibility = "Visiable";
                        }
                    }
                }
                if ((CheckIndex + 1) < PossiablePath.Count && PossiablePath[CheckIndex] != PossiablePath[CheckIndex + 1])
                {
                    Pos = Me.Pos;
                }

            }

        }


        private static void CheckQueenTherthend(State PlayerTurn,ref ObservableBoardCollection<TileModel> Tiles)
        {


            for(var tileIndex = 0; tileIndex < Tiles.Count; tileIndex++) 
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
                                    Tiles[tileIndex].MarkVisibility = "Visiable";
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


        public static State PlayerTurnSwitch(ref TileModel focus, ref ObservableBoardCollection<TileModel> tiles)
        {


            Rules_Chess.CheckQueenTherthend(focus.Piece.Player, ref tiles);

           

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

            foreach (TileModel tile in tiles)
            {
                tile.MarkVisibility = "Hidden";
            }

            return PlayerTurn;


        }


    }
}
