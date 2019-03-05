using Chess0.Helper;
using Chess0.Model;
using Chess0.Model.Peices;
using Chess0.ViewModel.Rules;
using Chess0.ViewModel.Rules.Chess;
using System;

namespace Chess0.ViewModel
{
    class CheckersBoardViewModel : BaseBoardGameViewModel
    {

        #region Constructor
        public CheckersBoardViewModel(IRules play) : base(play) { }


        #endregion Constructor


        protected override void MyOnClick(object o)
        {
            throw new NotImplementedException();
        }
    }
}
