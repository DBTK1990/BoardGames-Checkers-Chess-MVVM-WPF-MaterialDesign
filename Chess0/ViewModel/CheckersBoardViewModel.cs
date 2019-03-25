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

        State AI = State.Black;

        private string print_Board_ToString(ObservableBoardCollection<TileModel> print)
        {
            string strboard = $"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}<";
            bool firstTime = false;
            foreach (TileModel print_tile in print)
            {
                if(firstTime)
                strboard += ",";

                firstTime = true;

                if (print_tile.Piece == null)
                    strboard += " -- ";
                else if (print_tile.Piece is Piece_Man_M)
                    if (print_tile.Piece.Player==State.Black)
                        strboard += " mb ";
                    else
                        strboard += " mw ";
                else if (print_tile.Piece is Piece_FlyingKingC_M)
                    if (print_tile.Piece.Player == State.Black)
                        strboard += " fb ";
                    else
                        strboard += " fw ";

                if(print_tile.Pos.Y==7)
                    strboard += $">{Environment.NewLine}";



            }
            strboard += $"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}";
            return strboard;
        }

        private void AI_Move()
        {

            if (PlayerTurn == State.Black)
            {


                foreach (TileModel tile in Tiles)
                {
                    tile.MarkVisibility = "Hidden";
                    tile.MarkColor = "White";
                }

                DataMinMax movetodo = new DataMinMax();

                movetodo = AI_Player_Checkers.MinMaxDriver(Tiles.Clone(), 1, int.MinValue, int.MaxValue, State.Black, Rules as Rules_Checkers);

                //eatPieces
                Console.WriteLine($"/n/n/n{print_Board_ToString(movetodo.Move)}/n/n/n");
                Console.WriteLine($"/n/n/n{print_Board_ToString(Tiles)}/n/n/n");

                //genrate ai move
                MyPoint focus_pos=null;
                MyPoint moveto=null;
                bool capture=false;


                foreach (TileModel check in Tiles)
                {



                    if (check.Piece != null)
                    {
                        if (movetodo.Move[check.Pos].Piece == null && check.Piece.Player == AI)
                            focus_pos = check.Pos;
                        else if (movetodo.Move[check.Pos].Piece == null && check.Piece.Player == State.White)
                            capture = true;
                    }

                    if (movetodo.Move[check.Pos].Piece != null)
                        if (check.Piece == null && movetodo.Move[check.Pos].Piece.Player == AI)
                            moveto = check.Pos;

                 
                }


                

                if (capture)
                {
                    Rules.EatPiece(focus_pos, moveto, Tiles, DeadBlack, DeadWhite);

                }
                else
                {
                    Rules.MovePiece(focus_pos, moveto, Tiles);
                }

                //playerTURNswitch

                if (Rules.WinCondition(Tiles.Clone()))
                    base.ShowGameOverDialog();
                else
                    PlayerTurn = Rules.PlayerTurnSwitch(Focus, Tiles, PlayerTurn);

                MyPoint PTempFocus2 = null;
                if ((Rules as BaseRules_Checkers).Lock1 != null)
                    PTempFocus2 = (Rules as BaseRules_Checkers).Lock1;
                else if (!(Rules as BaseRules_Checkers).IsLock2Empty())
                    PTempFocus2 = (Rules as BaseRules_Checkers).Lock2.First();



                //chosse next turn focus
                Focus = (PTempFocus2 != null) ? Tiles[PTempFocus2] : null;

                if (Focus != null)
                    Rules.SimulatePath(Focus, Tiles);





            }
        }

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
                    PlayerTurn = Rules.PlayerTurnSwitch(Focus, Tiles, PlayerTurn);

                MyPoint PTempFocus2 = null;
                if ((Rules as BaseRules_Checkers).Lock1 != null)
                    PTempFocus2 = (Rules as BaseRules_Checkers).Lock1;
                else if (!(Rules as BaseRules_Checkers).IsLock2Empty())
                    PTempFocus2 = (Rules as BaseRules_Checkers).Lock2.First();

                

                //chosse next turn focus
                Focus = (PTempFocus2!=null) ?Tiles[PTempFocus2] : null;

                if (Focus != null)
                    Rules.SimulatePath(Focus, Tiles);

           

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
            AI_Move();
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
