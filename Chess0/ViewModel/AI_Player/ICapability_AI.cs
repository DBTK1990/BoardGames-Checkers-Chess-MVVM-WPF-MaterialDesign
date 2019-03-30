using Chess0.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess0.ViewModel.AI_Player
{
    interface ICapability_AI
    {
        bool AI_thinking { get; set; }
        
        void AI_Move();
    }
}
