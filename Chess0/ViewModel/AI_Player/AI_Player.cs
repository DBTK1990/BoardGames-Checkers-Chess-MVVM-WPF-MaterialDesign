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

        //fix clone
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
     

        private static DataMinMax EvaluateStateOfBoard(ObservableBoardCollection<TileModel> StateOfBoard)
        {
           
            DataMinMax Evel_arg = new DataMinMax();
            Evel_arg.StateOfTheBoard = StateOfBoard;
            Evel_arg.Eval = 0;


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
            return Evel_arg;



        }

        public static DataMinMax MinMaxDriver(ObservableBoardCollection<TileModel> StateOfBoard,int depth,int alpha,int beta,State PlayerTurn,Rules_Checkers rules)
        {
            count++;
            Console.WriteLine($"index case number:{count}");
            if (depth == 0 || rules.WinCondition(StateOfBoard))
            {
                return EvaluateStateOfBoard(StateOfBoard);
            }
            


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
                                CheckNewEval = PossiableMoveIteretor.Current;

                                if (CheckNewEval.Eval > MaxEvel.Eval)
                                    MaxEvel = CheckNewEval;

                                alpha = Math.Max(alpha, CheckNewEval.Eval);

                                if (beta <= alpha)
                                {
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
                                CheckNewEval = PossiableMoveIteretor.Current;

                                if (CheckNewEval.Eval < MinEvel.Eval)
                                    MinEvel = CheckNewEval;

                                alpha = Math.Min(beta, CheckNewEval.Eval);

                                if (beta <= alpha)
                                {
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

                    foreach (MyPoint move in viablepath.Moves)
                    {
                        if (NextMove == move)
                        {
                            MoveResults.Moves.Add(move);
                          
                            rules.EatPiece(PieceToCheck, move, MoveResults.StateOfTheBoard);
                            IEnumerator<DataMinMax> CanEatAnotherPiece = GetPieceAllPossiableMove(MoveResults.StateOfTheBoard, move, depth = depth + 1, rules);
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
                            MoveResults.Moves.Remove(move);


                            NextMove = NextMove + Capture.Key;
                        }

                    }

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
class PathData : ICloneable
        {


        public HashSet<MyPoint> Moves=null;
        public Dictionary<MyPoint, MyPoint> capturedPieces=null;

        
        public PathData( HashSet<MyPoint> _Moves, Dictionary<MyPoint, MyPoint> _capturedPieces=null)
        {
           
            Moves =(_Moves==null)?null:new HashSet<MyPoint>(_Moves);
            capturedPieces=(_capturedPieces==null)? _capturedPieces : new Dictionary<MyPoint, MyPoint>(_capturedPieces);

        }

        public object Clone()
        {
            
            return new PathData(Moves,capturedPieces);
        }
    }
}
