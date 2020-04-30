namespace ConstellationWebApp.Models
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
  
    public partial class User
    {
        public int UserID { get; set; }

        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
        [Required]
        public string UserName { get; set; }

        //The following regular expression is for a 4 to 8 char password and 
        //containing at least an alphabet and one Number.You can change numbers 
        //in the end to your allowed password length. ~ using this for testing/dev

        // "^.*(?=.{10,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$" 
        //is for the regular expression for validating a password with the condition 
        //that Password should contain at least an alphabet and one Symbol or Number. - use this in deployment

        /* [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{4,15}$")]*/
        [Required]
        public string Password { get; set; }


        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters and must only contain letters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters and must only contain letters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(2000, ErrorMessage = "Bio cannot be longer than 2000 characters.")]
        public string Bio { get; set; }


        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [StringLength(30)]
        public string Seeking { get; set; }


        /*[RegularExpression("([0-9a-zA-Z :\\-_!@$%^&*()])+(.jpg|.JPG|.jpeg|.JPEG|.bmp|.BMP|.gif|.GIF|.psd|.PSD)$")]*/
        public string PhotoPath { get; set; }


        // this REGEXP only ensure it is formated like and email; we must create an actual 
        // method to ensure that it is real

        public virtual ICollection<ContactLink> ContactLinks { get; set; }


        public ICollection<UserProject> UserProjects { get; set; }
    }

}


