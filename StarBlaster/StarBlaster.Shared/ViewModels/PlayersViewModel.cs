using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarBlaster.ViewModels
{
    public class PlayersViewModel
    {
        public PlayersViewModel()
        {

        }

        public PlayersViewModel(string name, string score)
        {
            this.Name = name;
            this.Score = score;
        }
        public string Name { get; set; }

        public string Score { get; set; }
    }
}
