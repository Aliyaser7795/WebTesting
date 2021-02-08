using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{

    public class OrderHeader
    {
        [key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Foreignkey("UserId")]

        public virtual ApplicationUser ApplicationUser { get; set; }
        [Required]

        public DateTime OrderDate { get; set; }
        [Required]
        public double OrderTotalOrginal { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:c}")]

        public double OrderTotal { get; set; }
        [Required]
        [Display(Name = "PickUp Time")]
        public DateTime PickUpTime { get; set; }
        [Required]
        [NotMapped]
        public DateTime PickUpDate { get; set; }
        [Display(Name = "Coupon Code")]
        public string CouponCode { get; set; }

        public double CouponCodeDiscount { get; set; }

        public string Status { get; set; }


        public string PaymentStatus { get; set; }

        public string Comments { get; set; }
        [Display(Name = "PickUp Name")]
        public string PickUpName { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public string TransactionId { get; set; }


    }

   
}

