using Parse;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarBlaster.Models
{
    [ParseClassName("Players")]
    public class Player : ParseObject
    {
        [ParseFieldName("name")]
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("score")]
        public string Score
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }
    }
}
