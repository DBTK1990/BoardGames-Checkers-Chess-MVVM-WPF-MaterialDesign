
using Chess0.Helper;

namespace Chess0.Model.Peices
{
    abstract class Compass
    {
        public enum CompassE { North, North_East,East,South_East ,South,South_Wast,Wast,North_Wast};
        public static readonly MyPoint[] Direction =
       {
        new MyPoint(-1,0),
        new MyPoint(-1,1),
        new MyPoint(0,1),
        new MyPoint(1,1),
        new MyPoint(1,0),
        new MyPoint(1,-1),
        new MyPoint(0,-1),
        new MyPoint(-1,-1),
        
        
        };
    }


}
