
using System.Windows;
using Chess0.Helper;
using Chess0.Model.Peices;


namespace Chess0.Model
{
    public class TileModel : NotifyPropertyChanged

    {
        private string color;
        public string Color
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged("Color");
            }
        }


        private MyPoint pos;
        public MyPoint Pos
        {
            get
            {
                return pos;
            }

            set
            {
                pos = value;
                OnPropertyChanged("Pos.X");
                OnPropertyChanged("Pos.Y");
            }
        }


        private IPieceModel piece;
        public IPieceModel Piece
        {
            get
            {
                return piece;
            }
            set
            {
                piece = value;
                OnPropertyChanged("Piece");
            }
        }


        private string markcolor;
        public string MarkColor
        {
            get
            {
                return markcolor;
            }
            set
            {
                markcolor = value;
                OnPropertyChanged("MarkColor");
            }
        }

        private string markvisibility;
        public string MarkVisibility
        {
            get
            {
                return markvisibility;
            }
            set
            {
                markvisibility = value;
                OnPropertyChanged("MarkVisibility");
            }
        }


        public TileModel(string color,MyPoint pos, IPieceModel piece=null)
        {
            Piece = piece;
            Color = color;
            Pos = pos;
            MarkVisibility = "Hidden";
            MarkColor="Green";
        }
    }
}
