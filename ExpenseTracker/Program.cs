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



decimal monthlyBudget = 0m;
decimal totalSpent = 0m;
int expenseCount = 0;
decimal highestExpense = 0m;


decimal foodTotal = 0m;
decimal transportTotal = 0m;
decimal utilitiesTotal = 0m;
decimal entertainmentTotal = 0m;
decimal otherTotal = 0m;

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
                finally
                {
                    if (amount > 0m)
                    {
                        Console.WriteLine($"{BudgetRules.FormatCurrency(amount)} is added!");
                    }
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

            totalSpent += amount;
            expenseCount++;

            if (amount > highestExpense)
            {
                highestExpense = amount;
            }

            switch (category)
            {
                case "Food":
                    foodTotal += amount;
                    break;
                case "Transport":
                    transportTotal += amount;
                    break;
                case "Utilities":
                    utilitiesTotal += amount;
                    break;
                case "Entertainment":
                    entertainmentTotal += amount;
                    break;
                case "Other":
                    otherTotal += amount;
                    break;
            }

            string size = BudgetRules.ClassifyAmount(amount);
            
            string displayDesc = description.Length > 20 
                ? description[..20] + "..." + description[^1]
                : description;

            
            Console.WriteLine($"Added : {BudgetRules.FormatCurrency(amount)} | {category} | {date}");
            Console.WriteLine($"Description: {displayDesc}");
            Console.WriteLine($"Size :  {size}");

            if (monthlyBudget > 0)
            {
                decimal remainingBudget = monthlyBudget - totalSpent;
                string statusOfBudget = BudgetRules.BudgetStatus(remainingBudget, monthlyBudget);
                
                Console.WriteLine($"Budget: {BudgetRules.FormatCurrency(remainingBudget)} remaining of {BudgetRules.FormatCurrency(monthlyBudget)} --> {statusOfBudget}");
            }
            
            break;
        case "2" :
            if (expenseCount == 0)
            {
                Console.WriteLine("No expenses yet.");
            }
            else
            {
                decimal avg = totalSpent / expenseCount;
                Console.WriteLine($"  Expenses    : {expenseCount}");
                Console.WriteLine($"  Total spent : {BudgetRules.FormatCurrency(totalSpent)}");
                Console.WriteLine($"  Average     : {BudgetRules.FormatCurrency(avg)}");
                Console.WriteLine($"  Highest     : {BudgetRules.FormatCurrency(highestExpense)}");
                Console.WriteLine($"\n  Food        : {BudgetRules.FormatCurrency(foodTotal)}");
                Console.WriteLine($"  Transport   : {BudgetRules.FormatCurrency(transportTotal)}");
                Console.WriteLine($"  Utilities   : {BudgetRules.FormatCurrency(utilitiesTotal)}");
                Console.WriteLine($"  Entertainment: {BudgetRules.FormatCurrency(entertainmentTotal)}");
                Console.WriteLine($"  Other       : {BudgetRules.FormatCurrency(otherTotal)}");

                if (monthlyBudget > 0)
                {
                    decimal remaining = monthlyBudget - totalSpent;
                    string status = BudgetRules.BudgetStatus(remaining, monthlyBudget);
                    Console.WriteLine($"\n  Budget: {BudgetRules.FormatCurrency(remaining)} remaining of {BudgetRules.FormatCurrency(monthlyBudget)} -> {status}");
                }
            }
            break;
        case "3" :
            Console.WriteLine("Monthly Budget: ");

            if (decimal.TryParse(Console.ReadLine(), out decimal budget) && budget > 0m)
            {
                monthlyBudget = budget;
                Console.WriteLine($"Monthly Budget: {BudgetRules.FormatCurrency(monthlyBudget)}");

                if (monthlyBudget > 0)
                {
                    decimal remainingBudget = monthlyBudget - totalSpent;
                    string statusOfBudget = BudgetRules.BudgetStatus(remainingBudget, monthlyBudget);
                    Console.WriteLine($"Budget is {BudgetRules.FormatCurrency(remainingBudget)} remaining of {BudgetRules.FormatCurrency(monthlyBudget)} | {statusOfBudget}");
                }
            }
            else
            {
                Console.WriteLine("Invalid input, please enter valid amount.");
            }
            break;
        
        case "4" :
            runningMenu = false;
            Console.WriteLine("See you!");
            break;
        default:
            Console.WriteLine("Invalid choice, please try again");
            break;
    }
    
}
