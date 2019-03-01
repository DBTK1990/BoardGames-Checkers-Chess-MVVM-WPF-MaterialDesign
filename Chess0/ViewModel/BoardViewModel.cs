using Chess0.Helper;
using Chess0.Model;
using Chess0.Model.Peices;
using Chess0.ViewModel.Rules;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Chess0.ViewModel
{
    public enum CompassE { North, North_East, East, South_East, South, South_Wast, Wast, North_Wast };

    class BoardViewModel 
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

        private ObservableCollection<IPieceModel> dead_white;
        public ObservableCollection<IPieceModel> DeadWhite
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


        private ObservableCollection<IPieceModel> dead_black;
        public ObservableCollection<IPieceModel> DeadBlack
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
        public TileModel Focus {
            get
            {
                return focus;
            }
            set
            {
                focus = value;
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

        public string PlayerTurnS
        {
            get
            {
                return playerturn.ToString();
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
        public BoardViewModel(IRules play) 
        {
            Tiles = new ObservableBoardCollection<TileModel>(8);
          
            Focus = null;

            //set game Rules
            Rules = play;

            //init boardGAME
            InitBoard();

            //Init all piece of the desire game
            Rules.InitPieces(Tiles);

            //lists of dead pieces
            DeadBlack = new ObservableCollection<IPieceModel>();
            dead_white = new ObservableCollection<IPieceModel>();

            //who starts the game
            PlayerTurn = State.Black;

            //Commant init
            TileCommand = new RelayCommand(MyOnClick);
            RestartCommand = new RelayCommand(RestartGame);
          

        }

      
        #endregion Constructor

        #region Commands

        private void RestartGame(object ob)
        {
           //close all dialogs if open
            DialogHost.CloseDialogCommand.Execute(null, null);

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
        }

        private void MyOnClick(object o)
        {
            MyPoint tileIndex= (MyPoint)o;

            object[] dead = { DeadWhite, DeadBlack };

            if (Focus != null && Tiles[tileIndex].MarkVisibility == "Visiable")
            {
                if (Tiles[tileIndex].MarkColor == "Green")
                {
                    MovePiece(Focus.Pos, tileIndex);
                }
                else if (Tiles[tileIndex].MarkColor == "Red")
                {
                    EatPiece(Focus.Pos, tileIndex);
                }

              
                //control win and turn switch
                if (Rules.WinCondition(dead[((int)PlayerTurn+1) % 2]))
                    ShowGameOverDialog();
                else
                    PlayerTurn =Rules.PlayerTurnSwitch( focus,  tiles);


                
            }
            else if (Tiles[tileIndex].Piece!=null)
                {
                    if (Focus == null && Tiles[tileIndex].Piece.Player == PlayerTurn || (Focus != null && Tiles[tileIndex].Piece.Player == PlayerTurn))
                    {
                        Focus = Tiles[tileIndex];
                        Rules.SimulatePath(Focus, Tiles);
                    }
                }

          
        }

        #endregion Commands

        #region Helpr Function

        private void InitBoard()
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

        private void ShowGameOverDialog()
        {


            foreach (TileModel tile in Tiles)
            {
                tile.MarkVisibility = "Hidden";
            }


            DialogGameOverModel gameOver = new DialogGameOverModel(this.PlayerTurnS);
            DialogHost.OpenDialogCommand.Execute(gameOver, null);

        }

        private void MovePiece(MyPoint point, MyPoint moveTo)
        {

            Tiles[moveTo].Piece = Tiles[point].Piece;
            Tiles[point].Piece.Pos = moveTo;

            Tiles[point].Piece = null;

            Tiles[moveTo].Piece.MovesMade++;

            Focus = Tiles[moveTo];


        }

        private void EatPiece(MyPoint point, MyPoint moveTo)
        {

            switch (Tiles[moveTo].Piece.Player)
            {
                case State.Black:
                    DeadBlack.Add(Tiles[moveTo].Piece);
                    break;
                case State.White:
                    DeadWhite.Add(Tiles[moveTo].Piece);
                    break;

            }

            MovePiece(point, moveTo);


        }

        #endregion  Helpr Function

        #region Testing

        private void testdeadPieces()
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
