using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web.Models.ValidationAttributes;
using DataContext.Models;
using Microsoft.AspNetCore.Http;

namespace Web.Models.ViewModels
{
    public class EditProfileViewModel
    {   
        public IList<Guid> SelectedLanguages { get; set; }

        public IList<Language> Languages { get; set; }

        public IFormFile Avatar { get; set; }

        public ApplicationUser User { get; set; }
    }
}