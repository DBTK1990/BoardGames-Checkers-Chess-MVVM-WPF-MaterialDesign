using Chess0.Helper;
using Chess0.Model;
using Chess0.Model.Peices;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Chess0.ViewModel.Rules.Chess
{
    abstract class BaseRules_Chess  : Compass, IRules
    {

        public abstract void EatPiece(MyPoint point, MyPoint moveTo, ObservableBoardCollection<TileModel> Tiles, ObservableCollection<IPiece> DeadBlack, ObservableCollection<IPiece> DeadWhite);
        public abstract void InitPieces(ObservableBoardCollection<TileModel> Tiles);
        public abstract void MovePiece(MyPoint point, MyPoint moveTo, ObservableBoardCollection<TileModel> Tiles);
        public abstract State PlayerTurnSwitch(TileModel focus, ObservableBoardCollection<TileModel> tiles, State playerturn = State.Black);
        public abstract void SimulatePath(TileModel Me, ObservableBoardCollection<TileModel> Tiles);
        public abstract bool WinCondition(object ob);
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
