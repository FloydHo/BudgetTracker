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

        //TODO: Warum darf SaveChanges nicht async sein ? 
        public async Task AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }
    }
}
