using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLaboIV.Models
{
    public class Product
    {
        public Product()
        {
            this.SupplierProduct = new HashSet<SupplierProduct>();
        }
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre no puede estar vacío.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El precio no puede estar vacío.")]
        [Range(0.10, 10000000, ErrorMessage = "El precio debe estar en el rango entre $0,10 y $10.000.000,00")]
        [Column(TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [RegularExpression("[0-9]+(\\.[0-9][0-9]?)?", ErrorMessage = "El precio debe ser un número con hasta dos decimales.")]
        [Display(Name = "Precio")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "La descripción no puede estar vacío.")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Favorito")]
        public bool Favourite { get; set; }

        [ForeignKey("Category")]
        [Display(Name = "Id-Categoria")]
        public int CategoryId { get; set; }
        
        [Display(Name = "Categoria")]
        public virtual Category Category{ get; set; }

        [ForeignKey("Brand")]
        [Display(Name = "Id-Marca")]
        public int BrandId { get; set; }

        [Display(Name = "Marca")]
        public virtual Brand Brand{ get; set; }

        [Display(Name = "Proveedores")]
        public ICollection<SupplierProduct> SupplierProduct { get; set; }

        [Display(Name = "Fotografía")]
        public string Photo { get; set; }
    }
}
