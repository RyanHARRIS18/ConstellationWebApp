using ConstellationWebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstellationWebApp.Models.ViewModels
{
    public class ProjectEditViewModel : ProjectCreateViewModel
    {
        public int ProjectID { get; set; }
        public string OldPhotoPath { get; set; }
    }
}
