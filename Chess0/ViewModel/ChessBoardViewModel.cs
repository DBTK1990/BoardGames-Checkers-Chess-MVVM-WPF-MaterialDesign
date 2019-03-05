using Chess0.Helper;
using Chess0.Model;
using Chess0.Model.Peices;
using Chess0.ViewModel.Rules;
using Chess0.ViewModel.Rules.Chess;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Chess0.ViewModel
{
    public enum CompassE { North, North_East, East, South_East, South, South_Wast, Wast, North_Wast };
    public enum State { White = 0, Black = 1 }

    class ChessBoardViewModel : BaseBoardGameViewModel
    {
    
        #region Constructor
        public ChessBoardViewModel(IRules play) : base(play) { }
        #endregion Constructor

        #region Commands
        protected override void MyOnClick(object o)
        {
            MyPoint tileIndex= (MyPoint)o;

            object[] dead = { DeadWhite, DeadBlack };

            if (Focus != null && Tiles[tileIndex].MarkVisibility == "Visible")
            {
                if (Tiles[tileIndex].MarkColor == "Green")
                {
                    Rules.MovePiece(Focus.Pos, tileIndex,Tiles);
                    
                }
                else if (Tiles[tileIndex].MarkColor == "Red")
                {
                    Rules.EatPiece(Focus.Pos, tileIndex,Tiles,DeadBlack,DeadWhite);
                  
                }

                Focus = Tiles[tileIndex];

                //hide all marking on the board
                foreach (TileModel tile in Tiles)
                {
                    tile.MarkVisibility = "Hidden";
                }
                //control win and turn switch
                if (Rules.WinCondition(dead[((int)PlayerTurn+1) % 2]))
                    base.ShowGameOverDialog();
                else
                    PlayerTurn =Rules.PlayerTurnSwitch(Focus,  Tiles);


                
            }
            else if (Tiles[tileIndex].Piece!=null)
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

        #endregion Commands



        #region Testing

        private void testdeadPieces()
        {
            ChessPlayer white = new ChessPlayer(State.White);
            ChessPlayer black = new ChessPlayer(State.Black);

            for (var i = 0; i < white.Pieces.Count; i++)
            {
                DeadWhite.Add(white.Pieces[i]);
                DeadBlack.Add(black.Pieces[i]);
            }

        }

        #endregion
    }
}
