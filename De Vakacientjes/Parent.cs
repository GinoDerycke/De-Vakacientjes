using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_Vakacientjes
{
    class Parent
    {
        public int Id { get; set; }
        public int FamilyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Parent()
        {
            Id = -1;
            FamilyId = -1;
            FirstName = "";
            LastName = "";
        }
    }
}
