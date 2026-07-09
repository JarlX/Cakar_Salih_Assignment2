using MyBudget.Core;
using MyBudget.Tests.Fakes;
using Xunit;

namespace MyBudget.Tests;

public class MyOwnTests
{
    [Fact]
    public void CreateOneTime_NegativeAmount_ThrowsInvalidExpenseException()
    {
        Assert.Throws<InvalidExpenseException>(() =>
            ExpenseFactory.CreateOneTime("Coffee", -5m, ExpenseCategory.Food, DateOnly.FromDateTime(DateTime.Today)));
    }

    [Fact]
    public void BudgetService_Evaluate_ReturnsNotSet_WhenLimitNotConfigured()
    {
        var service = new BudgetService();

        var status = service.Evaluate(50m);

        Assert.Equal(BudgetStatus.NotSet, status);
    }

    [Fact]
    public void ExpenseRepository_TotalsByCategory_GroupsAndSumsCorrectly()
    {
        var store = new InMemoryExpenseStore();
        var repository = new ExpenseRepository(store);

        var today = DateOnly.FromDateTime(DateTime.Today);
        repository.Add(ExpenseFactory.CreateOneTime("Lunch", 20m, ExpenseCategory.Food, today));
        repository.Add(ExpenseFactory.CreateOneTime("Dinner", 30m, ExpenseCategory.Food, today));
        repository.Add(ExpenseFactory.CreateOneTime("Bus", 10m, ExpenseCategory.Transport, today));

        var totals = repository.TotalsByCategory();

        Assert.Equal(50m, totals[ExpenseCategory.Food]);
        Assert.Equal(10m, totals[ExpenseCategory.Transport]);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    public void CreateRecurring_MonthlyImpact_MultipliesAmountByTimesPerMonth(int timesPerMonth)
    {
        var expense = ExpenseFactory.CreateRecurring(
            "Gym", 15m, ExpenseCategory.Entertainment, DateOnly.FromDateTime(DateTime.Today), timesPerMonth);

        Assert.Equal(15m * timesPerMonth, expense.MonthlyImpact);
    }
}