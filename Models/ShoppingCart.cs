using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class ShoppingCart
    {

        public ShoppingCart()
        {
            Count = 1;
        }
        [key]
        public int Id { get; set; }

      
        public string ApplicationUserId { get; set; }

        [NotMapped]
        [Foreignkey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int MenuItemId { get; set; }

        [NotMapped]
        [Foreignkey("MenuItemId")]
        public virtual MenuItem MenuItem { get; set; }

        [Range(1,int.MaxValue,ErrorMessage ="Please Enter value grater than or equal 1")]
        public int Count { get; set; }
    }
}
