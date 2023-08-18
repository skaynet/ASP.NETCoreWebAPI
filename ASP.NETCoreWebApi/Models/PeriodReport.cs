using ASP.NETCoreWebApi.Utility;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASP.NETCoreWebApi.Models
{
    public class PeriodReport
    {
        [DataType(DataType.Date)]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime EndDate { get; set; }

        public float TotalIncome { get; set; }
        public float TotalExpenses { get; set; }
        public ICollection<FinancialTransaction> Transactions { get; set; } = new List<FinancialTransaction>();
    }
}