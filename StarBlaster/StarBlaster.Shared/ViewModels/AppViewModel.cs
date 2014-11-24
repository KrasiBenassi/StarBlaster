using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Parse;
using StarBlaster.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections;

namespace StarBlaster.ViewModels
{
    public class AppViewModel : ViewModelBase
    {
        private ObservableCollection<PlayerViewModel> players;
        private ICommand refreshCommand;

        public IEnumerable<PlayerViewModel> Players
        {
            get
            {
                if (this.players == null)
                {
                    this.Players = new ObservableCollection<PlayerViewModel>();
                }
                return this.players;
            }
            set
            {
                if (this.players == null)
                {
                    this.players = new ObservableCollection<PlayerViewModel>();
                }
                this.players.Clear();
                foreach (var item in value)
                {
                    this.players.Add(item);
                }
            }
        }

        public ICommand Refresh
        {
            get
            {
                if (this.refreshCommand == null)
                {
                    this.refreshCommand = new RelayCommand(this.PerformRefresh);
                }
                return this.refreshCommand;
            }
        }

        private void PerformRefresh()
        {
            this.players.Clear();
            this.LoadPlayers();
        }

        public AppViewModel()
        {
            if (!(this.players == null))
            {
                this.players.Clear();
            } 
            this.LoadPlayers();
        }

        public async Task LoadPlayers()
        {
            var players = await new ParseQuery<Player>().OrderByDescending("score")
                .FindAsync(CancellationToken.None);
            this.Players = players.AsQueryable()
                .Select(PlayerViewModel.FromModel);
        }
    }
}
