using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess0.Helper;
using Chess0.Model;
using Chess0.ViewModel.Rules.Checkers;

namespace Chess0.ViewModel.AI_Player
{
    
    struct DataMinMax: ICloneable
    {
        public ObservableBoardCollection<TileModel> StateOfTheBoard;
        public List<MyPoint> Moves;
     
        public int Eval;

     
        public object Clone()
        {
            DataMinMax Clone = new DataMinMax
            {
                StateOfTheBoard = StateOfTheBoard.Clone(),
                Eval = Eval,
                Moves=Moves==null?null:new List<MyPoint>(Moves)
            };
            return Clone;
        }
    }

    class AI_Player_Checkers
    {

         static int count = 0;
     


        private static DataMinMax EvaluateStateOfBoard(ObservableBoardCollection<TileModel> _stateofboard)
        {
            count++;
            Console.WriteLine($"index case number:{count}");

            DataMinMax Evel_arg = new DataMinMax()
            {
                StateOfTheBoard= _stateofboard,
                Eval = 0
            };
    

            Console.WriteLine($"StateOfBoard-tiles::{MyDiagnosticTools.Print_Board_ToString(_stateofboard)}/n/n/n");
            foreach (TileModel tile in Evel_arg.StateOfTheBoard)
            {
                if (tile.Piece != null)
                {
                    switch (tile.Piece.Player)
                    {
                        case State.Black:
                            Evel_arg.Eval++;
                            break;
                        case State.White:
                            Evel_arg.Eval--;
                            break;
                    }

                }
            }
            Console.WriteLine($"evel:{Evel_arg.Eval}");
            return Evel_arg;


        }

        public static DataMinMax MinMaxDriver(ObservableBoardCollection<TileModel> StateOfBoard,int depth,int alpha,int beta,State PlayerTurn,Rules_Checkers rules)
        {
            
            if (depth == 0 || rules.WinCondition(StateOfBoard))
            {
                return EvaluateStateOfBoard(StateOfBoard);
            }

            Console.WriteLine($"MinMaxDriver LINE82 ::StateOfBoard-tiles::{MyDiagnosticTools.Print_Board_ToString(StateOfBoard)}/n/n/n");

            #region AI_Lock2_Checkrers
            List<TileModel> lock2 = new List<TileModel>();
            foreach (MyPoint lockerPos in rules.Restriction2_IsAnyPieceHasToEatEnemy(StateOfBoard, PlayerTurn))
                lock2.Add(StateOfBoard[lockerPos]);

            IList<TileModel> res = null;
            if (lock2.Count != 0)
                res = lock2;
            else
                res = StateOfBoard;

            #endregion

            if (PlayerTurn == State.Black)
            {
                DataMinMax MaxEvel=new DataMinMax()
                {
                    Eval = int.MinValue
                };
                
                DataMinMax CheckNewEval=new DataMinMax();

                foreach (TileModel tile in res)
                {
                    if (tile.Piece != null)
                    {
                        if(tile.Piece.Player==PlayerTurn )
                        {
                            IEnumerator<DataMinMax> PossiableMoveIteretor = GetPieceAllPossiableMove(StateOfBoard, tile.Pos, 0, rules);
                           
                            while (PossiableMoveIteretor.MoveNext())
                            {
                                CheckNewEval = MinMaxDriver(PossiableMoveIteretor.Current.StateOfTheBoard, depth - 1, alpha, beta, State.White, rules) ;
                                CheckNewEval.Moves = PossiableMoveIteretor.Current.Moves;
                               

                                if (CheckNewEval.Eval > MaxEvel.Eval)
                                    MaxEvel = CheckNewEval;

                                alpha = Math.Max(alpha, CheckNewEval.Eval);

                                if (beta <= alpha)
                                {
                                    Console.WriteLine("pruning black");
                                    break;
                                }
                            }

                            

                            

                        }
                    }
                }

                return MaxEvel;

            }
            else
            {
                DataMinMax MinEvel = new DataMinMax()
                {
                    Eval = int.MaxValue
                };

                DataMinMax CheckNewEval = new DataMinMax();

                foreach (TileModel tile in res)
                {
                    if (tile.Piece != null)
                    {
                        if (tile.Piece.Player == PlayerTurn)
                        {
                            IEnumerator<DataMinMax> PossiableMoveIteretor = GetPieceAllPossiableMove(StateOfBoard, tile.Pos, 0,rules);

                            while (PossiableMoveIteretor.MoveNext())
                            {
                                CheckNewEval = MinMaxDriver(PossiableMoveIteretor.Current.StateOfTheBoard, depth - 1, alpha, beta, State.Black, rules);
                                CheckNewEval.Moves = PossiableMoveIteretor.Current.Moves;

                                if (CheckNewEval.Eval < MinEvel.Eval)
                                    MinEvel = CheckNewEval;

                                beta = Math.Min(beta, CheckNewEval.Eval);

                                if (beta <= alpha)
                                {
                                    Console.WriteLine("pruning white");
                                    break;
                                }
                            }

                           

                        }
                    }
                }

                return MinEvel;
            }


        }


