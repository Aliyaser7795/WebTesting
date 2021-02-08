using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models.ViewModels
{
    public class SubCategoryAndCategoryViewModel
    {
        public IEnumerable<Category> CategoriesList { get; set; }

        public SubCategory SubCategory { get; set; }

       

        public String StatusMessage { get; set; }
    }
}
