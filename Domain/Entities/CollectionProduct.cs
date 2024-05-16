using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CollectionProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CollectionId { get; set; }
        public virtual Collections Collection { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
