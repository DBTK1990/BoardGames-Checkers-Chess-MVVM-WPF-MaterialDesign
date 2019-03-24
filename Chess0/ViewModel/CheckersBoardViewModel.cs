using Chess0.Helper;
using Chess0.Model;

using System.Collections.Generic;
using System.Linq;
using Chess0.ViewModel.Rules;
using Chess0.ViewModel.Rules.Checkers;
using System;
using Chess0.Model.Peices;
using Chess0.ViewModel.AI_Player;

namespace Chess0.ViewModel
{
    class CheckersBoardViewModel : BaseBoardGameViewModel
    {

        #region Constructor
        public CheckersBoardViewModel(IRules play) : base(play) { }


        #endregion Constructor


        int count = 0;


        protected override void MyOnClick(object o)
        {
            MyPoint tileIndex = (MyPoint)o;

            object[] dead = { DeadWhite, DeadBlack };
            


            if (Focus != null && Tiles[tileIndex].MarkVisibility == "Visible" && Tiles[tileIndex].MarkColor == "Green")
            {// if captured is not 0 do eat

                //move it to method inside base viewmodel

           
                //dismantel capture pieces var
                MyPoint CheckCapture = tileIndex - (tileIndex - Focus.Pos) / Math.Floor(MyPoint.GetDistence(Focus.Pos, tileIndex));
                if(Tiles[CheckCapture].Pos == Focus.Pos )
                    Rules.MovePiece(Focus.Pos, tileIndex, Tiles);
                else if (Tiles[CheckCapture].MarkVisibility == "Visible" && Tiles[CheckCapture].MarkColor == "Red")
                    Rules.EatPiece(Focus.Pos, tileIndex, Tiles, DeadBlack, DeadWhite);
         

                //shift focus to move/eat pos
                Focus = Tiles[tileIndex];

                //hide all marking on the board

                //control win and turn switch

                foreach (TileModel tile in Tiles)
                    tile.MarkVisibility = "Hidden";

                if (Rules.WinCondition(Tiles.Clone()))
                    base.ShowGameOverDialog();
                else
                    PlayerTurn = Rules.PlayerTurnSwitch(Focus, Tiles);


                MyPoint PTempFocus2 = null;
                if ((Rules as BaseRules_Checkers).Lock1 != null)
                    PTempFocus2 = (Rules as BaseRules_Checkers).Lock1;
                else if (!(Rules as BaseRules_Checkers).IsLock2Empty())
                    PTempFocus2 = (Rules as BaseRules_Checkers).Lock2.First();

                

                //chosse next turn focus
                Focus = (PTempFocus2!=null) ?Tiles[PTempFocus2] : null;

                if (Focus != null)
                    Rules.SimulatePath(Focus, Tiles);

                count = 0;

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


                if (count == 0 && Focus.Piece.Player == State.Black)
                {
                    if (PlayerTurn == State.Black)
                    {


                        foreach (TileModel tile in Tiles)
                        {
                            tile.MarkVisibility = "Hidden";
                            tile.MarkColor = "White";
                        }

                        DataMinMax movetodo = new DataMinMax();

                        movetodo = AI_Player_Checkers.MinMaxDriver(Tiles.Clone(), 3, int.MinValue, int.MaxValue, State.Black, Rules as Rules_Checkers);


                        foreach (TileModel checkinpurple in Tiles)
                        {

                            checkinpurple.Piece = movetodo.Move[checkinpurple.Pos].Piece;

                          
                        }


                        PlayerTurn = State.White;

                        foreach (TileModel checkpos in Tiles)
                        {
                            if (checkpos.Piece != null)
                                if (checkpos.Pos != checkpos.Piece.Pos)
                                    Console.WriteLine("SOMTHING IS VERY WRONG");

                        }

                        count++;
                    }


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
