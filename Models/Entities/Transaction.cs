using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        [Required]
        [Display(Name = "Betrag")]
        public decimal Amount { get; set; }
        [Display(Name = "Beschreibung")]
        public string? Description { get; set; }
        [Required]
        [Display(Name = "Datum")]
        public DateTime Date { get; set; }
        [Required]
        [Display(Name = "Konto")]
        public int? AccountId { get; set; }
        [Required]
        [Display(Name = "Konto")]
        virtual public Account? Account { get; set; }
        [Required]
        [Display(Name = "Kategorie")]
        public int? CategoryId { get; set; }
        [Required]
        [Display(Name = "Kategorie")]
        virtual public Category? Category { get; set; }
        public bool IsRecurring { get; set; } 
        public Frequency RecurrenceFrequency { get; set; } 
    }
    public enum Frequency
    {
        None,
        Daily,
        Weekly,
        Monthly
    }
}
