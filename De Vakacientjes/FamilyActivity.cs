using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_Vakacientjes
{
    class FamilyActivity
    {
        public int FamilyId { get; set; }
        public List<PlayActivity> ChildPlayActivities { get; }
        public List<PlayActivity> ParentPlayActivities { get; }

        public FamilyActivity()
        {
            FamilyId = -1;
            ChildPlayActivities = new List<PlayActivity>();
            ParentPlayActivities = new List<PlayActivity>();
        }
    }
}
