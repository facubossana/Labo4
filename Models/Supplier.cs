using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLaboIV.Models
{
    public class Supplier
    {
        public Supplier()
        {
            this.SupplierProduct = new HashSet<SupplierProduct>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre no puede estar vacío.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El número de teléfono no puede estar vacío.")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Por favor, ingrese un número de teléfono váildo")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Teléfono")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El domicilio no puede estar vacío.")]
        [Display(Name = "Domicilio")]
        public string Adress { get; set; }

        [Required(ErrorMessage = "La localidad no puede estar vacía.")]
        [Display(Name = "Localidad")]
        public string City { get; set; }

        [Required(ErrorMessage = "La provincia no puede estar vacía.")]
        [Display(Name = "Provincia")]
        public string State { get; set; }

        [Display(Name = "Productos")]
        public ICollection<SupplierProduct> SupplierProduct { get; set; }
    }
}
