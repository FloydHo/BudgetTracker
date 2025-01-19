using BudgetTracker.Models;
using BudgetTracker.Models.Dtos;
using BudgetTracker.Repositories;
using Microsoft.AspNetCore.Identity;

namespace BudgetTracker.Services
{
    public class TransactionService
    {
        public readonly TransactionRepository _transactionRepository;
        public TransactionService(TransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task<List<TransactionDto>> GetAllTransactionsByUserId(string userId)
        {
            List<Transaction> transactions = await _transactionRepository.GetAllTransactionsByUserId(userId);

            List<Account> accounts = await _transactionRepository.GetAllAccountsWTByUserId(userId);

            List<TransactionDto> transactionDtos = new List<TransactionDto>();

            foreach (var item in transactions)
            {
                transactionDtos.Add(new TransactionDto
                {
                    TrasactionId = item.TransactionId,
                    Account = item.Account.Name,
                    Category = item.Category.Name,
                    Amount = item.Amount,
                    Description = item.Description ?? "",
                    Date = item.Date
                });
            }

            // Damit auch Konten im Dashbaord dargestellt werden die eine Balance von 0 haben
            foreach (var account in accounts)
            {
                transactionDtos.Add(new TransactionDto
                {
                    TrasactionId = 0,
                    Account = account.Name,
                    Category = "",
                    Amount = 0,
                    Description = "",
                    Date = DateTime.Now
                });
            }



            return transactionDtos;
        }

        public async Task AddTransaction(string userid, int category, string date, decimal amount, string name, int accountId, bool isIncome = false)
        {


            Transaction transaction = new Transaction
            {

                AccountId = accountId,
                CategoryId = category,
                Amount = isIncome ? Math.Abs(amount) : Math.Abs(amount)*-1,
                Description = name,
                Date = DateTime.Parse(date),
                IsRecurring = false,
                RecurrenceFrequency = Frequency.None

            };
            await _transactionRepository.AddTransaction(transaction);
        }

        public async Task AddTransaction(Transaction transaction)
        {
            transaction.Amount = Math.Abs(transaction.Amount);
            if (!transaction.Category.IsIncome)
            {
                transaction.Amount *= -1;
            }
            await _transactionRepository.AddTransaction(transaction);
        }

        public async Task EditTransaction(int transactionId, int categoryId, string date, decimal amount,  string description, int accountId)
        {
            var transaction = _transactionRepository.GetTransactionById(transactionId);
            if (transaction != null)
            {
                //transaction.Account = await _transactionRepository.GetAccountById(accountId);
                transaction.Amount = _transactionRepository.GetCategoryById(categoryId).IsIncome ? Math.Abs(amount) : Math.Abs(amount) * -1;
                transaction.Description = description;
                transaction.Date = DateTime.Parse(date);
                transaction.AccountId = accountId;
                //transaction.Category = await _transactionRepository.GetCategoryById(categoryId);
                transaction.CategoryId = categoryId;

                _transactionRepository.UpdateTransaction(transaction);
            }
        }
    }
}
