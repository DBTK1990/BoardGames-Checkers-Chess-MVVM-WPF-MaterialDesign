
using System.Windows.Input;
using Chess0.Helper;
using Chess0.ViewModel.Rules;

namespace Chess0.ViewModel
{
    enum Game {Chess,Checkers}

    class MainWindowViewModel :  NotifyPropertyChanged
    {

        #region Proprties

        private BoardViewModel boardviewmodel;
        public BoardViewModel BoardViewModel
        {
            get
            {
                return boardviewmodel;
            }
            set
            {
                boardviewmodel = value;
                OnPropertyChanged("BoardViewModel");
            }
        }

        private ICommand choosegame;
        public ICommand ChooseGame
        {
            get { return choosegame; }
            set { choosegame = value; }
        }

        private ICommand restart_game_command;
        public ICommand RestartGameCommand
        {
            get
            {
                return restart_game_command;
            }
            set
            {
                restart_game_command = value;
            }
        }

        #endregion

        #region constructor
        public MainWindowViewModel()
        {
       
            BoardViewModel = new BoardViewModel(new Rules_Chess());
            ChooseGame = new RelayCommand(OnChooseGame);
            RestartGameCommand = BoardViewModel.RestartCommand;

        }

        #endregion constructor

        #region Command
        private void OnChooseGame(object game)
        {
            
            if (Game.Chess.ToString() == game as string)
            {
                BoardViewModel = new BoardViewModel(new Rules_Chess());
            }
            else if (Game.Checkers.ToString() == game as string)
            {
                BoardViewModel = null;
            }
        }
        #endregion Command
    }


}
