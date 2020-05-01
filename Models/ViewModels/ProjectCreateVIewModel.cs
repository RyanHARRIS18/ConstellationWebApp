using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ConstellationWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ConstellationWebApp.ViewModels
{
    public class ProjectCreateViewModel
    {
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
        [Required]
        public string Title { get; set; }


        [Required]
        [StringLength(2000, ErrorMessage = "Description cannot be longer than 2000 characters.")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public DateTime CreationDate { get; set; }

        public string PhotoPath { get; set; }
        public IFormFile Photo { get; set; }

        public ICollection<UserProject> UserProjects { get; set; }

        public string SearchString { get; set; }

        // this REGEXP only ensure it is formated like and email; we must create an actual 
        // method to ensure that it is real

        /* public string UserName { get; set; }*/

    }
}
