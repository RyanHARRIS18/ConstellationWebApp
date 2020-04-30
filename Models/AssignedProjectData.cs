using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstellationWebApp.Models
{
    public class AssignedProjectData
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public bool Assigned { get; set; }
    }
}
