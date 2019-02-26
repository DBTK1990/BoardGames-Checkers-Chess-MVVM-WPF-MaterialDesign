using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Chess0.Helper;

namespace Chess0.ViewModel
{
    enum Game {Chess,Checkers}

    class MainWindowViewModel : NotifyPropertyChanged
    {
        private int selectedgame;
        public int SelectedGame
        {
            get
            {
                return selectedgame;
            }
            set
            {
                selectedgame = value;
                OnPropertyChanged("SelectedGame");
            }
        }

        private ICommand choosegame;
        public ICommand ChooseGame
        {
            get { return choosegame; }
            set { choosegame = value; }
        }



        public MainWindowViewModel()
        {
            SelectedGame = (int)Game.Chess;
            ChooseGame = new RelayCommand(OnChooseGame);
        }


        private void OnChooseGame(object game)
        {
            
            if (Game.Chess.ToString() == game as string)
            {
                SelectedGame = (int)Game.Chess;
            }
            else if (Game.Checkers.ToString() == game as string)
            {
                SelectedGame = (int)Game.Checkers;
            }
        }
    }
}
