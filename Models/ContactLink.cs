using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ConstellationWebApp.Models
{
    public class ContactLink
    {
        public int ContactLinkID { get; set; }

        [Display(Name = "Link Label")]
        [StringLength(50)]
        public string ContactLinkLabel { get; set; }

        [Display(Name = "Link URL")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")]
        public string ContactLinkUrl { get; set; }

        public virtual User Users { get; set; }
    }
}