        private static IEnumerator<DataMinMax> GetPieceAllPossiableMove(ObservableBoardCollection<TileModel> rootStateOfBoard, MyPoint PieceToCheck, int depth,Rules_Checkers rules)
        {
            
            Console.WriteLine($"GetPieceAllPossiableMove LINE 192 ::StateOfBoard-tiles::{MyDiagnosticTools.Print_Board_ToString(rootStateOfBoard)}/n/n/n");

            PathData viablepath = Rules_Checkers.ViablePath(rootStateOfBoard[PieceToCheck], rootStateOfBoard);

            DataMinMax MoveResults = new DataMinMax()
            {
                StateOfTheBoard = rootStateOfBoard.Clone(),
                Moves = new List<MyPoint>()
             
            };

            if (depth == 0)
                MoveResults.Moves.Add(PieceToCheck);

                if (viablepath.capturedPieces == null && depth == 0)
            {
                foreach (MyPoint move in viablepath.Moves)
                {
                   
                    MoveResults.Moves.Add(move);
                
                    rules.MovePiece(PieceToCheck, move, MoveResults.StateOfTheBoard);

                    yield return (DataMinMax)MoveResults.Clone();

                    MoveResults.Moves.Remove(move);
                    MoveResults.StateOfTheBoard = rootStateOfBoard.Clone();

                }

            }
            else if (viablepath.capturedPieces != null)
            {
                foreach (KeyValuePair<MyPoint, MyPoint> Capture in viablepath.capturedPieces)
                {
                    MyPoint NextMove = Capture.Value + Capture.Key;

                  
                            MoveResults.Moves.Add(NextMove);
                          
                            rules.EatPiece(PieceToCheck, NextMove, MoveResults.StateOfTheBoard);
                            IEnumerator<DataMinMax> CanEatAnotherPiece = GetPieceAllPossiableMove(MoveResults.StateOfTheBoard, NextMove, depth = depth + 1, rules);
                            while (CanEatAnotherPiece.MoveNext())
                            {

                                for (int movesloop = 0; movesloop < CanEatAnotherPiece.Current.Moves.Count; movesloop++)
                                    MoveResults.Moves.Add(CanEatAnotherPiece.Current.Moves[movesloop]);

                                    MoveResults.StateOfTheBoard = CanEatAnotherPiece.Current.StateOfTheBoard;
                                    yield return (DataMinMax)MoveResults.Clone();

                                for (int movesloop = 1; movesloop < CanEatAnotherPiece.Current.Moves.Count; movesloop++)
                                    MoveResults.Moves.Remove(CanEatAnotherPiece.Current.Moves[movesloop]);

                                    MoveResults.StateOfTheBoard = rootStateOfBoard.Clone();
                            }
                            MoveResults.Moves.Remove(NextMove);


                          
                        

                    

                }

            }
            else
            {
                if (depth == 0)
                {
                    yield break;
                }
                else
                {
                    
                    yield return (DataMinMax)MoveResults.Clone();
                }


            }
        }


    }


//moveto helper file

}
