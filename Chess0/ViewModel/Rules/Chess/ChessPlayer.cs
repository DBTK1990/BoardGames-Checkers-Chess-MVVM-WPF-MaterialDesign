
using Chess0.Helper;
using Chess0.Model.Peices;
using System.Collections.Generic;


namespace Chess0.ViewModel.Rules.Chess
{

// this class respensiable for setting the pieces on the board
    public class ChessPlayer
    {

        private List<IPiece> pieces = new List<IPiece>();

        public List<IPiece> Pieces { get => pieces; }

        public ChessPlayer(State state)
        {
            switch (state)
            {
                case State.Black:
                    CreateBlackPieces(state);
                    break;
                case State.White:
                    CreateWhitePieces(state);
                    break;

            }

        }

        private void CreateBlackPieces(State state)
        {

            Pieces.Add(new Piece_Rook_M(new MyPoint(0, 0), state));
            Pieces.Add(new Piece_Knight_M(new MyPoint(0, 1), state));
            Pieces.Add(new Piece_Bishop_M(new MyPoint(0, 2), state));
            Pieces.Add(new Piece_Queen_M(new MyPoint(0, 3), state));
            Pieces.Add(new Piece_King_M(new MyPoint(0, 4), state));
            Pieces.Add(new Piece_Bishop_M(new MyPoint(0, 5), state));
            Pieces.Add(new Piece_Knight_M(new MyPoint(0, 6), state));
            Pieces.Add(new Piece_Rook_M(new MyPoint(0, 7), state));

            for (var i = 0; i < 8; i++)
            {
                pieces.Add(new Piece_Pawn_M(new MyPoint(1, i), state));
            }
        }

        private void CreateWhitePieces(State state)
        {
            Pieces.Add(new Piece_Rook_M(new MyPoint(7, 0), state));
            Pieces.Add(new Piece_Knight_M(new MyPoint(7, 1), state));
            Pieces.Add(new Piece_Bishop_M(new MyPoint(7, 2), state));
            Pieces.Add(new Piece_Queen_M(new MyPoint(7, 3), state));
            Pieces.Add(new Piece_King_M(new MyPoint(7, 4), state));
            Pieces.Add(new Piece_Bishop_M(new MyPoint(7, 5), state));
            Pieces.Add(new Piece_Knight_M(new MyPoint(7, 6), state));
            Pieces.Add(new Piece_Rook_M(new MyPoint(7, 7), state));

            for (var i = 0; i < 8; i++)
            {
                pieces.Add(new Piece_Pawn_M(new MyPoint(6, i), state));
            }
        }
    }
}
