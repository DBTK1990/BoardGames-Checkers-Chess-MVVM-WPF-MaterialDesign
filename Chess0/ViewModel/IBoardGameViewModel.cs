using Chess0.Helper;
using Chess0.Model;
using Chess0.Model.Peices;
using System.Collections.ObjectModel;

namespace Chess0.ViewModel
{
    internal interface IBoardGameViewModel
    {
        ObservableBoardCollection<TileModel> Tiles
        {
            get;

            set;
            
        }

        ObservableCollection<IPieceModel> DeadWhite
        {
            get;
            set;
        }

        ObservableCollection<IPieceModel> DeadBlack
        {
            get;
            set;
        }

        TileModel Focus
        {
            get;
            set;
        }

        State PlayerTurn
        {
            get;
            set;
        }
        string PlayerTurnS
        {
            get;
        }
    }
}