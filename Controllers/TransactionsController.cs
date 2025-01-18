using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetTracker.Data;
using BudgetTracker.Models;
using BudgetTracker.Services;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BudgetTracker.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly TransactionService _transactionService;
        private readonly UserManager<IdentityUser> _userManager;

        public TransactionsController(ApplicationDbContext context, TransactionService transactionService, UserManager<IdentityUser> userManager )
        {
            _userManager = userManager;
            _transactionService = transactionService;
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Transactions.Include(t => t.Account).Include(t => t.Category);
            return View(await applicationDbContext.OrderByDescending(o => o.Date).ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }


        /***********     Create       ******************/

        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(c => !c.IsIncome), "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(decimal Amount, string Description, string Date, int CategoryId)
        {

            if(Amount != 0)
            {
                string userid = ((await _userManager.GetUserAsync(HttpContext.User)).Id);
                _transactionService.AddTransaction(userid, CategoryId, Date, Amount, Description, false);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(c => !c.IsIncome), "CategoryId", "Name");
            return View();
        }

        public IActionResult CreateIncome()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(c => c.IsIncome), "CategoryId", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateIncome(decimal Amount,string Description, string Date, int CategoryId)
        {
            if (Amount != 0)
            {
                string userid = ((await _userManager.GetUserAsync(HttpContext.User)).Id);
                _transactionService.AddTransaction(userid, CategoryId, Date, Amount, Description, true);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(c => c.IsIncome), "CategoryId", "Name");
            return View();
        }


        /***********     Edit      ******************/

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", transaction.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionId,Amount,Description,Date,AccountId,CategoryId,IsRecurring,RecurrenceFrequency")] Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.TransactionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", transaction.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }
    }
}
