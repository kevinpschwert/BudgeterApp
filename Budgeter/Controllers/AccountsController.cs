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
    public class AccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Accounts
        public ActionResult Index()
        {
            //var accounts = db.Accounts.Include(a => a.HouseHold);
            var user = db.Users.Find(User.Identity.GetUserId());
            AccountsViewModel avm = new AccountsViewModel()
            {
                Account = db.Accounts.Where(u => u.HouseHoldId == user.HouseHoldId).ToList(),
                BudgetHistory = db.BudgetHistories.Where(u => u.HouseHoldId == user.HouseHoldId).ToList()
            };
            return View(avm);
        }

        public ActionResult ListedIndex()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var accounts = db.Accounts.Where(u => u.HouseHoldId == user.HouseHoldId);
            return View(accounts.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var household = db.HouseHolds.Where(u => u.Id == user.HouseHoldId).ToList();
            ViewBag.HouseHoldId = new SelectList(household, "Id", "Name");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id")] Account account, string Name, string Balance)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                account.HouseHoldId = user.HouseHoldId ?? 1;
                account.Name = Name;
                account.Balance = Convert.ToDouble(Balance);
                account.ReconciledBalance = 0;
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HouseHoldId = new SelectList(db.HouseHolds, "Id", "Name", account.HouseHoldId);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            var user = db.Users.Find(User.Identity.GetUserId());
            var household = db.HouseHolds.Where(u => u.Id == user.HouseHoldId).ToList();
            ViewBag.HouseHoldId = new SelectList(household, "Id", "Name", account.HouseHoldId);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseHoldId,Name,Balance,ReconciledBalance")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseHoldId = new SelectList(db.HouseHolds, "Id", "Name", account.HouseHoldId);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
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
