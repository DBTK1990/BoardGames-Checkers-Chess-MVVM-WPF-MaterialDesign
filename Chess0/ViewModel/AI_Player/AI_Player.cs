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
       public ObservableBoardCollection<TileModel> Move;
       public int Eval;

        public object Clone()
        {
            DataMinMax Clone = new DataMinMax
            {
                Move = Move.Clone(),
                Eval = Eval
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
            Evel_arg.Move = StateOfBoard;
            Evel_arg.Eval = 0;


            foreach (TileModel tile in Evel_arg.Move)
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
            return (DataMinMax)Evel_arg.Clone();



        }

        public static DataMinMax MinMaxDriver(ObservableBoardCollection<TileModel> StateOfBoard,int depth,int alpha,int beta,State PlayerTurn,Rules_Checkers rules)
        {
            count++;
            Console.WriteLine($"index case number:{count}");
            if (depth == 0 || rules.WinCondition(StateOfBoard))
            {
                return EvaluateStateOfBoard(StateOfBoard);
            }
            ;

            if (PlayerTurn == State.Black)
            {
                DataMinMax MaxEvel=new DataMinMax();
                MaxEvel.Eval = int.MinValue;
               
                List<TileModel> lock2 = new List<TileModel>();
                foreach (MyPoint lockerPos in rules.Restriction2_IsAnyPieceHasToEatEnemy(StateOfBoard, PlayerTurn))
                {
                    lock2.Add(StateOfBoard[lockerPos]);
                }

               

                IList<TileModel> res=null;
                if (lock2.Count != 0)
                    res = lock2;
                else
                    res = StateOfBoard;

                DataMinMax CheckNewEval=new DataMinMax();
                foreach (TileModel tile in res)
                {
                    if (tile.Piece != null)
                    {
                        if(tile.Piece.Player==PlayerTurn )
                        {
                            IEnumerator<ObservableBoardCollection<TileModel>> PossiableMoveIteretor = rules.GetPieceAllPossiableMove(StateOfBoard, tile.Pos, 0);
                           
                            while (PossiableMoveIteretor.MoveNext())
                            {
                                CheckNewEval = MinMaxDriver(PossiableMoveIteretor.Current, depth - 1, alpha, beta, State.White, rules) ;
                                CheckNewEval.Move = PossiableMoveIteretor.Current;

                                if (CheckNewEval.Eval > MaxEvel.Eval)
                                    MaxEvel = (DataMinMax)CheckNewEval.Clone();

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
                DataMinMax MinEvel = new DataMinMax();
                MinEvel.Eval = int.MaxValue;
                

                List<TileModel> lock2 = new List<TileModel>();
                foreach (MyPoint lockerPos in rules.Restriction2_IsAnyPieceHasToEatEnemy(StateOfBoard, PlayerTurn))
                {
                    lock2.Add(StateOfBoard[lockerPos]);
                }

                IList<TileModel> res = null;
                if (lock2.Count != 0)
                    res = lock2;
                else
                    res = StateOfBoard;

                DataMinMax CheckNewEval = new DataMinMax();
                foreach (TileModel tile in res)
                {
                    if (tile.Piece != null)
                    {
                        if (tile.Piece.Player == PlayerTurn)
                        {
                            IEnumerator<ObservableBoardCollection<TileModel>> PossiableMoveIteretor = rules.GetPieceAllPossiableMove(StateOfBoard, tile.Pos, 0);

                            while (PossiableMoveIteretor.MoveNext())
                            {
                                CheckNewEval = MinMaxDriver(PossiableMoveIteretor.Current, depth - 1, alpha, beta, State.Black, rules);

                                CheckNewEval.Move = PossiableMoveIteretor.Current;

                                if (CheckNewEval.Eval < MinEvel.Eval)
                                    MinEvel = (DataMinMax)CheckNewEval.Clone();

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

        /*
         * 
         * struct MinMaxRes
         * {
         * public int maxEval;
         * public PathData NextMove;
         * }
         * 
         * 
                function minimax(stateOfBoard, depth, alpha, beta, PlayerTurn  )
            if depth == 0 or game over in position
                return static evaluation of position AND Path;  And eat or move

            if PlayerTurn==state.black
                maxEval = -infinity
              
                for each piece in tiles
                    for each move of allpossibleMove(piece)
                        eval = minimax(move, depth - 1, alpha, beta, state.White)
                       
                        if(eval[MinMaxRes.maxEval]>maxEval)
                            maxEval = eval[MinMaxRes.maxEval];
                            eval[MinMaxRes.BestNextMove]=move;
                        alpha = max(alpha, eval)
                        if beta <= alpha
                               break
                        return maxEval,BestNextMove

            else
                minEval = +infinity
                 for each piece in tiles
                    for each move of allpossibleMove(piece)
                        eval = minimax(move, depth - 1, alpha, beta, state.Black)
                    minEval = min(minEval, eval)
                    beta = min(beta, eval)
                    if beta <= alpha
                        break
                return minEval,null


        // initial call
        minimax(currentPosition, 3, -∞, +∞, true)
         */


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

        object ICloneable.Clone()
        {
            
            return new PathData(Moves,capturedPieces);
        }
    }
}
