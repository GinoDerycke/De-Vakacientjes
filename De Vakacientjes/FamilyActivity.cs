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

        private int GetNumberOfChildActivities(int weekNumber)
        {
            return ChildPlayActivities.Count(a => a.WeekNumber == weekNumber);

        }

        public int NumberOfChildActivities
        {
            get
            {
                return ChildPlayActivities.Count();
            }
        }

        public int NumberOfChildActivitiesWeek1
        {
            get
            {
                return GetNumberOfChildActivities(1);
            }
        }

        public int NumberOfChildActivitiesWeek2
        {
            get
            {
                return GetNumberOfChildActivities(2);
            }
        }

        public int NumberOfChildActivitiesWeek3
        {
            get
            {
                return GetNumberOfChildActivities(3);
            }
        }

        public int NumberOfChildActivitiesWeek4
        {
            get
            {
                return GetNumberOfChildActivities(4);
            }
        }

        public int NumberOfChildActivitiesWeek5
        {
            get
            {
                return GetNumberOfChildActivities(5);
            }
        }

        public int NumberOfChildActivitiesWeek6
        {
            get
            {
                return GetNumberOfChildActivities(6);
            }
        }

        public int NumberOfChildActivitiesWeek7
        {
            get
            {
                return GetNumberOfChildActivities(7);
            }
        }

        public int NumberOfChildActivitiesWeek8
        {
            get
            {
                return GetNumberOfChildActivities(8);
            }
        }

        private int GetNumberOfParentActivities(int weekNumber)
        {
            return ParentPlayActivities.Count(a => a.WeekNumber == weekNumber);
        }

        public int NumberOfParentActivities
        {
            get
            {
                return ParentPlayActivities.Count();
            }
        }

        public int NumberOfParentActivitiesWeek1
        {
            get
            {
                return GetNumberOfParentActivities(1);
            }
        }

        public int NumberOfParentActivitiesWeek2
        {
            get
            {
                return GetNumberOfParentActivities(2);
            }
        }

        public int NumberOfParentActivitiesWeek3
        {
            get
            {
                return GetNumberOfParentActivities(3);
            }
        }

        public int NumberOfParentActivitiesWeek4
        {
            get
            {
                return GetNumberOfParentActivities(4);
            }
        }

        public int NumberOfParentActivitiesWeek5
        {
            get
            {
                return GetNumberOfParentActivities(5);
            }
        }

        public int NumberOfParentActivitiesWeek6
        {
            get
            {
                return GetNumberOfParentActivities(6);
            }
        }

        public int NumberOfParentActivitiesWeek7
        {
            get
            {
                return GetNumberOfParentActivities(7);
            }
        }

        public int NumberOfParentActivitiesWeek8
        {
            get
            {
                return GetNumberOfParentActivities(8);
            }
        }

        private int GetNumberOfOverlappingActivities(int weekNumber)
        {
            return 0; //TODO GetNumberOfOverlappingActivities
        }

        public int NumberOfOverlappingActivities
        {
            get
            {
                return GetNumberOfOverlappingActivities(0);
            }
        }

        public int NumberOfOverlappingActivitiesWeek1
        {
            get
            {
                return GetNumberOfOverlappingActivities(1);
            }
        }

        public int NumberOfOverlappingActivitiesWeek2
        {
            get
            {
                return GetNumberOfOverlappingActivities(2);
            }
        }

        public int NumberOfOverlappingActivitiesWeek3
        {
            get
            {
                return GetNumberOfOverlappingActivities(3);
            }
        }

        public int NumberOfOverlappingActivitiesWeek4
        {
            get
            {
                return GetNumberOfOverlappingActivities(4);
            }
        }

        public int NumberOfOverlappingActivitiesWeek5
        {
            get
            {
                return GetNumberOfOverlappingActivities(5);
            }
        }

        public int NumberOfOverlappingActivitiesWeek6
        {
            get
            {
                return GetNumberOfOverlappingActivities(6);
            }
        }

        public int NumberOfOverlappingActivitiesWeek7
        {
            get
            {
                return GetNumberOfOverlappingActivities(7);
            }
        }

        public int NumberOfOverlappingActivitiesWeek8
        {
            get
            {
                return GetNumberOfOverlappingActivities(8);
            }
        }

        public float TotalPrice
        {
            get
            {
                int numberOfActivities = (NumberOfChildActivities - NumberOfOverlappingActivities);
                return numberOfActivities * 2; //TODO Variable price 
            }
        }

        public int ActivitySaldo
        {
            get
            {
                return 0; //TODO ActivitySaldo
            }
        }

        public Family Family
        {
            get
            {
                return VakacientjesDb.GetFamily(FamilyId);
            }
        }
    }
}
