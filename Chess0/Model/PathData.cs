using Chess0.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess0.Model
{
    class PathData : ICloneable
    {


        public HashSet<MyPoint> Moves = null;
        public Dictionary<MyPoint, MyPoint> capturedPieces = null;


        public PathData(HashSet<MyPoint> _Moves, Dictionary<MyPoint, MyPoint> _capturedPieces = null)
        {

            Moves = (_Moves == null) ? null : new HashSet<MyPoint>(_Moves);
            capturedPieces = (_capturedPieces == null) ? _capturedPieces : new Dictionary<MyPoint, MyPoint>(_capturedPieces);

        }

        public object Clone()
        {

            return new PathData(Moves, capturedPieces);
        }
    }
}
