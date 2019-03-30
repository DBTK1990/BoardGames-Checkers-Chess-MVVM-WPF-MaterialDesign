using Chess0.Model;
using Chess0.Model.Peices;
using Chess0.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess0.Helper
{
    static class DiagnosticTools
    {

        public static string print_Board_ToString(ObservableBoardCollection<TileModel> print)
        {
            string strboard = $"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}<";
            bool firstTime = false;
            foreach (TileModel print_tile in print)
            {
                if (firstTime)
                    strboard += ",";

                firstTime = true;

                if (print_tile.Piece == null)
                    strboard += " -- ";
                else if (print_tile.Piece is Piece_Man_M)
                    if (print_tile.Piece.Player == State.Black)
                        strboard += " mb ";
                    else
                        strboard += " mw ";
                else if (print_tile.Piece is Piece_FlyingKingC_M)
                    if (print_tile.Piece.Player == State.Black)
                        strboard += " fb ";
                    else
                        strboard += " fw ";

                if (print_tile.Pos.Y == 7)
                    strboard += $">{Environment.NewLine}";



            }
            strboard += $"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}";
            return strboard;
        }

    }
}
