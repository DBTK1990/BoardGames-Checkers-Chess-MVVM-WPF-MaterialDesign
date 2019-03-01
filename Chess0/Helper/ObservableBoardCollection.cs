using Chess0.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Chess0.Helper
{
    public class ObservableBoardCollection<T> : ObservableCollection<T>
    {

        private int RowColLength { get; set; }

        public ObservableBoardCollection(int RowColLength) : base()
        {
            this.RowColLength = RowColLength;
            

        }

       
        public T this[MyPoint index]
        {
            get
            { 
                    return this[(int)index.X * RowColLength + (int)index.Y];  
            }

            set
            {
                
                InsertItem(((int)index.X * RowColLength + (int)index.Y), value);

            }
        }
        

        public T this[int row, int col]
        {
            get
            {
                return this[row * RowColLength + col];
            }

            set
            { 
                InsertItem(row * RowColLength + col, value);
            }
        }

        


    }
}
