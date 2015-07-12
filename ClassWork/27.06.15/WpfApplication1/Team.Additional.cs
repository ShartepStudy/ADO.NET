using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    partial class Team
    {
        public IEnumerable<Game> AllGames
        {
            get { return Games.Union(Games1); }
        }
    }
}
