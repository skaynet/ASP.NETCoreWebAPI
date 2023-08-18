using ASP.NETCoreWebApi.Models;
using ASP.NETCoreWebApi.Utility;
using System.Globalization;

namespace ASP.NETCoreWebApiTests
{
    public static class TestsData
    {
        public static List<OperationType> GetTestOperationTypes()
        {
            return new List<OperationType>
            {
                new OperationType { Id = 1, Name = "Income salary"},
                new OperationType { Id = 2, Name = "Share Income"},
                new OperationType { Id = 3, Name = "Other income"},
                new OperationType { Id = 4, Name = "Expenses: Products"},
                new OperationType { Id = 5, Name = "Expenses: Medicine"},
                new OperationType { Id = 6, Name = "Expenses: Insurance"},
                new OperationType { Id = 7, Name = "Expenses: Other"},
            };
        }

        public static List<FinancialTransaction> GetTestFinancialTransactions()
        {
            return new List<FinancialTransaction>() {
                new FinancialTransaction() { Id = 1, Description = "Received salary according to the contract", Amount = 8000, Date = DateTime.ParseExact("2023-06-01", JsonDateConverter.DateFormat, CultureInfo.InvariantCulture), TypeId = 1},
                new FinancialTransaction() { Id = 2, Description = "Paying for car insurance", Amount = -500, Date = DateTime.ParseExact("2023-06-01", JsonDateConverter.DateFormat, CultureInfo.InvariantCulture), TypeId = 6},
                new FinancialTransaction() { Id = 3, Description = "Income from Tesla shares", Amount = 1500, Date = DateTime.ParseExact("2023-06-05", JsonDateConverter.DateFormat, CultureInfo.InvariantCulture), TypeId = 2},
                new FinancialTransaction() { Id = 4, Description = "Payment of interest on a bank deposit", Amount = 300, Date = DateTime.ParseExact("2023-06-05", JsonDateConverter.DateFormat, CultureInfo.InvariantCulture), TypeId = 3},
                new FinancialTransaction() { Id = 5, Description = "Grocery shopping at Walmart", Amount = -2300, Date = DateTime.ParseExact("2023-06-05", JsonDateConverter.DateFormat, CultureInfo.InvariantCulture), TypeId = 4},
                new FinancialTransaction() { Id = 6, Description = "Dental services", Amount = -1500, Date = DateTime.ParseExact("2023-06-06", JsonDateConverter.DateFormat, CultureInfo.InvariantCulture), TypeId = 5},
                new FinancialTransaction() { Id = 7, Description = "Rest in the park", Amount = -700, Date = DateTime.ParseExact("2023-06-07" ,JsonDateConverter.DateFormat, CultureInfo.InvariantCulture), TypeId = 7}
            };
        }

        public static FinancialTransactionDTO FinancialTransactionToDTO(FinancialTransaction financialTransaction) =>
            new FinancialTransactionDTO
            {
                Id = financialTransaction.Id,
                Description = financialTransaction.Description,
                Amount = financialTransaction.Amount,
                Date = financialTransaction.Date,
                TypeId = financialTransaction.TypeId
            };
    }
}
