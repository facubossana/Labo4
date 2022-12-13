using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLaboIV.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }
    }
}
