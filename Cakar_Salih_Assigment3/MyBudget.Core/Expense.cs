using System.Text.Json.Serialization;

namespace MyBudget.Core;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(OneTimeExpense), "onetime")]
[JsonDerivedType(typeof(RecurringExpense), "recurring")]
public abstract record Expense(Guid Id, string Description,
    decimal Amount, ExpenseCategory Category, DateOnly Date) : IReportable
{
    public abstract decimal MonthlyImpact { get;}

    public virtual string ToReportLine() =>
        $"{Date} | {Category} | ${Amount:0.00} | {Description}";
    
}

public sealed record OneTimeExpense(Guid Id, string Description,
    decimal Amount, ExpenseCategory Category, DateOnly Date)
    : Expense(Id, Description, Amount, Category, Date)
{
    public override decimal MonthlyImpact => Amount;
}


public sealed record RecurringExpense(Guid Id, string Description,
    decimal Amount, ExpenseCategory Category, DateOnly Date, int TimesPerMonth)
    : Expense(Id, Description, Amount, Category, Date)
{
    public override decimal MonthlyImpact => Amount * TimesPerMonth;

    public override string ToReportLine() =>
        base.ToReportLine() + $" (x{TimesPerMonth}/month)";
}