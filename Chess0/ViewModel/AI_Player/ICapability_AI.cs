using Chess0.Helper;
using Chess0.Model;
using System.Collections;
using System.Collections.Generic;


namespace Chess0.ViewModel.AI_Player
{
    interface ICapability_AI 
    {
        IEnumerator<ObservableBoardCollection<TileModel>> GetPieceAllPossiableMove(ObservableBoardCollection<TileModel> StateOfBoard/*clone to baseTilew*/,MyPoint PieceToCheck, int depth = 0);
    }
}
