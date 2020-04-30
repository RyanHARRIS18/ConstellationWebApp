using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ConstellationWebApp.Models
{
    public class ProjectLink
    {
        public int ProjectLinkID { get; set; }

        [Display(Name = "Link Label")]
        [StringLength(50)]
        public string ProjectLinkLabel { get; set; }

        [Display(Name = "Link URL")]
        public string ProjectLinkUrl { get; set; }

        public virtual Project Projects { get; set; }
    }
}