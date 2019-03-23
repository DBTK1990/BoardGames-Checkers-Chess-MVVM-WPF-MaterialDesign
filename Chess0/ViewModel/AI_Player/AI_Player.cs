using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess0.Helper;
using Chess0.Model;

namespace Chess0.ViewModel.AI_Player
{
    
    class AI_Player_Checkers
    {

        private ObservableBoardCollection<TileModel> RootStateOfBoard;
        /*
         * 
         * struct MinMaxRes
         * {
         * public int maxEval;
         * public PathData NextMove;
         * }
         * 
         * 
                function minimax(position, depth, alpha, beta, maximizingPlayer ,function )
            if depth == 0 or game over in position
                return static evaluation of position AND Path;  And eat or move

            if maximizingPlayer
                maxEval = -infinity
              
                for each piece in tiles
                    for each move of allpossibleMove(piece)
                        eval = minimax(move, depth - 1, alpha, beta, false)
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
                        eval = minimax(move, depth - 1, alpha, beta, true)
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
