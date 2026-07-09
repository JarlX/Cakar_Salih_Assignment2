// =====================================================================
//  MyBudget — Assignment 3 entry point (composition root).
//
//  >>> YOU WRITE THE DEPENDENCY-INJECTION WIRING HERE (Module 8). <<<
//
//  The ConsoleApp UI is provided and depends only on the service
//  abstractions. Register your implementations with the container so that
//  ConsoleApp can be resolved. See the "Build specification" in the brief.
// =====================================================================
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyBudget.App;
using MyBudget.Core;
using MyBudget.Data;

var builder = Host.CreateApplicationBuilder(args);

string dataPath = Path.Combine(AppContext.BaseDirectory, "expenses.json");

builder.Services.AddSingleton<IExpenseStore>(new JsonExpenseStore(dataPath));
builder.Services.AddSingleton<IExpenseRepository, ExpenseRepository>();
builder.Services.AddSingleton<IBudgetService, BudgetService>();
builder.Services.AddSingleton<ConsoleApp>();

using IHost host = builder.Build();

host.Services.GetRequiredService<ConsoleApp>().Run();
