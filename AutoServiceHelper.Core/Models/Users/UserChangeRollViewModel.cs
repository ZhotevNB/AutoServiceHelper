using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceHelper.Core.Models.Users
{
    public class UserChangeRollViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool WantToBeManager { get; set; }

        public bool WantToBeMechanic { get; set; }



    }
}
