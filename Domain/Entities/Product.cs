using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NameProduct { get; set; }
        public string DescriptionProduct { get; set; }       
        public double Price { get; set; }
        public int Quantity {  get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [ForeignKey("MaterialId")]
        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }

        [ForeignKey("GenderId")]
        public int GenderId { get; set; }
        public virtual Gender Gender { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }
        public ICollection<ZodiacProduct> ProductZodiacs { get; set; }
        public ICollection<CollectionProduct> CollectionProducts { get; set; }
    }
}
