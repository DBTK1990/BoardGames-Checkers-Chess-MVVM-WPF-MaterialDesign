using Chess0.Model;
using Chess0.Helper;
namespace Chess0.ViewModel.Rules
{
    interface IRules
    {

        void InitPieces(ObservableBoardCollection<TileModel> Tiles);
        void SimulatePath(TileModel Me, ObservableBoardCollection<TileModel> Tiles);
        State PlayerTurnSwitch(TileModel focus, ObservableBoardCollection<TileModel> tiles);
        bool WinCondition(object ob);
    }
}
