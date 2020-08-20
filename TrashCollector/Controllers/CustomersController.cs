using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrashCollector.Data;
using TrashCollector.Models;

namespace TrashCollector.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomersController : Controller
    {
        private ApplicationDbContext _dbContext;

        public CustomersController(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        // GET: Customers
        public ActionResult Index()
        {
            var customerList = _dbContext.Customer.ToList();
            return View(customerList);
        }

        // GET: Customers/Details/5
        public ActionResult Details()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _dbContext.Customer.Where(m => m.IdentityUserId ==
            userId).SingleOrDefault();
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                customer.IdentityUserId = userId;
                _dbContext.Customer.Add(customer);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int id)
        {
            Customer customer = _dbContext.Customer.Where(m => m.Id == id).FirstOrDefault();
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Customer customerUpdated)
        {
            try
            {
                Customer customer = _dbContext.Customer.Where(m => m.Id == id).FirstOrDefault();
                customer.Name = customerUpdated.Name;
                customer.Address = customerUpdated.Address;
                customer.PickUpDay = customerUpdated.PickUpDay;
                customer.OneTimePickup = customerUpdated.OneTimePickup;
                customer.StartDate = customerUpdated.StartDate;
                customer.EndDate = customerUpdated.EndDate;
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int id)
        {
            Customer customer = _dbContext.Customer.Where(m => m.Id == id).FirstOrDefault();
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Customer customer)
        {
            try
            {
                _dbContext.Customer.Remove(customer);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
