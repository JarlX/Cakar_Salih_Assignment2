namespace MyBudget.Core;

public static class ExpenseFactory
{

    public static decimal ValidateAmount(decimal amount)
    {
        if (amount <= 0 || amount > 1_000_000m)
        {
            throw new InvalidExpenseException("Amount must be greater than zero and no more than 1,000,000.");
        }

        return decimal.Round(amount, 2);
    }


    public static OneTimeExpense CreateOneTime(string description, decimal amount, ExpenseCategory category,
        DateOnly date)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidExpenseException("Description cannot be empty.");
        }
        
        amount = ValidateAmount(amount);
        description = description.Trim();
        return new OneTimeExpense(Guid.NewGuid(), description, amount, category, date);
    }

    public static RecurringExpense CreateRecurring(string description, decimal amount,
        ExpenseCategory category, DateOnly date, int timesPerMonth)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidExpenseException("Description cannot be empty.");
        }

        if (timesPerMonth < 1)
        {
            throw new InvalidExpenseException("TimesPerMonth must be greater than zero.");
        }
        
        amount = ValidateAmount(amount);
        description = description.Trim();
        return new RecurringExpense(Guid.NewGuid(), description, amount, category, date, timesPerMonth);
    }
}