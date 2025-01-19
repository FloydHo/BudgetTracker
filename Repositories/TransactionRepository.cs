using BudgetTracker.Data;
using BudgetTracker.Models;
using Microsoft.EntityFrameworkCore;


namespace BudgetTracker.Repositories
{
    public class TransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetAllTransactionsByUserId(string userId)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Account)
                .Where(t => t.Account.UserId == userId)
                .ToListAsync();
        }

        public Transaction GetTransactionById(int transactionId)
        {
            return _context.Transactions
                .FirstOrDefault(t => t.TransactionId == transactionId);
        }

        //TODO: Warum darf SaveChanges nicht async sein ? 
        public async Task AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        public void UpdateTransaction(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            var entityState = _context.Entry(transaction).State;
            Console.WriteLine($"Entity State: {entityState}");
            _context.SaveChanges();
        }

        public async Task<List<Account>> GetAllAccountsWTByUserId(string userId)
        {
            return await _context.Accounts
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Account>> GetAllAccountsByUserId(string userId)
        {
            return await _context.Accounts
                .Where(t => t.UserId == userId)
                .Include(t => t.Transactions)
                .ToListAsync();
        }

        public async Task<Account> GetAccountById(int accountId)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
        }

        public async Task AddAccount(Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();
        }

        public void DeleteAccount(int accountId)
        {
            var account = _context.Accounts.Include(t => t.Transactions).FirstOrDefault(a => a.AccountId == accountId);
            _context.Accounts.Remove(account);
            _context.SaveChanges();
        }

        public void EditAccount(string name, int aid)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.AccountId == aid);
            account.Name = name;
            _context.Accounts.Update(account);
            _context.SaveChanges();
        }

        public Category GetCategoryById(int categoryId)
        {
            return _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
        }
    }
}
