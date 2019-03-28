using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cerberus.Models.ValidationAttributes;
using DataContext.Models;

namespace Cerberus.Models.ViewModels
{
    public class EditProfileViewModel
    {   
        public IEnumerable<Guid> SelectedLanguages { get; set; }

        public IEnumerable<Language> Languages { get; set; }

        public ApplicationUser User { get; set; }
    }
}