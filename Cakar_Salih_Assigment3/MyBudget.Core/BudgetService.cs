namespace MyBudget.Core;

public class BudgetService:IBudgetService
{
    public decimal MonthlyLimit { get; private set; }
    
    public void SetMonthlyLimit(decimal limit)
    {

        if (limit <= 0)
        {
            throw new InvalidExpenseException("Monthly limit must be greater than zero");
        }
        
        MonthlyLimit = decimal.Round(limit, 2);
    }

    public decimal Remaining(decimal totalSpent) => MonthlyLimit - totalSpent;
    

    public BudgetStatus Evaluate(decimal totalSpent)
    {
        if (MonthlyLimit == 0)
        {
            return BudgetStatus.NotSet;
        }

        decimal remaining = Remaining(totalSpent);

        return remaining switch
        {
            < 0 => BudgetStatus.OverBudget,
            _ when remaining < MonthlyLimit * 0.10m => BudgetStatus.AlmostOut,
            _ => BudgetStatus.OnTrack
        };
    }
}