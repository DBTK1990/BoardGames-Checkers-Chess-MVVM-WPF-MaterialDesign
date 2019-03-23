
using Chess0.Helper;
using Chess0.Model.Peices;
using System;
using Chess0.ViewModel;
using System.Collections.Generic;

//making player class as factorys

namespace Chess0.ViewModel.Rules.Checkers
{
    class CheckersPlayer
    {

        private List<IPiece> pieces = new List<IPiece>();

        public List<IPiece> Pieces { get => pieces; }

        public CheckersPlayer()
        {
            this.CreateTestPieces();
        }
        public CheckersPlayer(State state)
        {
        
                switch (state)
                {
                    case State.Black:
                        this.CreatePieces(state, 1, 3, CreateBlackPieces);
                        break;
                    case State.White:
                        this.CreatePieces(state, 6, 8, CreateWhitePieces);
                        break;

                }
           

        }
        private void CreatePieces(State state, int startRow, int endRow, Action<int, int, int, State> enterPieces)
        {
            int middle = (startRow + endRow) / 2;
            for (var indexRow = startRow; indexRow <= endRow; indexRow++)
            {

                for (var indexCol = 1; indexCol <= 8; indexCol++)
                {
                    enterPieces(middle, indexRow, indexCol, state);
                }
            }

        }

        private void CreateBlackPieces(int middle, int indexRow, int indexCol, State state )
        {

            if (indexRow == middle && indexCol % 2 == 0)
                Pieces.Add(new Piece_Man_M(new MyPoint(indexRow - 1, indexCol - 1), state));
            else if (indexRow != middle && indexCol % 2 == 1)
                Pieces.Add(new Piece_Man_M(new MyPoint(indexRow - 1, indexCol - 1), state));

        }

        private void CreateWhitePieces(int middle, int indexRow, int indexCol, State state)
        {
            if (indexRow == middle && indexCol % 2 == 1)
                Pieces.Add(new Piece_Man_M(new MyPoint(indexRow - 1, indexCol - 1), state));
            else if (indexRow != middle && indexCol % 2 == 0)
                Pieces.Add(new Piece_Man_M(new MyPoint(indexRow - 1, indexCol - 1), state));
        }


        private void CreateTestPieces()
        {
            Pieces.Add(new Piece_Man_M(new MyPoint(1, 1), State.Black));
            Pieces.Add(new Piece_Man_M(new MyPoint(2, 2), State.Black));
            Pieces.Add(new Piece_Man_M(new MyPoint(1, 3), State.Black));
            Pieces.Add(new Piece_Man_M(new MyPoint(4, 4), State.White));
            Pieces.Add(new Piece_Man_M(new MyPoint(5, 5), State.White));
            
           
            

        }
    }
}
