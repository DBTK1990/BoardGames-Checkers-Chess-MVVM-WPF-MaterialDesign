using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess0.Model
{
    public class DialogGameOverModel
    {
        private string winner=string.Empty;
        public string Winner { get { return winner; }set { winner = value; } }

        public DialogGameOverModel(string player)
        {
            Winner = player;
        }
    }
}
