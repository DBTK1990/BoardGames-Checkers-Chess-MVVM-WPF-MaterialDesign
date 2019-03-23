


using Chess0.Helper;
using Chess0.Model.Peices;
using System;

namespace Chess0.Model
{
     class TileModel : NotifyPropertyChanged , ICloneable


    {
        private string color;
        public string Color
        {
            get => color;
            set
            {
                color = value;
                OnPropertyChanged("Color");
            }
        }


        private MyPoint pos;
        public MyPoint Pos
        {
            get => pos;
            set
            {
                pos = value;
                OnPropertyChanged("Pos.X");
                OnPropertyChanged("Pos.Y");
            }
        }


        private IPiece piece;
        public IPiece Piece
        {
            get => piece;
            set
            {
                piece = value;
                OnPropertyChanged("Piece");
            }
        }


        private string markcolor;
        public string MarkColor
        {
            get => markcolor;
            set
            {
                markcolor = value;
                OnPropertyChanged("MarkColor");
            }
        }

        private string markvisibility;
        public string MarkVisibility
        {
            get => markvisibility;
            set
            {
                markvisibility = value;
                OnPropertyChanged("MarkVisibility");
            }
        }

      

        public TileModel(MyPoint pos, IPiece piece = null)
        {
            Piece = piece;
            Pos = pos;
        }


        public TileModel(string color,MyPoint pos)
        {
            Piece = null;
            Color = color;
            Pos = pos;
            MarkVisibility = "Hidden";
            MarkColor="White";
            
        }

  
        object ICloneable.Clone()
        {
            return new TileModel(Pos, Piece);
        }
    }
}
