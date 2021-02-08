using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class MenuItem
    {

        [key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }
        public string Image { get; set; }

        public string Spicyness { get; set; }
        public enum ESpicy {NA=0,NotSpicy=1,Spicy=2,VerySpicy=3  }//هي مجموعة تحتووي  عثوابت رقمية لها اسماء 

        [Display(Name="Category")]
        public int CategoryId { get; set; }

        [Foreignkey("CategoryId")]

        public Category Category { get; set; }


        [Display(Name = "Sub Category")]
        public int SubCategoryId { get; set; }

        [Foreignkey("SubCategoryId")]

        public SubCategory SubCategory { get; set; }
    }
}
