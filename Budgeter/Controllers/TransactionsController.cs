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
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        BudgetHelper bh = new BudgetHelper();

        // GET: Transactions
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var account = db.Accounts.Where(u => u.HouseHoldId == user.HouseHoldId).ToList();
            var category = db.Categories.Where(u => u.HouseHoldId == user.HouseHoldId).ToList();
            ViewBag.AccountId = new SelectList(account, "Id", "Name");
            ViewBag.CategoryId = new SelectList(category, "Id", "Name");
            //ViewBag.EnteredById = new SelectList(db.Users, "Id", "DisplayName");
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name");
            var myaccount = db.Accounts.First(u => u.HouseHoldId == user.HouseHoldId);
            //var transactions = db.Transactions.Where(u => u.EnteredById == user.Id || u.AccountId == myaccount.Id).Where(z => z.Date.Month == DateTimeOffset.Now.Month).ToList();
            TransactionViewModel tvm = new TransactionViewModel
            {
                Transaction = db.Transactions.Where(u => u.Account.HouseHoldId == user.HouseHoldId).Where(z => z.Date.Month == DateTimeOffset.Now.Month).ToList(),
                BudgetHistory = db.BudgetHistories.Where(u => u.HouseHoldId == user.HouseHoldId).ToList()
            };
            
            return View(tvm);
        }
        

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var account = db.Accounts.Where(u => u.HouseHoldId == user.HouseHoldId).ToList();          

            ViewBag.AccountId = new SelectList(account, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            //ViewBag.EnteredById = new SelectList(db.Users, "Id", "DisplayName");
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,CategoryId,TransactionTypeId")] Transaction transaction, string datepicker, string Description, string Amount)
        {
            if (ModelState.IsValid)
            {
                bh.ManageAccount(transaction, datepicker, Description, Amount);
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "DisplayName", transaction.EnteredById);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name", transaction.TransactionTypeId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            var user = db.Users.Find(User.Identity.GetUserId());
            var entereduser = db.Users.Where(u => u.Id == user.Id);
            var category = db.Categories.Where(u => u.HouseHoldId == user.HouseHoldId).ToList();
            var account = db.Accounts.Where(u => u.HouseHoldId == user.HouseHoldId).ToList();
            ViewBag.AccountId = new SelectList(account, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(category, "Id", "Name", transaction.CategoryId);
            ViewBag.EnteredById = new SelectList(entereduser, "Id", "DisplayName", transaction.EnteredById);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name", transaction.TransactionTypeId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,AccountId,Date,Amount,EnteredById,CategoryId,Reconciled,ReconciledAmount,TransactionTypeId,Void")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                bh.AmountChanged(transaction);
                if (transaction.Void == true)
                {
                    bh.Void(transaction);
                }  
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "DisplayName", transaction.EnteredById);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name", transaction.TransactionTypeId);
            return View(transaction);
        }

        public ActionResult AccountTransactions()
        {
            
            var user = db.Users.Find(User.Identity.GetUserId());
            var account = db.Accounts.Where(u => u.HouseHoldId == user.HouseHoldId).ToList();
            AccountsViewModel avm = new AccountsViewModel()
            {
                Account = db.Accounts.Where(u => u.HouseHoldId == user.HouseHoldId).ToList(),
                BudgetHistory = db.BudgetHistories.Where(u => u.HouseHoldId == user.HouseHoldId).ToList()
            };
            return View(avm);
        }

        public ActionResult CategoryTransactions()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            HouseHoldViewModel hvm = new HouseHoldViewModel
            {
                Category = db.Categories.Where(u => u.HouseHoldId == user.HouseHoldId).ToList(),
                Transaction = db.Transactions.ToList(),
                BudgetHistory = db.BudgetHistories.Where(u => u.HouseHoldId == user.HouseHoldId).ToList()
            };
            return View(hvm);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
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
