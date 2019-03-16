using Chess0.Helper;
using Chess0.Model;
using Chess0.Model.Peices;
using System.Collections.Generic;


namespace Chess0.ViewModel.Rules.Chess
{
    class BaseRules_Chess  : Compass
    {

        protected void CheckKingTherthend(State PlayerTurn, ObservableBoardCollection<TileModel> Tiles)
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
                                if (Tiles[Pos].Piece is Piece_King_M && Tiles[Pos].Piece.Player != PlayerTurn)
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
