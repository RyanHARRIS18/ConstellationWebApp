using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstellationWebApp.Models
{
    public class UserProject
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int Projectid { get; set; }
        public Project Project { get; set; }
    }
}
