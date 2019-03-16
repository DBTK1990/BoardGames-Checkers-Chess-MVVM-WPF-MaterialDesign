using System.Collections.Generic;
using System.Linq;
using Chess0.Helper;

namespace Chess0.ViewModel.Rules.Checkers
{
     class BaseRules_Checkers
    {


        protected Dictionary<MyPoint, MyPoint> capturedpieces = new Dictionary<MyPoint, MyPoint>();

        //LOCK1: CONTROL CONSACTIVE EATS
        private MyPoint _lock1 = null;
        public MyPoint Lock1
        {
            get => _lock1;
            protected set => _lock1 = value;
        }
        //LOCK2: CONTROL HAS TO EAT RULE
        private List<MyPoint> _lock2 = new List<MyPoint>();
        public List<MyPoint> Lock2
        {
            get => _lock2;
            protected set => _lock2 = value;
        }
        public bool isCapturedPiecesEmpty => (capturedpieces.Count() == 0) ? true : false;



    }
}
