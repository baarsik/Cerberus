using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cerberus.Models.ValidationAttributes;
using DataContext.Models;

namespace Cerberus.Models.ViewModels
{
    public class EditProfileViewModel
    {   
        public IList<Guid> SelectedLanguages { get; set; }

        public IList<Language> Languages { get; set; }

        public ApplicationUser User { get; set; }
    }
}