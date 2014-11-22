using Parse;
using StarBlaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace StarBlaster.ViewModels
{
    public class PlayerViewModel
    {
        public static Expression<Func<Player, PlayerViewModel>> FromModel
        {
            get
            {
                return model =>
                    new PlayerViewModel()
                    {
                        Name = model.Name,
                        Score = model.Score
                    };
            }
        }

        public string Name { get; set; }

        public string Score { get; set; }
    }
}
