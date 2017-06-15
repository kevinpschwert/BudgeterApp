using BudgetApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BudgetApp.Helper;

namespace BudgetApp.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        BudgetHistoryHelper bhh = new BudgetHistoryHelper();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LogMeIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult LogMeIn1(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult DashBoard()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            bhh.History();

            //var myaccount = db.Accounts.Where(u => u.HouseHoldId == user.HouseHoldId).ToList();
            //var category = db.Categories.Where(u => u.HouseHoldId == user.HouseHoldId).ToList();
            //var transact = db.Transactions.Where(u => u.EnteredById == user.Id).ToList();
            //foreach (var item in myaccount)
            //{
            //   var old = db.Transactions.AsNoTracking().Where(u => u.AccountId == item.Id).ToList();
            //   foreach (var thing in old)
            //    {
            //        var yes = thing.Amount;
            //        var no = thing.Date;
            //        var maybe = thing.Description;
            //    }

            //}
            //}

            user.LastLogIn = DateTimeOffset.Now;
            db.SaveChanges();
            Transaction transaction = new Transaction();
            HouseHold house = new HouseHold();
            if (user.HouseHoldId == 2 || user.HouseHoldId == null)
            {
                return RedirectToAction("LogMeIn", "Home");
            }
            else
            {
                var myaccount = db.Accounts.First(u => u.HouseHoldId == user.HouseHoldId);
                var newuser = db.Users.Where(u => u.HouseHoldId == house.Id).ToList();

                if (myaccount == null)
                {
                    return RedirectToAction("Create", "HouseHolds");
                }                

                DashboardViewModel dvm = new DashboardViewModel()
                {
                    Account = db.Accounts.Where(u => u.HouseHoldId == user.HouseHoldId).ToList(),                    
                    Transaction = db.Transactions.Where(u => u.Account.HouseHoldId == user.HouseHoldId).Where(z => z.Date.Month == DateTimeOffset.Now.Month).ToList(),
                    Notification = db.Notifications.Where(u => u.HouseHoldId == user.HouseHoldId).ToList(),
                    BudgetHistory = db.BudgetHistories.Where(u => u.HouseHoldId == user.HouseHoldId).ToList()
                };

                return View(dvm);
            }           
        }

        public ActionResult Welcome()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Welcome(string welcome)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            HouseHold house = new HouseHold();
            house.Name = welcome;
            db.HouseHolds.Add(house);
            db.SaveChanges();
            user.HouseHoldId = house.Id;
            db.SaveChanges();
            return RedirectToAction("JoinAccount", "Home");
        }

        public ActionResult JoinAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult JoinAccount(string Name, double Balance)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            Account account = new Account();
            account.HouseHoldId = user.HouseHoldId ?? 1;
            account.Name = Name;
            account.Balance = Balance;
            account.ReconciledBalance = 0;
            db.Accounts.Add(account);
            db.SaveChanges();
            return RedirectToAction("Dashboard", "Home");
        }

        public ActionResult RegisterMe()
        {
            return View();
        }

        public ActionResult LeaveHoushold()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            user.HouseHoldId = 2;            
            db.SaveChanges();
            return RedirectToAction("LogMeIn", "Home");
        }       

        public ActionResult LogMeInJoin(string code)
        {
            ViewBag.Code = code;
            Invitation invite = db.Invitations.First(u => u.Code == code);
            return View(invite);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> RegisterJoin(int? id, string MyFirstName, string MyLastName, string MyDisplayName, string MyEmail, string MyConfirm, string MyPassword)
        {
            //RegisterViewModel rvm = new RegisterViewModel();
            //var user = new ApplicationUser { UserName = MyEmail, Email = MyEmail, DisplayName = MyDisplayName, FirstName = MyFirstName, LastName = MyLastName };
            //var result = await Microsoft.AspNet.Identity.UserManager.CreateAsync(user, MyPassword);
            //user.HouseHoldId = id;
            //rvm.FirstName = FirstName;
            //rvm.LastName = LastName;
            //rvm.DisplayName = DisplayName;
            //rvm.Email = Email;
            //user.UserName = Email;
            //user.PasswordHash = rvm.
            return View();
        }

        public ActionResult Muscatello()
        {
            return View();
        }

        public ActionResult Roelandts()
        {
            return View();
        }

        public ActionResult Lisowy()
        {
            return View();
        }

        public ActionResult Gustaphson()
        {
            return View();
        }

        public ActionResult McEwen()
        {
            return View();
        }

        public ActionResult Surprise()
        {
            return View();
        }
    }
}