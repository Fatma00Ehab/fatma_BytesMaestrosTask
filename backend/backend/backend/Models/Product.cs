using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Models
{
    public enum ProductType { InStock, Fresh, External }


    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

         

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [FileExtensions(Extensions = "jpg,jpeg,png,webp")]
        [DataType(DataType.ImageUrl)]
        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
       

        [Column(TypeName = "nvarchar(50)")]
        public ProductType Type { get; set; }

    }
}
