using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BudgetApp.Models;
using Microsoft.AspNet.Identity;

namespace BudgetApp.Controllers
{
    public class BudgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Budgets
        public ActionResult Index()
        {
            
            var user = db.Users.Find(User.Identity.GetUserId());
            var budgets = db.Budgets.Where(u => u.HouseHoldId == user.HouseHoldId);
            var household = db.HouseHolds.Where(u => u.Id == user.HouseHoldId).ToList();
            var frequency = db.Frequencies.Where(u => u.Id != 4).ToList();           
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.FrequencyId = new SelectList(frequency, "Id", "Name");
            ViewBag.HouseHoldId = new SelectList(household, "Id", "Name");
            BudgetViewModel bvm = new BudgetViewModel()
            {
                Budget = db.Budgets.Where(u => u.HouseHoldId == user.HouseHoldId).ToList(),
                BudgetHistory = db.BudgetHistories.Where(u => u.HouseHoldId == user.HouseHoldId).ToList()
            };
            return View(bvm);
        }

        // GET: Budgets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // GET: Budgets/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var household = db.HouseHolds.Where(u => u.Id == user.HouseHoldId).ToList();
            var category = db.Categories.Where(u => u.HouseHoldId == user.HouseHoldId).ToList();
            ViewBag.CategoryId = new SelectList(category, "Id", "Name");
            ViewBag.FrequencyId = new SelectList(db.Frequencies, "Id", "Name");
            ViewBag.HouseHoldId = new SelectList(household, "Id", "Name");
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HouseHoldId,FrequencyId")] Budget budget, string Name, string Amount)
        {
            if (ModelState.IsValid)
            {
                budget.Name = Name;
                budget.Amount = Convert.ToDouble(Amount);
                budget.Expire = DateTimeOffset.Now;
                budget.ConstAmount = budget.Amount;
                budget.TransactionAmount = 0;
                db.Budgets.Add(budget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", budget.CategoryId);
            ViewBag.FrequencyId = new SelectList(db.Frequencies, "Id", "Name", budget.FrequencyId);
            ViewBag.HouseHoldId = new SelectList(db.HouseHolds, "Id", "Name", budget.HouseHoldId);
            return View(budget);
        }

        // GET: Budgets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            var user = db.Users.Find(User.Identity.GetUserId());
            var frequency = db.Frequencies.Where(u => u.Id != 4).ToList();
            var house = db.HouseHolds.Where(u => u.Id == user.HouseHoldId).ToList();
            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", budget.CategoryId);
            ViewBag.FrequencyId = new SelectList(frequency, "Id", "Name", budget.FrequencyId);
            ViewBag.HouseHoldId = new SelectList(house, "Id", "Name", budget.HouseHoldId);
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseHoldId,Name,FrequencyId,Amount,Expire")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                var oldamount = db.Budgets.AsNoTracking().Where(u => u.Id == budget.Id);
                budget.TransactionAmount = oldamount.FirstOrDefault().TransactionAmount;
                budget.ConstAmount = budget.Amount;
                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", budget.CategoryId);
            ViewBag.FrequencyId = new SelectList(db.Frequencies, "Id", "Name", budget.FrequencyId);
            ViewBag.HouseHoldId = new SelectList(db.HouseHolds, "Id", "Name", budget.HouseHoldId);
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Budget budget = db.Budgets.Find(id);
            db.Budgets.Remove(budget);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
