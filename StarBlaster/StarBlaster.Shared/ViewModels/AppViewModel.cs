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
            this.LoadPlayers();
        }

        public AppViewModel()
        {
            this.LoadPlayers();
        }

        public async Task LoadPlayers()
        {
            //var phoneLinq = 
            //    from phone in new ParseQuery<Phone>()
            //    where phone.Model.StartsWith("L")
            //    orderby phone.Model
            //    select phone;

            var players = await new ParseQuery<Player>()
                .FindAsync(CancellationToken.None);

            this.Players = players.AsQueryable()
                .Select(PlayerViewModel.FromModel);
            //var phones = await ParseObject.GetQuery("Phones")
            //        .FindAsync();
            //this.Phones = phones.AsQueryable()
            //    .Select(PhoneViewModel.FromParseObject);
        }
    }
}
