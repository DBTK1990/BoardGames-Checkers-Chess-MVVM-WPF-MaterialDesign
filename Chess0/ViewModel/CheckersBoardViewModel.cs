using Chess0.Helper;
using Chess0.Model;

using System.Collections.Generic;
using System.Linq;
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

                //move it to method inside base viewmodel

           
                //dismantel capture pieces var
                MyPoint CheckCapture = tileIndex - (tileIndex - Focus.Pos) / Math.Floor(MyPoint.getDistence(Focus.Pos, tileIndex));
                if(Tiles[CheckCapture].Pos == Focus.Pos )
                    Rules.MovePiece(Focus.Pos, tileIndex, Tiles);
                else if (Tiles[CheckCapture].MarkVisibility == "Visible" && Tiles[CheckCapture].MarkColor == "Red")
                    Rules.EatPiece(Focus.Pos, tileIndex, Tiles, DeadBlack, DeadWhite);
         

                //shift focus to move/eat pos
                Focus = Tiles[tileIndex];

                //hide all marking on the board

                //control win and turn switch
                if (Rules.WinCondition(dead[((int)PlayerTurn + 1) % 2]))
                    base.ShowGameOverDialog();
                else
                    PlayerTurn = Rules.PlayerTurnSwitch(Focus, Tiles);


                MyPoint PTempFocus2 = null;
                if ((Rules as BaseRules_Checkers).Lock1 != null)
                {
                    PTempFocus2 = (Rules as BaseRules_Checkers).Lock1;
                }
                else if (!(Rules as BaseRules_Checkers).IsLock2Empty())
                {
                    PTempFocus2 = (Rules as BaseRules_Checkers).Lock2.First();
                }
                

                //chosse next turn focus
                Focus = (PTempFocus2!=null) ?Tiles[PTempFocus2] : null;

            }
            else if (Tiles[tileIndex].Piece != null)
            {
                if (Tiles[tileIndex].Piece.Player == PlayerTurn)
                {

                    foreach (TileModel tile in Tiles)
                    {
                        tile.MarkVisibility = "Hidden";
                        tile.MarkColor = "White";
                    }


                    //convertor to my point to check if null
                    MyPoint PTempFocus = null;
                    if ((Rules as Rules_Checkers).Lock1 != null)
                        PTempFocus = (Rules as Rules_Checkers).Lock1;
                    else if ((Rules as Rules_Checkers).Lock2.Count != 0)
                    {
                        if ((Rules as Rules_Checkers).Lock2.Contains(tileIndex))
                            PTempFocus = tileIndex;

                    }
                    else
                    {
                        PTempFocus = tileIndex;
                    }


                    Focus = PTempFocus!=null ? Tiles[PTempFocus]:Focus;

                    Rules.SimulatePath(Focus, Tiles);
                    
                }
              
            }

            /*if(AI_Player!=null)
                   {
                    AI_Player.ChossePathToGo(Tiles,IRules); -output:pointToMoveto


                       if(AI_Player.move==true)
                            Rules.MovePiece(piece to move, point to moveto, Tiles);
                       else if(AI_Player.eat==truee) 
                            Rules.EatPiece(Focus.Pos, tileIndex, Tiles, DeadBlack, DeadWhite);


                       PlayerTurn = Rules.PlayerTurnSwitch(Focus, Tiles);
                   }
               */

        }
    }
}
