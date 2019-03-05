
using Chess0.Helper;
using Chess0.ViewModel;
using System.Collections.Generic;
using System.Windows.Media.Imaging;



namespace Chess0.Model.Peices
{
    public interface IPiece
    {
        BitmapImage ImagePiece{ get; set; }
        MyPoint Pos { get; set; }
        State Player { get; set; }
        int MovesMade { get; set; }

        List<MyPoint> PossiablePath(MyPoint TargetPos=null);
    }

    
}
