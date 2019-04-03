using Chess0.Helper;
using Chess0.Model;


using System.Linq;
using Chess0.ViewModel.Rules;
using Chess0.ViewModel.Rules.Checkers;

using System;

using Chess0.ViewModel.AI_Player;
using System.Threading.Tasks;

namespace Chess0.ViewModel
{
    class CheckersBoardViewModel : BaseBoardGameViewModel
    {
        bool _AI_thinking = false;

        public bool AI_thinking { get => _AI_thinking; set => _AI_thinking = value; }
        
        #region Constructor
        public CheckersBoardViewModel(IRules play) : base(play) { }


        #endregion Constructor
  
       

      
        private bool AI_Move()
        {
           
            
                Massege.AI_IsThinking_Snackbar = true;
                AI_thinking = true;

                foreach (TileModel tile in Tiles)
                {
                    tile.MarkVisibility = "Hidden";
                    tile.MarkColor = "White";
                }

                //res val of ai
                DataMinMax movetodo = new DataMinMax();

                //genrate ai move
                movetodo = AI_Player_Checkers.MinMaxDriver(Tiles.Clone(),3, int.MinValue, int.MaxValue, State.Black, (Rules_Checkers)(Rules as Rules_Checkers).Clone());

                //logBoard org and choise of ai
                Console.WriteLine($"Org-tiles::{MyDiagnosticTools.Print_Board_ToString(Tiles)}/n/n/n");
                Console.WriteLine($"NextMove-tiles::{MyDiagnosticTools.Print_Board_ToString(movetodo.StateOfTheBoard)}/n/n/n");
           

                MyPoint focus_pos=null;
                MyPoint moveto=null;
                bool capture = (MyPoint.GetDistence(movetodo.Moves[0], movetodo.Moves[1])>=2)?true:false;


                for (int movesloop = 0; movesloop < movetodo.Moves.Count-1; movesloop++)
                {
                    focus_pos = movetodo.Moves[movesloop];
                    moveto= movetodo.Moves[movesloop+1];

                    
                    if (capture)
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                        {

                            Console.WriteLine($"capture happend: focus:({focus_pos.X},{focus_pos.Y}) , moveto:({moveto.X},{moveto.Y})");
                            try
                            {
                                Rules.EatPiece(focus_pos, moveto, Tiles, DeadBlack, DeadWhite);
                            }
                            catch (Exception e)
                            {
                                
                                Console.WriteLine(e.Message);
                                Console.WriteLine($"Exception NULL Ref EatPiece AI::{MyDiagnosticTools.Print_Board_ToString(Tiles)}/n/n/n");
                                throw new Exception("PROBLEM IN EatPiece AI Task");
                            }
                         
                        });


                    }
                    else
                    {
                          
                            
                            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                            {
                                Console.WriteLine($"move happend: focus:({focus_pos.X},{focus_pos.Y}) , moveto:({moveto.X},{moveto.Y})");
                                try
                                    {
                                        Rules.MovePiece(focus_pos, moveto, Tiles);
                                    }
                                catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                        Console.WriteLine($"Exception NULLRef MovePiece AI::{MyDiagnosticTools.Print_Board_ToString(Tiles)}/n/n/n");
                                        throw new Exception("PROBLEM IN MovePiece AI Task");
                                    }
                            });

                    
                     
                    }
               
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


             
                Massege.AI_IsThinking_Snackbar = false;

            return false;

        }

        protected async override void MyOnClick(object o)
        {
            if (!AI_thinking)
            {


                MyPoint tileIndex = (MyPoint)o;

                object[] dead = { DeadWhite, DeadBlack };


                if (Focus != null && Tiles[tileIndex].MarkVisibility == "Visible" && Tiles[tileIndex].MarkColor == "Green" && PlayerTurn==State.White)
                {

                    if ((Rules as Rules_Checkers).Capture)
                        Rules.EatPiece(Focus.Pos, tileIndex, Tiles, DeadBlack, DeadWhite);
                    else
                        Rules.MovePiece(Focus.Pos, tileIndex, Tiles);

                    //shift focus to move/eat pos
                    Focus = Tiles[tileIndex];



                    
                    //hide all marking on the board
                    foreach (TileModel tile in Tiles)
                        tile.MarkVisibility = "Hidden";
                   
                    //control win and turn switch
                    if (Rules.WinCondition(Tiles.Clone()))
                        base.ShowGameOverDialog();
                    else
                        PlayerTurn = Rules.PlayerTurnSwitch(Focus, Tiles, PlayerTurn);

                    //restriction in checkers control focus
                    MyPoint PTempFocus2 = null;
                    if ((Rules as BaseRules_Checkers).Lock1 != null)
                        PTempFocus2 = (Rules as BaseRules_Checkers).Lock1;
                    else if (!(Rules as BaseRules_Checkers).IsLock2Empty() && false)
                        PTempFocus2 = (Rules as BaseRules_Checkers).Lock2.First();



                    //chosse next turn focus
                    Focus = (PTempFocus2 != null) ? Tiles[PTempFocus2] : null;

                    //mark path for foucs on ui
                    if (Focus != null)
                        Rules.SimulatePath(Focus, Tiles);



                }
                else if (Tiles[tileIndex].Piece != null && !AI_thinking && PlayerTurn == State.White)
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


                        Focus = PTempFocus != null ? Tiles[PTempFocus] : Focus;

                        Rules.SimulatePath(Focus, Tiles);


                    }


                  

                }
                // AI_Move()
                if (PlayerTurn == State.Black)
                {
                    AI_thinking = await Task.Run(new Func<bool>(AI_Move));

                }


            }

        }
      
    }
}
