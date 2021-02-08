using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModels;
using Spice.Utility;

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Spice.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext db;
        public OrdersController(ApplicationDbContext db)
        {
            this.db = db;
        }



        [Authorize]
        public async Task<IActionResult> Confirm(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderDetailsViewModel OrderDetailsVM = new OrderDetailsViewModel()
            {
                OrderHeader = await db.OrderHeaders.Include(m=>m.ApplicationUser).FirstOrDefaultAsync(m => m.UserId == claim.Value && m.Id == id),
                OrderDetails = await db.OrderDetails.Where(m => m.OrderId == id).ToListAsync()
            };
            return View(OrderDetailsVM);
        }



        private int PageSize = 2;
        [Authorize]
        public async Task<IActionResult> OrderHistory(int pageNumber=1)
        {// جلب اليوزر الحالي 
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // List<OrderDetailsViewModel> orderDetailsVMList = new List<OrderDetailsViewModel>();
            OrderListViewModel orderListVM = new OrderListViewModel()
            {
                Orders = new List<OrderDetailsViewModel>()
            };

            List<OrderHeader> orderHeadersList = await db.OrderHeaders.Include(m => m.ApplicationUser).Where(m => m.UserId == claim.Value).ToListAsync();

            foreach (var orderHeader in orderHeadersList)
            {
                OrderDetailsViewModel orderDetailsVM = new OrderDetailsViewModel() {

                    OrderHeader = orderHeader,
                    OrderDetails = await db.OrderDetails.Where(m => m.OrderId == orderHeader.Id).ToListAsync()
                };

                orderListVM.Orders.Add(orderDetailsVM);
            }

            //
            var Count = orderListVM.Orders.Count;
            orderListVM.Orders = orderListVM.Orders.OrderByDescending(o => o.OrderHeader.Id).Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();//

            orderListVM.PagingInfo = new PagingInfo() { 
            CurrentPage=pageNumber,
            RecordsPerPage=PageSize,
            TotalRecords =Count,
            urlparam= "/Customer/Orders/OrderHistory?pageNumber=:"
            };


            return View(orderListVM);
        }

        public async Task<IActionResult> GetOrderDetails(int id)
        {
            OrderDetailsViewModel orderDetailsVM = new OrderDetailsViewModel() { 
            
            OrderHeader=await db.OrderHeaders.Include(m=>m.ApplicationUser).FirstOrDefaultAsync(m=>m.Id==id),
            OrderDetails= await db.OrderDetails.Where(m=>m.OrderId==id).ToListAsync()
            };
            return PartialView("_IndividualOrderDetails", orderDetailsVM);
        }


        public async Task<IActionResult> GetOrderStatus(int id)
        {
            OrderHeader orderHeader =await db.OrderHeaders.FindAsync(id);

            return PartialView("_OrderStatus", orderHeader.Status);//يعمل مقارنة لمعرفة عرض صورة اة حالة الطلب 
        
        }



        [Authorize(Roles =SD.ManagerUser +","+ SD.KitchenUser)]
        public async Task<IActionResult> ManageOrder()
        {


             List<OrderDetailsViewModel> orderDetailsVMList = new List<OrderDetailsViewModel>();
           

            List<OrderHeader> orderHeadersList = await db.OrderHeaders.Where(o=>o.Status==SD.StatusInProcess || o.Status==SD.StatusSubmitted).ToListAsync();

            foreach (var orderHeader in orderHeadersList)
            {
                OrderDetailsViewModel orderDetailsVM = new OrderDetailsViewModel()
                {

                    OrderHeader = orderHeader,
                    OrderDetails = await db.OrderDetails.Where(m => m.OrderId == orderHeader.Id).ToListAsync()
                };

                orderDetailsVMList.Add(orderDetailsVM);
            }


           

            return View(orderDetailsVMList.OrderBy(o=>o.OrderHeader.PickUpTime).ToList());
        }



        [Authorize(Roles = SD.ManagerUser + "," + SD.KitchenUser)]
        public async Task<IActionResult> OrderPrepare(int orderid)
        {
            var orderHeader = await db.OrderHeaders.FindAsync(orderid);
            orderHeader.Status = SD.StatusInProcess;

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(ManageOrder));


        }


        [Authorize(Roles = SD.ManagerUser + "," + SD.KitchenUser)]
        public async Task<IActionResult> OrderReady(int orderid)
        {
            var orderHeader = await db.OrderHeaders.FindAsync(orderid);
            orderHeader.Status = SD.StatusResdy;

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(ManageOrder));


        }

        [Authorize(Roles = SD.ManagerUser + "," + SD.KitchenUser)]
        public async Task<IActionResult> OrderCancel(int orderid)
        {
            var orderHeader = await db.OrderHeaders.FindAsync(orderid);
            orderHeader.Status = SD.StatusCancelled;

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(ManageOrder));


        }



        [Authorize(Roles = SD.ManagerUser + "," + SD.FrontDeskUser)]
        public async Task<IActionResult> OrderPickup(int pageNumber = 1, string searchName = null, string searchPhone = null, string searchEmail = null)
        {

            OrderListViewModel orderListVM = new OrderListViewModel()
            {
                Orders = new List<OrderDetailsViewModel>()
            };
            StringBuilder param = new StringBuilder();
            param.Append("/Customer/Orders/OrderPickup?pageNumber=:");
            param.Append("&searchName=");// عملية البحث 
            if (searchName != null)
            {
                param.Append(searchName);
            }
            else
            {
                searchName = "";
            }

            param.Append("&searchPhone=");
            if (searchPhone != null)
            {
                param.Append(searchPhone);
            }
            else
            {
                searchPhone = "";
            }

            param.Append("&searchEmail=");
            if (searchEmail != null)
            {
                param.Append(searchEmail);
            }
            else
            {
                searchEmail = "";
            }


            List<OrderHeader> orderHeadersList = await db.OrderHeaders.Include(m => m.ApplicationUser).OrderByDescending(o=>o.OrderDate)
                .Where(m => m.Status==SD.StatusResdy && m.PickUpName.Contains(searchName)&&m.PhoneNumber.Contains(searchPhone)
                && m.ApplicationUser.Email.Contains(searchEmail)).ToListAsync();



            foreach (var orderHeader in orderHeadersList)
            {
                OrderDetailsViewModel orderDetailsVM = new OrderDetailsViewModel()
                {

                    OrderHeader = orderHeader,
                    OrderDetails = await db.OrderDetails.Where(m => m.OrderId == orderHeader.Id).ToListAsync()
                };

                orderListVM.Orders.Add(orderDetailsVM);
            }


            var Count = orderListVM.Orders.Count;
            orderListVM.Orders = orderListVM.Orders.OrderByDescending(o => o.OrderHeader.Id).Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();//

            orderListVM.PagingInfo = new PagingInfo()
            {
                CurrentPage = pageNumber,
                RecordsPerPage = PageSize,
                TotalRecords = Count,
                urlparam = param.ToString()
            };


            return View(orderListVM);
        }


        [Authorize(Roles = SD.ManagerUser + "," + SD.FrontDeskUser)]
        [HttpPost]
        public async Task<IActionResult> OrderPickup(int orderId)
        {
            var orderHeader = await db.OrderHeaders.FindAsync(orderId);
            orderHeader.Status = SD.StatusCompleted;

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(OrderPickup));


        }


    }
}
