using GalaSoft.MvvmLight;
using Parse;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarBlaster.ViewModels
{
    public class AppViewModel : ViewModelBase
    {
        public IEnumerable<PlayersViewModel> Players { get; set; }

        public AppViewModel()
        {
            //ParseObject player = new ParseObject("Players");

            //player["name"] = "krasi";
            //player["score"] = "35";

            //player.SaveAsync();
        }
    }
}
