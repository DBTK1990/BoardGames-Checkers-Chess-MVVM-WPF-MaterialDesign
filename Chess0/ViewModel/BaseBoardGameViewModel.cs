using Chess0.Helper;
using Chess0.Model;
using Chess0.Model.Peices;
using Chess0.ViewModel.Rules;
using Chess0.ViewModel.Rules.Chess;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Chess0.ViewModel
{
    abstract class BaseBoardGameViewModel
    {
        #region properties
        private ObservableBoardCollection<TileModel> tiles;
        public ObservableBoardCollection<TileModel> Tiles
        {
            get
            {
                return tiles;
            }
            set
            {
                tiles = value;
            }
        }

        private ObservableCollection<IPiece> dead_white;
        public ObservableCollection<IPiece> DeadWhite
        {
            get
            {
                return dead_white;
            }
            set
            {
                dead_white = value;
            }
        }


        private ObservableCollection<IPiece> dead_black;
        public ObservableCollection<IPiece> DeadBlack
        {
            get
            {
                return dead_black;
            }
            set
            {
                dead_black = value;
            }
        }

        private TileModel focus;
        public TileModel Focus
        {
            get
            {
                return focus;
            }
            set
            {
                focus = value;
            }
        }

        private DialogGameOverModel gameover;
        public DialogGameOverModel GameOver
        {
            get
            {
                return gameover;
            }
            set
            {
                gameover = value;
            }
        }

        private State playerturn;
        public State PlayerTurn
        {
            get
            {
                return playerturn;
            }
            set
            {
                playerturn = value;
            }
        }

        private IRules rules;
        public IRules Rules
        {
            get
            {
                return rules;
            }
            set
            {
                rules = value;
            }
        }

        private ICommand tilecommand;
        public ICommand TileCommand
        {
            get
            {
                return tilecommand;

            }


            private set
            {
                tilecommand = value;

            }
        }

        private ICommand restartcommand;
        public ICommand RestartCommand
        {
            get
            {
                return restartcommand;

            }


            private set
            {
                restartcommand = value;
            }
        }

        #endregion properties

        #region Constructor
        protected BaseBoardGameViewModel(IRules play)
        {
            Tiles = new ObservableBoardCollection<TileModel>(8);

            Focus = null;

            //set game Rules
            Rules = play;

            //init boardGAME
            InitBoard();

            //Init all piece of the desire game
            if (Rules is Rules_Chess)
                Rules.InitPieces(Tiles);
            else
                (Rules as Rules.Checkers.Rules_Checkers).testPieces(Tiles);
            //testinghere

            //lists of dead pieces
            DeadBlack = new ObservableCollection<IPiece>();
            DeadWhite = new ObservableCollection<IPiece>();

            //who starts the game
            PlayerTurn = State.Black;

            //Commant init
            TileCommand = new RelayCommand(MyOnClick);
            RestartCommand = new RelayCommand(RestartGame);

            GameOver = new DialogGameOverModel("");


        }


        #endregion Constructor

        #region Commands

        protected void RestartGame(object ob)
        {
            //close all dialogs if open
            if (GameOver.Open == "True")
                GameOver.Open = "False";

            //clear deadPieces
            DeadWhite.Clear();
            DeadBlack.Clear();

            //clear the board of pieces
            for (var LoopIndex = 0; LoopIndex < 64; LoopIndex++)
            {
                Tiles[LoopIndex].Piece = null;
            }

            //initNewGame
            
                Rules.InitPieces(Tiles);
          
               
            PlayerTurn = State.Black;
        }

        protected abstract void MyOnClick(object o);

        /// <summary>
        /// 
        /// </summary>
        #endregion Commands

        #region Helpr Function

        protected void InitBoard()
        {
            for (var LoopIndex = 0; LoopIndex < 64; LoopIndex++)
            {
                TileModel tile;

                int RowIndex = (int)(LoopIndex / 8);
                int ColIndex = LoopIndex % 8;

                if (RowIndex % 2 == 0)
                    tile = (ColIndex % 2 == 0) ? new TileModel("BurlyWood", new MyPoint(RowIndex, ColIndex)) : new TileModel("#FF876539", new MyPoint(RowIndex, ColIndex));
                else
                    tile = (ColIndex % 2 == 0) ? new TileModel("#FF876539", new MyPoint(RowIndex, ColIndex)) : new TileModel("BurlyWood", new MyPoint(RowIndex, ColIndex));


                Tiles.Add(tile);
            }

        }

        protected void ShowGameOverDialog()
        {

            GameOver.Winner = PlayerTurn.ToString();
            GameOver.Open = "True";

        }

        #endregion  Helpr Function

        #region Testing

        protected void testdeadPieces()
        {
            ChessPlayer white = new ChessPlayer(State.White);
            ChessPlayer black = new ChessPlayer(State.Black);

            for (var i = 0; i < white.Pieces.Count; i++)
            {
                DeadWhite.Add(white.Pieces[i]);
                DeadBlack.Add(black.Pieces[i]);
            }

        }

        #endregion
    }
}
