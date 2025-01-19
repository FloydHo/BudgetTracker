using BudgetTracker.Repositories;
using BudgetTracker.Models;

namespace BudgetTracker.Services
{
    public class AccountsService
    {
        public readonly TransactionRepository _transactionRepository;
        public AccountsService(TransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<List<(int aid, string name, decimal balance)>> GetAllAccountsByUserId(string userId)
        {
            List<Account> accounts = await _transactionRepository.GetAllAccountsByUserId(userId);

            var output = new List<(int aid, string name, decimal balance)>();

            foreach (var account in accounts)
            {
                if (account.Transactions == null) account.Transactions = new List<Transaction>();

                output.Add((account.AccountId, account.Name, account.Transactions.Sum(t => t.Amount)));
            }

            return output;
        }

        public async Task<Account> GetAccountById(int accountId)
        {
            return await _transactionRepository.GetAccountById(accountId);
        }

        public async Task AddAccount(string name, string userid)
        {
            Account account = new Account
            {
                Name = name,
                UserId = userid,
            };
            _transactionRepository.AddAccount(account);
        }

        public void DeleteAccount(int accountId)
        {
            _transactionRepository.DeleteAccount(accountId);
        }

        public void EditAccount(string name, int aid)
        {
            _transactionRepository.EditAccount(name, aid);
        }
    }
}
