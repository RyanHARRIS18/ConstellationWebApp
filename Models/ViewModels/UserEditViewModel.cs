using ConstellationWebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstellationWebApp.Models.ViewModels
{
    public class UserEditViewModel : UserCreateViewModel
    {
        public int UserID { get; set; }
        public string OldPhotoPath { get; set; }
    }
}
