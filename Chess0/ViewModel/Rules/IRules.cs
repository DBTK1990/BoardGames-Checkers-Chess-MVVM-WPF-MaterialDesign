using Chess0.Model;
using Chess0.Helper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chess0.Model.Peices;

namespace Chess0.ViewModel.Rules
{
    interface IRules
    {
        void InitPieces(ObservableBoardCollection<TileModel> Tiles);
        void SimulatePath(TileModel Me, ObservableBoardCollection<TileModel> Tiles);
        State PlayerTurnSwitch(TileModel focus, ObservableBoardCollection<TileModel> tiles,State playerturn=State.Black);
        bool WinCondition(object ob);
        void MovePiece(MyPoint point, MyPoint moveTo, ObservableBoardCollection<TileModel> Tiles);
        void EatPiece(MyPoint point, MyPoint moveTo, ObservableBoardCollection<TileModel> Tiles, ObservableCollection<IPiece> DeadBlack, ObservableCollection<IPiece> DeadWhite);
    }
}
