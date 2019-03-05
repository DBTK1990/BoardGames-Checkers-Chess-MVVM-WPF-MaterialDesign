using Chess0.Helper;
using Chess0.Model;
using Chess0.Model.Peices;
using Chess0.ViewModel.Rules;
using Chess0.ViewModel.Rules.Checkers;
using System;

namespace Chess0.ViewModel
{
    class CheckersBoardViewModel : BaseBoardGameViewModel
    {

        #region Constructor
        public CheckersBoardViewModel(IRules play) : base(play) { }


        #endregion Constructor


        protected override void MyOnClick(object o)
        {
            MyPoint tileIndex = (MyPoint)o;

            object[] dead = { DeadWhite, DeadBlack };

            if (Focus != null && Tiles[tileIndex].MarkVisibility == "Visible" && Tiles[tileIndex].MarkColor == "Green")

            {// if captured is not 0 do eat
                if (!(Rules as Rules_Checkers).isCapturedPiecesEmpty)
                Rules.EatPiece(Focus.Pos, tileIndex, Tiles, DeadBlack, DeadWhite);
                else//if capture is 0 do move
                Rules.MovePiece(Focus.Pos, tileIndex, Tiles);

                    //if capture not null do eat


                    Focus = Tiles[tileIndex];

                    //hide all marking on the board
                    foreach (TileModel tile in Tiles)
                    {
                        tile.MarkVisibility = "Hidden";
                    }
                    //control win and turn switch
                    if (Rules.WinCondition(dead[((int)PlayerTurn + 1) % 2]))
                        base.ShowGameOverDialog();
                    else
                        PlayerTurn = Rules.PlayerTurnSwitch(Focus, Tiles);

            }
            else if (Tiles[tileIndex].Piece != null)
            {
                if (Focus == null && Tiles[tileIndex].Piece.Player == PlayerTurn || (Focus != null && Tiles[tileIndex].Piece.Player == PlayerTurn))
                {
                    foreach (TileModel tile in Tiles)
                    {
                        tile.MarkVisibility = "Hidden";
                    }

                    Focus = Tiles[tileIndex];
                    Rules.SimulatePath(Focus, Tiles);
                }
            }

        }
    }
}
