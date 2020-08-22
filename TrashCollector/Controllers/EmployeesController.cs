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
    [Authorize(Roles = "Employee")] 
    public class EmployeesController : Controller
    {
        private ApplicationDbContext _dbContext;

        public EmployeesController(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        // GET: EmployeeController
        public ActionResult Index()
        {
            var today = DateTime.Today.DayOfWeek.ToString();
            var todaysCustomers = new List<Customer>();
            var oneTimePickups = new List<Customer>();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = _dbContext.Employees.Where(m => m.IdentityUserId == userId).FirstOrDefault();
            if (employee == null)
            {
                return RedirectToAction("Create");
            }
            else
            {
                //var employeeZipCode = _dbContext.Employees.Where(m => m.IdentityUserId == userId).Select(m => m.ZipCode).ToString();
                todaysCustomers = _dbContext.Customers.Where(m => m.ZipCode == employee.ZipCode).Where(m => m.PickUpDay == today).ToList();
                //oneTimePickups = _dbContext.Customers.Where(m => m.OneTimePickup == today).ToList();
            }
            return View(todaysCustomers);
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            //try
            //{
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                employee.IdentityUserId = userId;
                _dbContext.Employees.Add(employee);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
        }
        //catch
        //{
        //    return View();
        //}


        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            Customer customer = _dbContext.Customers.Where(m => m.Id == id).FirstOrDefault();
            return View(customer);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Customer customerUpdates)
        {
            try
            {
                Customer customer = _dbContext.Customers.Where(m => m.Id == id).FirstOrDefault();
                customer.Balance = customerUpdates.Balance;
                customer.TrashCollected = customerUpdates.TrashCollected;
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: EmployeeController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
