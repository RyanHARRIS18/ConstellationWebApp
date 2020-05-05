using ConstellationWebApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstellationWebApp.ViewModels
{
    public class UserCreateViewModel
    {
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
        [Required]
        public string UserName { get; set; }

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

     
        public string PhotoPath { get; set; }

        /*[RegularExpression("([0-9a-zA-Z :\\-_!@$%^&*()])+(.jpg|.JPG|.jpeg|.JPEG|.bmp|.BMP|.gif|.GIF|.psd|.PSD)$")]*/
        public IFormFile Photo { get; set; }


        // this REGEXP only ensure it is formated like and email; we must create an actual 
        // method to ensure that it is real


        public ICollection<UserProject> UserProjects { get; set; }
    }

}