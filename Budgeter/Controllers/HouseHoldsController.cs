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
using BudgetApp.Helper;

namespace BudgetApp.Controllers
{
    public class HouseHoldsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        HouseholdHelper hh = new HouseholdHelper();

        // GET: HouseHolds
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var budget = db.Budgets.Where(u => u.HouseHoldId == user.HouseHoldId).ToList();
            ViewBag.BudgetId = new SelectList(budget, "Id", "Name");
            HouseHoldViewModel hvm = new HouseHoldViewModel()
            {
                HouseHold = db.HouseHolds.Where(u => u.Id == user.HouseHoldId).ToList(),
                Invitation = db.Invitations.Where(u => u.Accepted == false && u.HouseHoldId == user.HouseHoldId).ToList(),
                Category = db.Categories.Where(u => u.HouseHoldId == user.HouseHoldId).ToList(),
                BudgetHistory = db.BudgetHistories.Where(u => u.HouseHoldId == user.HouseHoldId).ToList()
            };
            
            return View(hvm);
        }

        // GET: HouseHolds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseHold houseHold = db.HouseHolds.Find(id);
            if (houseHold == null)
            {
                return HttpNotFound();
            }
            return View(houseHold);
        }

        // GET: HouseHolds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HouseHolds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] HouseHold houseHold)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());                              
                db.HouseHolds.Add(houseHold);
                user.HouseHoldId = houseHold.Id;
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }

            return View(houseHold);
        }

        // GET: HouseHolds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseHold houseHold = db.HouseHolds.Find(id);
            if (houseHold == null)
            {
                return HttpNotFound();
            }
            return View(houseHold);
        }

        // POST: HouseHolds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] HouseHold houseHold)
        {
            if (ModelState.IsValid)
            {
                db.Entry(houseHold).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(houseHold);
        }

        // GET: HouseHolds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseHold houseHold = db.HouseHolds.Find(id);
            if (houseHold == null)
            {
                return HttpNotFound();
            }
            return View(houseHold);
        }

        // POST: HouseHolds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HouseHold houseHold = db.HouseHolds.Find(id);
            db.HouseHolds.Remove(houseHold);
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
       

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> RequestToJoin(string myemail)
        {
            Invitation invite = new Invitation();
            var user = db.Users.Find(User.Identity.GetUserId());
            await hh.SendEmailRequest(invite, user, myemail);       
            return RedirectToAction("Index", "HouseHolds");
        }
    }
}
