using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BudgetTracker.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Transaction> Transactions { get; set; }

        public bool IsIncome { get; set; }
    }
}
