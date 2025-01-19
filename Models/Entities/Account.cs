using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BudgetTracker.Models
{
    //Für mehrere Konten die ein User haben kann um genauer zu tracken wenn gewollt
    public class Account
    {
        public int AccountId { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 5)]
        public string Name { get; set; } 
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }

        [JsonIgnore]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
