using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLaboIV.Models
{
    public class SupplierProduct
    {
        [Key]
        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
