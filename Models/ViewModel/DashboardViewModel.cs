namespace BudgetTracker.Models.ViewModel
{
    public class DashboardViewModel
    {
        /*****  Upper Dashboard  *****/

        public string Total { get; set; }

        public List<(string Name, string Balance)> AccountBalance { get; set; }

        public string CurrentMonthIncome { get; set; }

        public string CurrentMonthExpense { get; set; }

        public string CurrentMonthSavings { get; set; }

        /*****  ChartData  *****/

        //LineChart
        public List<string> LineChartLabel { get; set; }
        public List<decimal> IncomeDataByMonth { get; set; }
        public List<decimal> SavingsDataByMonth { get; set; }
        public List<decimal> ExpenseDataByMonth { get; set; }


        //PieChart

        public string[] ExpensesInMonthByCategoryLabels { get; set; }
        public decimal[] ExpensesInMonthByCategoryAmount { get; set; }



        public DashboardViewModel()
        {
            LineChartLabel = new List<string>();
            AccountBalance = new List<(string Name, string Balance)>();
            IncomeDataByMonth = new List<decimal>();
            SavingsDataByMonth = new List<decimal>();
            ExpenseDataByMonth = new List<decimal>();
   
        }   
    }
}
