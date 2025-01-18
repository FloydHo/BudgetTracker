using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Models.Dtos
{
    public class TransactionDto 
    {
        public int TrasactionId { get; set; }
        public string Account { get; set; }
        public string Category { get; set; }
        public bool IsIncome { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
