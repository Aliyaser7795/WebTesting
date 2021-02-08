using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class SubCategory
    {
        [key]
        public int Id { get; set; }

        [Required]
        [Display(Name=" Sub Category Name")]
        public String Name { get; set; }
        [Required]
        [Display(Name = " Cateory")]
        public int CategoryId { get; set; }
        [Foreignkey("CategoryId")]

        public virtual Category Category { get; set; }
    }

   
}
