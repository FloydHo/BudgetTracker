using System.Diagnostics;
using BudgetTracker.Models;
using BudgetTracker.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers
{
    public class HomeController : Controller
    {
        public readonly TransactionRepository _transactionRepository;

        public HomeController(TransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }


        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "BudgetTracker");
            }
            return View();
        }
    }
}
