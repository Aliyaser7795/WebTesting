using System.Collections.Generic;

namespace Spice.Models.ViewModels
{
    public class OrderListViewModel
    {
        public List<OrderDetailsViewModel> Orders { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
