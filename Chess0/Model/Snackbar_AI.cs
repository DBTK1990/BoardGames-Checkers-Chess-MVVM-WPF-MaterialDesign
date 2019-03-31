using Chess0.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess0.Model
{
    class Snackbar_AI : NotifyPropertyChanged
    {
        private bool ai_isthinking_snackbar;
        public bool AI_IsThinking_Snackbar
        {
            get => ai_isthinking_snackbar;
            set
            {
                ai_isthinking_snackbar = value;
                OnPropertyChanged("AI_IsThinking_Snackbar");

            }

        }
    }
}
