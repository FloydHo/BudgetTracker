using BudgetTracker.Models.Dtos;
using BudgetTracker.Models.ViewModel;
using BudgetTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers
{



    //Ein einziger Controller sollte genügen anstatt seperat für Dahboard, Transaction und Accounts
    [Authorize]
    public class BudgetTrackerController : Controller
    {
        private readonly TransactionService _transactionService;
        private readonly UserManager<IdentityUser> _userManager;

        public BudgetTrackerController(TransactionService transactionService, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _transactionService = transactionService;
        }

        public async Task<ActionResult> Dashboard()
        {
            int currentMonth = DateTime.Now.Month;
            int currentYear = 2024;
            decimal savingsOverTime = 0;

            List<TransactionDto> transactions = _transactionService.GetAllTransactionsByUserId((await _userManager.GetUserAsync(HttpContext.User)).Id).Result;

            DashboardViewModel dashboardViewModel = new DashboardViewModel
            {
                Total = transactions.Sum(t => t.Amount).ToString("0.00"),

                CurrentMonthIncome = transactions.Where(t => t.Amount > 0 && t.Date.Month == currentMonth && t.Date.Year == currentYear).Sum(t => t.Amount).ToString("0.00"),
                CurrentMonthExpense = transactions.Where(t => t.Amount < 0 && t.Date.Month == currentMonth && t.Date.Year == currentYear).Sum(t => t.Amount).ToString("0.00"),
                CurrentMonthSavings = transactions.Where(t => t.Date.Month == currentMonth && t.Date.Year == currentYear).Sum(t => t.Amount).ToString("0.00"),
            };

            DateTime twelveMonthsAgo = DateTime.Now.AddMonths(-12);
            decimal preMonthBalance = transactions.Where(t => t.Date < twelveMonthsAgo).Sum(t => t.Amount);

            for (int i = 11; i >= 0; i--)
            {
                int month = (currentMonth - i - 1 + 12) % 12 + 1;
                int year = 2025;

                if (month > currentMonth)
                {
                    year--;
                }

                string[] monthName = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                string[] dateLabel = { monthName[month - 1], (year%1000).ToString() };
                decimal monthlyIncome = transactions.Where(t => t.Amount > 0 && t.Date.Month == month && t.Date.Year == year).Sum(t => t.Amount);
                decimal monthlyExpense = transactions.Where(t => t.Amount < 0 && t.Date.Month == month && t.Date.Year == year).Sum(t => t.Amount);
                preMonthBalance += transactions.Where(t => t.Date.Month == month && t.Date.Year == year).Sum(t => t.Amount);

                dashboardViewModel.LineChartLabel.Add($"{dateLabel[0]}'{dateLabel[1]}");
                dashboardViewModel.IncomeDataByMonth.Add(monthlyIncome);
                dashboardViewModel.SavingsDataByMonth.Add(preMonthBalance);
                dashboardViewModel.ExpenseDataByMonth.Add(monthlyExpense*-1);
            }


            foreach (var acc in transactions.
                GroupBy(t => t.Account).
                Select(s => new 
                { 
                    Name = s.Key, 
                    Balance = s.Sum(t => t.Amount)
                }).ToList())
            {
                dashboardViewModel.AccountBalance.Add((acc.Name, acc.Balance.ToString("0.00")));
            }

            List<string> ExpensesInMonthByCategoryLabels = new List<string>();
            List<decimal> ExpensesInMonthByCategoryAmount = new List<decimal>();

            foreach (var category in transactions.
                Where(t => t.Amount < 0 && t.Date.Year == currentYear && t.Date.Month == currentMonth).
                GroupBy(t => t.Category).
                Select(s => new
                {
                    Category = s.Key,
                    Amount = s.Sum(t => t.Amount)
                }).ToList())
            {
                ExpensesInMonthByCategoryLabels.Add(category.Category);
                ExpensesInMonthByCategoryAmount.Add(category.Amount*-1);
            }

            dashboardViewModel.ExpensesInMonthByCategoryLabels = ExpensesInMonthByCategoryLabels.ToArray();
            dashboardViewModel.ExpensesInMonthByCategoryAmount = ExpensesInMonthByCategoryAmount.ToArray();

            return View(dashboardViewModel);
        }

        public async Task<ActionResult> Transactions()
        {
            List<TransactionDto> transactions = _transactionService.GetAllTransactionsByUserId((await _userManager.GetUserAsync(HttpContext.User)).Id).Result;
            return View(transactions);
        }

        public async Task<ActionResult> GetPartialAddTransaction()
        {
            return PartialView("Partial/_AddTransactionPartial");
        }

        //TODO: Nachschauen ob es mit unobtrusiv Ajax möglich anzuzeigen ob erfolgreich oder nicht
        public async Task AddTransaction(int category, string date, decimal amount, string name)
        {
            string userid = ((await _userManager.GetUserAsync(HttpContext.User)).Id);
            _transactionService.AddTransaction(userid, category, date, amount, name);
        }

    }


}
