using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Collections
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NameCollection { get; set; }
        public string ImageCollection { get; set; }
        public DateTime DateOpen { get; set; }
        public DateTime DateClose { get; set; }
        public byte Status { get; set; }
        public ICollection<CollectionProduct> CollectionProducts { get; set; }

    }
}
