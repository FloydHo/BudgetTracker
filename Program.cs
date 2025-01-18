using BudgetTracker.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using BudgetTracker.Models;
using BudgetTracker.Repositories;
using BudgetTracker.Services;

namespace BudgetTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<TransactionRepository>();
            builder.Services.AddScoped<TransactionService>();

            var app = builder.Build();


            /***********  Mock Data Creation  ***************/

            //using (var scope = app.Services.CreateScope())
            //{
            //    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //    CreateMockData(context);
            //}


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        public static void CreateMockData(ApplicationDbContext context)
        {

            var userId = "4c85580d-89ad-4499-8ffd-37040aeb7699";

            var account = new Account
            {
                UserId = userId,
                Name = "Giro Konto"
            };
            context.Accounts.Add(account);

            var categories = new List<Category>
            {
                new Category { Name = "Einkommen" },
                new Category { Name = "Lebensmittel" },
                new Category { Name = "Freizeit" },
                new Category { Name = "Hobby" },
                new Category { Name = "Sonstiges" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            var transactions = new List<Transaction>();
            var random = new Random();

            for (int month = 1; month <= 12; month++)
            {
                transactions.Add(new Transaction
                {
                    AccountId = account.AccountId,
                    CategoryId = categories.First(c => c.Name == "Einkommen").CategoryId,
                    Amount = 2000m,
                    Description = "Monatliches Einkommen",
                    Date = new DateTime(2024, month, 1),
                    IsRecurring = true,
                    RecurrenceFrequency = Frequency.Monthly
                });

                foreach (var category in categories.Where(c => c.Name != "Einkommen"))
                {
                    transactions.Add(new Transaction
                    {
                        AccountId = account.AccountId,
                        CategoryId = category.CategoryId,
                        Amount = Math.Round((decimal)random.Next(50, 500) * -1, 2), 
                        Description = $"Ausgabe für {category.Name}",
                        Date = new DateTime(2024, month, random.Next(1, 28)),
                        IsRecurring = false,
                        RecurrenceFrequency = Frequency.None
                    });
                }
            }

            context.Transactions.AddRange(transactions);
            context.SaveChanges();

            Console.WriteLine("Dummy-Daten erfolgreich generiert!");
            Console.WriteLine($"Benutzer-ID: {userId}");
            Console.WriteLine($"Account-ID: {account.AccountId}");
            Console.WriteLine("Transaktionen:");
            foreach (var transaction in context.Transactions.Include(t => t.Category))
            {
                Console.WriteLine($"{transaction.Date:yyyy-MM-dd} | {transaction.Amount:C} | {transaction.Description} | Kategorie: {transaction.Category?.Name ?? "Keine"}");
            }

        }
    }
}
