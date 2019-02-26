using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess0.Model
{
    public class GameOverModel
    {
        private string winner=string.Empty;
        public string Winner { get { return winner; }set { winner = value; } }

        public GameOverModel(string player)
        {
            Winner = player;
        }
    }
}
