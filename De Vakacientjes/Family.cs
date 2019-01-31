using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_Vakacientjes
{
    class Family
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public Family()
        {
            Id = -1;
            Name = "";
            Email = "";
        }
    }
}
