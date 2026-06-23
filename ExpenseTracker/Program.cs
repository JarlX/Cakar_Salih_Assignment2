// =====================================================================
//  Program.cs  —  the interactive console UI for MyBudget (Assignment 1).
//  Target framework: .NET 10 (LTS), language C# 14.
//
//  >>> BUILD THE MENU-DRIVEN UI HERE (Modules 1-3). <<<
//
//  Once you have implemented BudgetRules.cs (so the unit tests pass), wire it
//  up to a console interface that meets the assignment brief:
//
//    * Print a banner (try a raw string literal).
//    * Loop a menu until the user exits, using a switch on the choice:
//        1) Add an expense   2) View summary   3) Set monthly budget   4) Exit
//    * Read and VALIDATE input, re-prompting on bad data (decimal.TryParse,
//      BudgetRules.NormalizeCategory, a date parse, non-empty text).
//    * Keep running totals in simple variables (no collections / no classes).
//    * Use BudgetRules.ValidateAmount / ClassifyAmount / BudgetStatus /
//      FormatCurrency for all logic and formatting.
//    * Handle bad input with try / catch / finally and InvalidExpenseException.
//
//  See section 6 of the assignment brief for a sample run to aim for.
// =====================================================================
using ExpenseTracker;


const string banner = """
                      ============================================================
                        MyBudget Expense Tracker
                      ============================================================
                      """;

Console.WriteLine(banner);

bool runningMenu = true;
while (runningMenu)
{
    Console.WriteLine("\n 1) Add an expense   2) View Summary  3) Set Monthly Budget  4) Exit");
    Console.Write("->");
    
    var choice = Console.ReadLine() ?? "";
    
    switch (choice)
    {
        case "1":
            string description = "";
            while (string.IsNullOrWhiteSpace(description))
            {
                Console.Write("Please enter description: ");
                description = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(description))
                {
                    Console.WriteLine("Description can not be empty");
                }
            }

            decimal amount = 0m;
            while (amount == 0m)
            {
                Console.Write("Please enter amount: ");
                try
                {
                    if (!decimal.TryParse(Console.ReadLine(), out decimal parsedAmount))
                    {
                        throw new InvalidExpenseException("Please enter a valid amount");
                    }

                    amount = BudgetRules.ValidateAmount(parsedAmount);

                }
                catch (InvalidExpenseException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
            
            string? category = null;
            while (category is null)
            {
                Console.Write("Category    : [Food/Transport/Utilities/Entertainment/Other] ");
                category = BudgetRules.NormalizeCategory(Console.ReadLine());

                if (category is null)
                {
                    Console.WriteLine("Category can not be empty");
                }
            }
            
            var date = DateOnly.FromDateTime(DateTime.Now);
            bool validDate = false;
            while (!validDate)
            {
                Console.Write("Please enter date [Enter for Today]: ");
                string dateString = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(dateString))
                {
                    validDate = true;
                }
                else if (DateOnly.TryParse(dateString, out DateOnly parsedDate))
                {
                    if (parsedDate > DateOnly.FromDateTime(DateTime.Today))
                    {
                        Console.WriteLine("Hi, future traveller?");
                    }
                    else
                    {
                        date = parsedDate;
                        validDate = true;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid date, please try again. Format(yyyy-mm-dd)");
                }
            }
            
            Console.Write("Note (optional): ");
            string? optionalNote = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(optionalNote)) 
                optionalNote = null;
            break;
    }
    
}
