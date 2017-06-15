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
    public class BudgetHistoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BudgetHistories
        public ActionResult Index(string Title)
        {
            ViewBag.ThisTitle = Title;
            var user = db.Users.Find(User.Identity.GetUserId());
            var budgetHistories = db.BudgetHistories.Where(u => u.HouseHoldId == user.HouseHoldId);
            return View(budgetHistories.ToList());
        }

        // GET: BudgetHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetHistory budgetHistory = db.BudgetHistories.Find(id);
            if (budgetHistory == null)
            {
                return HttpNotFound();
            }
            return View(budgetHistory);
        }

        // GET: BudgetHistories/Create
        public ActionResult Create()
        {
            ViewBag.FrequencyId = new SelectList(db.Frequencies, "Id", "Name");
            ViewBag.HouseHoldId = new SelectList(db.HouseHolds, "Id", "Name");
            return View();
        }

        // POST: BudgetHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Date,Name,HouseHoldId,FrequencyId,TransactionAmount,Budgeted")] BudgetHistory budgetHistory)
        {
            if (ModelState.IsValid)
            {
                db.BudgetHistories.Add(budgetHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FrequencyId = new SelectList(db.Frequencies, "Id", "Name", budgetHistory.FrequencyId);
            ViewBag.HouseHoldId = new SelectList(db.HouseHolds, "Id", "Name", budgetHistory.HouseHoldId);
            return View(budgetHistory);
        }

        // GET: BudgetHistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetHistory budgetHistory = db.BudgetHistories.Find(id);
            if (budgetHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.FrequencyId = new SelectList(db.Frequencies, "Id", "Name", budgetHistory.FrequencyId);
            ViewBag.HouseHoldId = new SelectList(db.HouseHolds, "Id", "Name", budgetHistory.HouseHoldId);
            return View(budgetHistory);
        }

        // POST: BudgetHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Date,Name,HouseHoldId,FrequencyId,TransactionAmount,Budgeted")] BudgetHistory budgetHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budgetHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FrequencyId = new SelectList(db.Frequencies, "Id", "Name", budgetHistory.FrequencyId);
            ViewBag.HouseHoldId = new SelectList(db.HouseHolds, "Id", "Name", budgetHistory.HouseHoldId);
            return View(budgetHistory);
        }

        // GET: BudgetHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetHistory budgetHistory = db.BudgetHistories.Find(id);
            if (budgetHistory == null)
            {
                return HttpNotFound();
            }
            return View(budgetHistory);
        }

        // POST: BudgetHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BudgetHistory budgetHistory = db.BudgetHistories.Find(id);
            db.BudgetHistories.Remove(budgetHistory);
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
