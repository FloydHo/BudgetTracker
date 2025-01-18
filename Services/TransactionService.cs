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



            return transactionDtos;
        }

        public async void AddTransaction(string userid, int category, string date, decimal amount, string name, bool isIncome = false)
        {


            Transaction transaction = new Transaction
            {

                AccountId = 1,
                CategoryId = category,
                Amount = isIncome ? Math.Abs(amount) : Math.Abs(amount)*-1,
                Description = name,
                Date = DateTime.Parse(date),
                IsRecurring = false,
                RecurrenceFrequency = Frequency.None

            };
            await _transactionRepository.AddTransaction(transaction);
        }

        public async void AddTransaction(Transaction transaction)
        {
            transaction.Amount = Math.Abs(transaction.Amount);
            if (!transaction.Category.IsIncome)
            {
                transaction.Amount *= -1;
            }
            await _transactionRepository.AddTransaction(transaction);
        }
    }
}
