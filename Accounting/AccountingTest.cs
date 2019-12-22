using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AccountingTest
{
    public class AccountingTest
    {
        private List<Budget> _budgets;
        private Accounting _accounting;
        private DateTime _startDate;
        private DateTime _endDate;

        private void GivenBudgets()
        {
            var janBudget = new Budget() { YearMonth = "201901", Amount = 31 };
            var febBudget = new Budget() { YearMonth = "201902", Amount = 280 };
            var marBudget = new Budget() { YearMonth = "201903", Amount = 3100 };
            var decBudget = new Budget() { YearMonth = "201912", Amount = 31000 };

            var janBudget2020 = new Budget() { YearMonth = "202001", Amount = 62 };

            _budgets = new List<Budget> { janBudget, febBudget, marBudget, decBudget, janBudget2020 };
        }

        private void CreateAccounting()
        {
            var budgetRepository = new BudgetRepository(_budgets);
            _accounting = new Accounting(budgetRepository);
        }

        private void BudgetShouldBe(int expected)
        {
            Assert.AreEqual(expected, _accounting.QueryBudget(_startDate, _endDate));
        }

        [SetUp]
        public void Setup()
        {
            GivenBudgets();
            CreateAccounting();
        }

        [Test]
        public void Get_Single_Month_Budget()
        {
            _startDate = new DateTime(2019, 1, 1);
            _endDate = new DateTime(2019, 1, 31);

            BudgetShouldBe(31);
        }

        [Test]
        public void Get_Two_Month_Budget()
        {
            _startDate = new DateTime(2019, 1, 1);
            _endDate = new DateTime(2019, 2, 28);
            BudgetShouldBe(311);
        }

        [Test]
        public void Get_Partial_Month_Budget()
        {
            _startDate = new DateTime(2019, 1, 15);
            _endDate = new DateTime(2019, 1, 30);
            BudgetShouldBe(16);
        }

        [Test]
        public void Get_Cross_Year_Budget()
        {
            _startDate = new DateTime(2019, 12, 29);
            _endDate = new DateTime(2020, 1, 1);
            BudgetShouldBe(3002); // $1000 * 3days + $2 * 1days = 3002
        }
    }
}