
using System.Collections.Generic;

namespace Spice.Models.ViewModels
{
    public class MenuItemViewModel
    {
        public MenuItem MenuItem { get; set; }

        public IEnumerable<Category> CategoriesList { get; set; }
        public IEnumerable<SubCategory> SubCatrgoriesList { get; set; }
    }
}
