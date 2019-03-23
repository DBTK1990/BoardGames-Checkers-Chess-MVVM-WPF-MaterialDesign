﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Chess0.Helper;
using Chess0.Model;
using Chess0.Model.Peices;
using Chess0.ViewModel.AI_Player;

namespace Chess0.ViewModel.Rules.Checkers
{
     abstract class BaseRules_Checkers: IRules, ICapability_AI
    {


      

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
     
        public bool IsLock2Empty() => (Lock2.Count() == 0) ? true : false;

        public abstract MyPoint Restriction1_IsThisPieceCanEatAnotherEnemy(ObservableBoardCollection<TileModel> tiles,MyPoint CheckPiece);
        public abstract List<MyPoint> Restriction2_IsAnyPieceHasToEatEnemy(ObservableBoardCollection<TileModel> tiles,State PlayerTurn);
        public abstract void InitPieces(ObservableBoardCollection<TileModel> Tiles);
        public abstract void SimulatePath(TileModel Me, ObservableBoardCollection<TileModel> Tiles);
        public abstract State PlayerTurnSwitch(TileModel focus, ObservableBoardCollection<TileModel> tiles);
        public abstract bool WinCondition(object ob);
        public abstract void MovePiece(MyPoint point, MyPoint moveTo, ObservableBoardCollection<TileModel> Tiles);
        public abstract void EatPiece(MyPoint point, MyPoint moveTo, ObservableBoardCollection<TileModel> Tiles, ObservableCollection<IPiece> DeadBlack=null, ObservableCollection<IPiece> DeadWhite=null);
        public abstract IEnumerator<ObservableBoardCollection<TileModel>> GetPieceAllPossiableMove(ObservableBoardCollection<TileModel> StateOfBoard, MyPoint PieceToCheck, int depth = 0);


       
        
    }
}
