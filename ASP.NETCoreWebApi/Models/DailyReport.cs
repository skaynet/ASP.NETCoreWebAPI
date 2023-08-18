using ASP.NETCoreWebApi.Utility;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASP.NETCoreWebApi.Models
{
    public class DailyReport
    {
        [DataType(DataType.Date)]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime Date { get; set; }
        public float TotalIncome { get; set; }
        public float TotalExpenses { get; set; }
        public ICollection<FinancialTransaction> Transactions { get; set; } = new List<FinancialTransaction>();
    }
}