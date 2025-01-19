using BudgetTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly AccountsService _accountsService;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountsController(AccountsService accountsService, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _accountsService = accountsService;
        }

        public async Task<IActionResult> Index()
        {
            string userid = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            var accounts = await _accountsService.GetAllAccountsByUserId(userid);

            ViewData["Accounts"] = accounts;
            ViewData["Total"] = accounts.Sum(a => a.balance);

            return View();
        }

        public async Task<IActionResult> AddAccount(string name)
        {
            string userid = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            await _accountsService.AddAccount(name, userid);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int aid)
        {
            var account = await _accountsService.GetAccountById(aid);
            string userid = (await _userManager.GetUserAsync(HttpContext.User)).Id;

            if (account == null || account.UserId != userid)
            {
                return NotFound();
            }
            else
            {
                _accountsService.DeleteAccount(aid);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string name, int aid)
        {
            var account = await _accountsService.GetAccountById(aid);
            string userid = (await _userManager.GetUserAsync(HttpContext.User)).Id;

            if (account == null || account.UserId != userid)
            {
                return NotFound();
            }
            else
            {
                if (String.IsNullOrWhiteSpace(name) || name.Length < 5)
                {
                    return NotFound();
                }
                _accountsService.EditAccount(name, aid);
            }


            return RedirectToAction("Index");
        }
    }
}
