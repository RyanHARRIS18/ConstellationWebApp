

namespace ConstellationWebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Project
    {
         public int ProjectID { get; set; }

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

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        public string ImageSource { get; set; }

        // this REGEXP only ensure it is formated like and email; we must create an actual 
        // method to ensure that it is real
        [Required]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")]
        public string ProjectLinkOne { get; set; }


        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")]
        public string ProjectLinkTwo { get; set; }


        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")]
        public string ProjectLinkThree { get; set; }

        public ICollection<UserProject> UserProjects { get; set; }
    }

    }

