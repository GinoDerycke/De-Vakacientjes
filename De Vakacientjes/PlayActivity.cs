using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_Vakacientjes
{
    class PlayActivity
    {
        public int PersonId { get; set; }
        public int WeekNumber { get; set; }
        public int DayNumber { get; set; }
        public bool Morning { get; set; }

        public PlayActivity()
        {
            PersonId = -1;
            WeekNumber = -1;
            DayNumber = -1;
            Morning = false;
        }
    }
}
