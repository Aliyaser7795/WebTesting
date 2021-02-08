using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class OrderDetail
    {
        [key]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Foreignkey("OrderId")]
        public virtual OrderHeader OrderHeader { get; set; }
        [Required]
        public int MenuItemId { get; set; }

        [Foreignkey("MenuItemId")]
        public virtual MenuItem MenuItem { get; set; }

        public int Count { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }
    }
}