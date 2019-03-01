using Chess0.Helper;

namespace Chess0.Model
{
    public class DialogGameOverModel : NotifyPropertyChanged
    {
        private string winner=string.Empty;
        public string Winner
        {
            get
            {
                return winner;
            }
            set
            {
                winner = value;
                OnPropertyChanged("Winner");
            }
        }

        private string open;
        public string Open
        {
            get
            {
                return open;
            }
            set
            {
                if (value == "True" || value == "False")
                {
                    open = value;
                    OnPropertyChanged("Open");
                }
                else
                    open = "False";
                
            }
        }

        public DialogGameOverModel(string player)
        {
            Winner = player;
            open = "False";
        }
    }
}
